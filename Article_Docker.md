# How to Containerize EMA C# Project and Solution

## <a id="intro"></a>Introduction

[Real-Time SDK (C# Edition)](https://developers.lseg.com/en/api-catalog/refinitiv-real-time-opnsrc/refinitiv-real-time-csharp-sdk) (RTSDK, formerly known as Elektron SDK) is a suite of modern and open source APIs ([GitHub](https://github.com/Refinitiv/Real-Time-SDK)) that aim to simplify development through a strong focus on ease of use and standardized access to LSEG Real-Time Platform via the proprietary TCP connection named RSSL and proprietary binary message encoding format named OMM Message. The capabilities range from low latency/high-performance APIs right through to simple streaming Web APIs.

The RTSDK C# Edition can run on Windows, Oracle Linux Server, Red Hat Enterprise Server and Ubuntu Linux platforms. It supports the [Visual Studio 2022 IDE](https://visualstudio.microsoft.com/vs/) for the full features development experience but the IDE is available for Windows developers only. Fortunately, the RTSDK C# Edition also supports the cross-platform [.NET SDK 6](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-6) (aka .NET Core 6) framework and the [Visual Studio Code](https://code.visualstudio.com/) (aka VS Code) editor is available for all major OS platforms. Linux and Windows developers who are using the VS Code editor can implement the real-time streaming application with LSEG Real-Time platform using the RTSDK C# Edition.

This document explains how to containerize the EMA C# Project and Solution. I am using [Docker](https://www.docker.com/) as a main container orchestration tool in this document.

**Note**: Please note that the Real-Time SDK isn't qualified on the Docker platform. If you find any problems while running it on the Docker platform, the issues must be replicated on bare metal machines before contacting the helpdesk/Real-Time APIs support.

## What is a Container?

According to [Docker](https://www.docker.com/resources/what-container/) and [Azure](https://azure.microsoft.com/en-in/resources/cloud-computing-dictionary/what-is-a-container), and [GCP](https://cloud.google.com/learn/what-are-containers) documents, a container is a standard unit of software that packages up code and all its dependencies and configurations files required for the app to run. The application's container can runs quickly and reliably from one computing environment to another. Multiple containers can run on the same machine and share the OS kernel with other containers, each running as isolated processes in user space. The containerized application can be tested as a unit and deployed as a container image instance to the host operating system.

![figure-1](images/container/container_1.png)

Image from [Azure website](https://azure.microsoft.com/en-in/resources/cloud-computing-dictionary/what-is-a-container).

Container Benefits:

- Enable developers and IT professionals to deploy applications across environments with little or no modification.
- Containers take up less space than Virtual Machines.
- Streamline the whole dev and test cycle by shipping the application as container which is easy for deployment.
- Increases collaboration and efficiency between dev and operations teams to ship apps faster.
- Portability between OS platforms and between clouds.
- Container is lightweight, so many containers can be supported on the same infrastructure.
- Easy for rapid scale-up and scale-down scenarios.

## Containerize EMA C# Project and Solution

The [.NET Docker Images](https://github.com/dotnet/dotnet-docker) are hosted on the [Microsoft Container Registry repository](https://mcr.microsoft.com/en-us/). We can use a [Docker desktop application](https://www.docker.com/products/docker-desktop/) to pull .NET Docker Images directly.

### Docker ignore file

Before I am going further, let's start by creating a ```.dockerignore``` file inside a ```ema_solution``` folder to define the files/folders that we do not want Docker to build or copy to an image. Those files and folders are the compiled files/folders in our environment and a ```.env``` file.

```ini
EMAConsumer/bin/
EMAConsumer/obj/
EMAConsumer/.env
EMAConsumer/.env.example
JSONUtil/bin/
JSONUtil/obj/
global.json
```

**Note**: A ```global.json``` file is for defining .NET SDK version in a local environment only. We are going to use .NET Docker Images to build the project, so that file is not need for a Docker image.

### Let's build our first image

The easiest way is to copy our project/solution source code to a container and use [.NET SDK images](https://hub.docker.com/_/microsoft-dotnet-sdk/) to build and run the application. I am demonstrating with the ```ema_solution``` project with the ```mcr.microsoft.com/dotnet/sdk:6.0``` .NET 6 Docker image. A ```Dockerfile``` (inside a ```ema_solution``` folder) is as follows:

```dockerfile
FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/sdk:6.0
LABEL maintainer="Developer Relations"

# Create app directory
WORKDIR /App

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish --configuration Release -o out
#Copy EmaConfig.xml
COPY EMAConsumer/EmaConfig.xml .
#Run the application
ENTRYPOINT ["dotnet","/App/out/EMAConsumer.dll"]
```

Please be noticed that I am setting the optional ```--platform=linux/amd64``` flag  to make sure our image is always in *linux/amd64* platform.

Then build a docker image from with the following [docker build](https://docs.docker.com/reference/cli/docker/image/build/) command:

```bash
docker build . -t emacsharp_solution
```

Once a build process is successful, you can use a [docker images](https://docs.docker.com/reference/cli/docker/image/ls/) command to check our newly created Docker image.

![figure-2](images/container/container_2.png)

We can run a EMA C# project container via a [docker run](https://docs.docker.com/reference/cli/docker/container/run/) command as follows (you should have a ```.env``` file that have your Authentication Version 2 define in it):

```bash
docker run -it --name emacsharp_solution --env-file EMAConsumer/.env emacsharp_solution
```

Result:

![figure-3](images/container/container_3.png)

However, our images isn't optimized yet. The image size is also 893mb which is very huge. It is ideally to share smaller images among the teams.

### Optimize dotnet Restore

By default, each instruction in a Dockerfile translates to a layer in your final image in an order specified. If Docker finds that you have already executed a similar instruction before, Docker skips that layer and uses the cached result instead. However, if there is a layer that have been changed (updated code, add new instruction, etc.), that layer will need to be re-built. And if a layer is changed, all other layers that come after it are also affected and need to re-built too.

This is called a *Docker build cache*. One main target of a Dockerfile optimization is structuring a Dockerfile to reuse results from previous builds and skipping unnecessary work as much as possible.

You can find more detail about a Docker build cache behavior from the following resources:

- [Docker build cache document](https://docs.docker.com/build/cache/)
- [Using the build cache document](https://docs.docker.com/guides/docker-concepts/building-images/using-the-build-cache/)

For .NET application, it is advisable to copy only the **.csproj**, **.sln**, and **nuget.config** files for your application before performing a [dotnet restore](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-restore) command. By copying those files first, Docker can cache the project dependencies restoration result, and the dependencies restoration process will not be effected by just a code changed.

```dockerfile
FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/sdk:6.0
LABEL maintainer="Developer Relations"

# Create app directory
WORKDIR /App

# Copy project files
COPY ema_solution.sln ./
COPY EMAConsumer/EMAConsumer.csproj /App/EMAConsumer/EMAConsumer.csproj
COPY JSONUtil/JSONUtil.csproj /App/JSONUtil/JSONUtil.csproj
# Restore as distinct layers
RUN dotnet restore
# Copy everything else
COPY . ./
# Build and publish a release
RUN dotnet publish --configuration Release -o out

#Copy EmaConfig.xml
COPY EMAConsumer/EmaConfig.xml .
#Run the application
ENTRYPOINT ["dotnet","/App/out/EMAConsumer.dll"]
```

Now, we have optimized our Dockerfile to copy the solution *.sln* and *.csproj* files and run a *dotnet restore* command before copy the source code. However, if you run a *docker images* command you will be noticed that the image size does not change. How can we reduce our image size?

### Multi-Stage Build

[tbd]

## Reference

- [Docker: Containerize a .NET application](https://docs.docker.com/language/dotnet/containerize/)
- [Microsoft Learn: Tutorial - Containerize a .NET app](https://learn.microsoft.com/en-us/dotnet/core/docker/build-container?tabs=windows&pivots=dotnet-8-0)
- [Docker Blog: 9 Tips for Containerizing Your .NET Application](https://www.docker.com/blog/9-tips-for-containerizing-your-net-application/)
-
