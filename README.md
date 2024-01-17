# Step By Step Guide EMA C# project with VS Code 
- version: 1.0.0
- Last Update: January 2024
- Environment: Ubuntu or Windows
- Compiler: .NET 6.0
- Prerequisite: [prerequisite](#prerequisite)

Example Code Disclaimer:
ALL EXAMPLE CODE IS PROVIDED ON AN “AS IS” AND “AS AVAILABLE” BASIS FOR ILLUSTRATIVE PURPOSES ONLY. REFINITIV MAKES NO REPRESENTATIONS OR WARRANTIES OF ANY KIND, EXPRESS OR IMPLIED, AS TO THE OPERATION OF THE EXAMPLE CODE, OR THE INFORMATION, CONTENT, OR MATERIALS USED IN CONNECTION WITH THE EXAMPLE CODE. YOU EXPRESSLY AGREE THAT YOUR USE OF THE EXAMPLE CODE IS AT YOUR SOLE RISK.

## <a id="intro"></a>Introduction

[Real-Time SDK (C# Edition)](https://developers.lseg.com/en/api-catalog/refinitiv-real-time-opnsrc/refinitiv-real-time-csharp-sdk) (RTSDK, formerly known as Elektron SDK) is a suite of modern and open source APIs ([GitHub](https://github.com/Refinitiv/Real-Time-SDK)) that aim to simplify development through a strong focus on ease of use and standardized access to a broad set of Refinitiv proprietary content and services via the proprietary TCP connection named RSSL and proprietary binary message encoding format named OMM Message. The capabilities range from low latency/high-performance APIs right through to simple streaming Web APIs. 

The RTSDK C# Edition can run on Windows, Oracle Linux Server, Red Hat Enterprise Server and Ubuntu Linux platforms. It supports the [Visual Studio 2022 IDE](https://visualstudio.microsoft.com/vs/) for the full features development experience but the IDE is available for Windows developers only. Fortunately, the RTSDK C# Edition also supports the cross-platform [.NET SDK 6](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-6) (aka .NET Core 6) framework and the [Visual Studio Code](https://code.visualstudio.com/) (aka VS Code) editor is available for all major OS platforms. Linux and Windows developers who are using the VS Code editor can implement the real-time streaming application with LSEG Real-Time platform using the RTSDK C# Edition.

This example project shows a step-by-step guide to create the EMA API .NET project and solution with the RTSDK C# Edition on VS Code and the [C# extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp). I am demonstrating with the RTSDK C# version 2.1.3.L1 and it can be applied to any supported OS platforms.

## <a id="prerequisite"></a>Prerequisite

Before I am going further, there is some prerequisite, dependencies, and libraries that the project is needed.

### .NET SDK

Firstly, you need .NET 6 SDK. You can download the SDK based on your system from [Microsoft .NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) website.

### Visual Studio Code

Next, the [VS Code](https://code.visualstudio.com/) editor tool.

### Access to the RTO

This project uses RTO access credentials for Version 2 Authentication (Service ID).

Please contact your LSEG representative to help you with the RTO account and services.

### Internet Access

The RTSDK C# libraries (both ETA and EMA APIs) are available on the [NuGet](https://www.nuget.org/) package manager and distribution platform. You can use the built-in VS Code CLI command to download the EMA and ETA libraries from NuGet over internet. 

## Create a single project

Let's me start with how to create a single EMA C# .NET project. 


### Initialize Project

Firstly, create a new folder name ```ema_project``` in your workbench environment.

```bash
$>mkdir ema_project
```

Next, access the ```ema_project``` and create a new console project with the following command:

```bash
$>ema_project>dotnet new console --framework net6.0 --use-program-main
```
Example result:

```bash
C:\>cd ema_project

C:\ema_project>dotnet new console --framework net6.0 --use-program-main
The template "Console App" was created successfully.

Processing post-creation actions...
Restoring C:\ema_project\ema_project.csproj:
  Determining projects to restore...
  Restored C:\ema_project.csproj (in 76 ms).
Restore succeeded.


C:\ema_project>
```

The ```dotnet new console --framework net6.0 --use-program-main``` command creates a new console project with .NET 6.0 framework for you. The command also create a simple application ```Program.cs``` for as a starting point for the project as follows:

![figure-1](images/01_emaproj_folder.png "dotnet new project command result")

We can test our project creation by running this ```Program.cs``` with the following command:

```bash
$>ema_project>dotnet run

Hello, World!
```
### Add EMA Library

The RTSDK C# libraries are available on the [NuGet](https://www.nuget.org/) package manager and distribution platform. We can add the [EMA Core library](https://www.nuget.org/packages/LSEG.Ema.Core) into the project with the ```dotnet add package LSEG.Ema.Core --version {version}``` command as follows.

```bash
$>ema_project>dotnet add package LSEG.Ema.Core --version 3.1.0
```
Result:
```bash
C:\ema_project>dotnet add package LSEG.Ema.Core --version 3.1.0
  Determining projects to restore...
  Writing C:\Users\<user>\AppData\Local\Temp\tmpEB94.tmp
info : X.509 certificate chain validation will use the default trust store selected by .NET.
info : X.509 certificate chain validation will use the default trust store selected by .NET.
info : Adding PackageReference for package 'LSEG.Ema.Core' into project 'C:\ema_project\ema_project.csproj'.info : Restoring packages for C:\ema_project\ema_project.csproj...
info :   GET https://api.nuget.org/v3-flatcontainer/lseg.ema.core/index.json
info :   OK https://api.nuget.org/v3-flatcontainer/lseg.ema.core/index.json 1492ms
info :   GET https://api.nuget.org/v3-flatcontainer/lseg.ema.core/3.1.0/lseg.ema.core.3.1.0.nupkg
info :   OK https://api.nuget.org/v3-flatcontainer/lseg.ema.core/3.1.0/lseg.ema.core.3.1.0.nupkg 829ms
....
info : Package 'LSEG.Ema.Core' is compatible with all the specified frameworks in project 'C:\ema_project\ema_project.csproj'.
info : PackageReference for package 'LSEG.Ema.Core' version '3.1.0' added to file 'C:\ema_project\ema_project.csproj'.
info : Writing assets file to disk. Path: C:\ema_project\obj\project.assets.json
log  : Restored C:\ema_project.csproj (in 6.7 sec).

C:\ema_project>
```
You can use ```dotnet list package``` to verify the EMA library package installation.

```bash
C:\ema_project>dotnet list package
Project 'ema_project' has the following package references
   [net6.0]: 
   Top-level Package      Requested   Resolved
   > LSEG.Ema.Core        3.1.0       3.1.0  

```
Now the ema_project is ready for implementing the real-time application with EMA C# API.

### Add the Real-Time Application Source Code with EMA

[to be done]

## A single project

1. dotnet new console --framework net6.0 --use-program-main
2. dotnet add package LSEG.Ema.Core --version 3.1.0
3. dotnet build
4. dotnet run

## A Solution
1. dotnet new sln
2. dotnet new console --framework net6.0 -o EMAConsumer --use-program-main
3. dotnet sln add EMAConsumer/EMAConsumer.csproj
4. cd EMAConsumer/
5. dotnet add package LSEG.Ema.Core --version 3.1.0
6. cd ..
7. dotnet build
8. dotnet run

## Docker
1. docker build . -t dotnetema
2. docker run -it --name dotnetema dotnetema

# Links
1. https://learn.microsoft.com/en-us/dotnet/core/tutorials/library-with-visual-studio-code?pivots=dotnet-6-0
2. https://learn.microsoft.com/en-us/dotnet/core/docker/build-container?tabs=windows&pivots=dotnet-7-0
3. https://hub.docker.com/_/microsoft-dotnet-sdk/
4. https://hub.docker.com/_/microsoft-dotnet-runtime/
5. https://www.nuget.org/packages/LSEG.Ema.Core 