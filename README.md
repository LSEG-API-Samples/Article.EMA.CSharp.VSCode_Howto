# Step By Step Guide EMA C# project and solution with VS Code

- version: 1.0.1
- Last Update: June 2024
- Environment: Ubuntu or Windows
- Compiler: .NET 6.0
- Prerequisite: [prerequisite](#prerequisite)

Example Code Disclaimer:
ALL EXAMPLE CODE IS PROVIDED ON AN “AS IS” AND “AS AVAILABLE” BASIS FOR ILLUSTRATIVE PURPOSES ONLY. REFINITIV MAKES NO REPRESENTATIONS OR WARRANTIES OF ANY KIND, EXPRESS OR IMPLIED, AS TO THE OPERATION OF THE EXAMPLE CODE, OR THE INFORMATION, CONTENT, OR MATERIALS USED IN CONNECTION WITH THE EXAMPLE CODE. YOU EXPRESSLY AGREE THAT YOUR USE OF THE EXAMPLE CODE IS AT YOUR SOLE RISK.

## <a id="intro"></a>Introduction

[Real-Time SDK (C# Edition)](https://developers.lseg.com/en/api-catalog/refinitiv-real-time-opnsrc/refinitiv-real-time-csharp-sdk) (RTSDK, formerly known as Elektron SDK) is a suite of modern and open source APIs ([GitHub](https://github.com/Refinitiv/Real-Time-SDK)) that aim to simplify development through a strong focus on ease of use and standardized access to LSEG Real-Time Platform via the proprietary TCP connection named RSSL and proprietary binary message encoding format named OMM Message. The capabilities range from low latency/high-performance APIs right through to simple streaming Web APIs.

The RTSDK C# Edition can run on Windows, Oracle Linux Server, Red Hat Enterprise Server and Ubuntu Linux platforms. It supports the [Visual Studio 2022 IDE](https://visualstudio.microsoft.com/vs/) for the full features development experience but the IDE is available for Windows developers only. Fortunately, the RTSDK C# Edition also supports the cross-platform [.NET SDK 6](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-6) (aka .NET Core 6) framework and the [Visual Studio Code](https://code.visualstudio.com/) (aka VS Code) editor is available for all major OS platforms. Linux and Windows developers who are using the VS Code editor can implement the real-time streaming application with LSEG Real-Time platform using the RTSDK C# Edition.

This example project shows a step-by-step guide to create the EMA API .NET project and solution with the RTSDK C# Edition on VS Code and the [C# extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp). I am demonstrating with the RTSDK C# version 2.1.3.L1 on Ubuntu Linux, and this step-by-step guide can be applied to any supported OS platforms.

## <a id="prerequisite"></a>Prerequisite

Before I am going further, there is some prerequisite, dependencies, and libraries that the project is needed.

### .NET SDK

Firstly, you need .NET 6 SDK. You can download the SDK based on your system from [Microsoft .NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) website.

Please check [How to check that .NET is already installed](https://learn.microsoft.com/en-us/dotnet/core/install/how-to-detect-installed-versions) to verify installed .NET versions on your machine.

### Visual Studio Code

Next, the [VS Code](https://code.visualstudio.com/) editor tool with the free [C# extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp).

**Note**:

1. There is also the [C# Dev Kit extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit) that gives developers more "Visual Studio like" experience and features than the C# extension. However, the C# Dev Kit extension requires Visual Studio License.
2. The C# extension requires [.NET 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) (**As of April 2024**), but you can setup and compile .NET 6.0 project with the .NET 8.0 SDK.

### Access to the RTO

This project uses RTO access credentials for Version 2 Authentication (Service ID).

Please contact your LSEG representative to help you with the RTO account and services.

### Internet Access

The RTSDK C# libraries (both ETA and EMA APIs) are available on the [NuGet](https://www.nuget.org/) package manager and distribution platform. You can use the built-in VS Code CLI command to download the EMA and ETA libraries from NuGet over internet.

That covers the prerequisite of this project.

## <a id="how_to_setup_proj_solution"></a>How to setup the EMA C# API Project and Solution

Please see the [Article document](./Article.md) for more detail.

## <a id="how_to_run"></a>How to run the demo applications

The first step is to unzip or download the example project folder into a directory of your choice, then set up Python or Postman environments based on your preference.

### <a id="project_example_run"></a>Run the demo project

1. Open a terminal and go to the project' ```ema_project``` folder.
2. Create a file name ```.env``` in the ```ema_project``` folder with the following content.

    ``` ini
    CLIENT_ID=<Your Auth V2 Client-ID>
    CLIENT_SECRET=<Your Auth V2 Client-Secret>
    ```

3. Run the following command inside the ```ema_project``` folder to build the project.

    ``` bash
    $>dotnet build
    ```

4. Run the following command inside the ```ema_project``` folder to run the project.

    ``` bash
    $>dotnet run
    ```

5. Run the following command inside the ```ema_project``` folder to publish the project.

    ``` bash
    $>dotnet publish --configuration Release --runtime <.NET runtime identifier> --self-contained
    ```

For more detail about the .NET runtime identifier, please check [this document](https://learn.microsoft.com/en-us/dotnet/core/rid-catalog).

### <a id="solution_example_run"></a>Run the demo solution

1. Open a terminal and go to the project' ```ema_solution``` folder.
2. Create a file name ```.env``` in the ```ema_solution\EMAConsumer``` folder with the following content.

    ``` ini
    CLIENT_ID=<Your Auth V2 Client-ID>
    CLIENT_SECRET=<Your Auth V2 Client-Secret>
    ```

3. Run the following command inside the ```ema_solution``` folder to build the solution.

    ``` bash
    $>dotnet build
    ```

4. Run the following command inside the ```ema_solution\EMAConsumer``` folder to run the EMAConsumer project.

    ``` bash
    $>dotnet run
    ```

5. Run the following command inside the ```ema_solution``` folder to publish the project.

    ``` bash
    $>dotnet publish --configuration Release --runtime <.NET runtime identifier> --self-contained
    ```

For more detail about the .NET runtime identifier, please check [this document](https://learn.microsoft.com/en-us/dotnet/core/rid-catalog).

## <a href="summary"></a>Conclusion and Next Steps

Before I finish, let me just say the RTSDK C# give developers access to the LSEG Real-Time platform's real-time streaming data with both low-level and high-levels APIs interfaces for every developers' requirements. For ultra-high performance applications, there is the ETA API that provides high performance, low latency, and open source low-level API interfaces for developers. For the majority of use cases, there is the ease-of-use EMA API with high-level API interfaces for developers. The C# edition SDK supports the cross-platform [.NET SDK 6](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-6) (aka .NET Core 6) which makes the application can be developed on various types of development environments such as the full-feature Visual Studio 2022 IDE on Windows, or the [.NET CLI tool](https://learn.microsoft.com/en-us/dotnet/core/tools/) with any editors on the supported platforms.

[Visual Studio Code](https://code.visualstudio.com/) (or just VSCode) is a free, cross-platform source code editor that took over developers' popularity based on its fast and lightweight, supports a variety of programming languages with IntelliSense, and has complete development operations like debugging, task running, and version control. With the the free [C# extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp), the VS Code can do basic development tasks with .NET development including the RTSDK C# APIs using the editor with the .NET CLI tool. The combination of the VS Code C# extension and .NET CLI tool make they suitable for developing the real-time applications on non-Windows platforms, or even on Windows for developers who do not have the Visual Studio Professional/Enterprise subscriptions.

If you want more powerful development feature on VS Code for the RTSDK C# edition, there is the [C# Dev kit extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit) that gives you the development experience much closer to the full feature Visual Studio IDE while maintain its lightweight and supports all major OS platforms. However, the C# Dev kit extension requires the Visual Studio Professional or Enterprise subscriptions license.

That’s all I have to say about the RTSDK C# development with VS Code.

## <a id="ref"></a>References

For further details, please check out the following resources:
- [Real-Time SDK C# page](https://developers.lseg.com/en/api-catalog/refinitiv-real-time-opnsrc/refinitiv-real-time-csharp-sdk) on the [LSEG Developer Community](https://developers.lseg.com/) website.
- [Real-Time SDK Family](https://developers.lseg.com/en/use-cases-catalog/refinitiv-real-time) page.
- [Real-Time SDK C# Quick Start](https://developers.lseg.com/en/api-catalog/refinitiv-real-time-opnsrc/refinitiv-real-time-csharp-sdk/quick-start).
- [Developer Article: 10 important things you need to know before you write an Enterprise Real Time application](https://developers.lseg.com/article/10-important-things-you-need-know-you-write-elektron-real-time-application).
- [Changes to Customer Access and Identity Management: Refinitiv Real-Time - Optimized](https://developers.lseg.com/en/article-catalog/article/changes-to-customer-access-and-identity-management--refinitiv-re).
- [EMA C# API Library on NuGet](https://www.nuget.org/packages/LSEG.Ema.Core ) platform.
- [.NET SDK](https://learn.microsoft.com/en-us/dotnet/core/sdk) page.
- [Tutorial: Create a .NET console application using Visual Studio Code](https://learn.microsoft.com/en-us/dotnet/core/tutorials/with-visual-studio-code?pivots=dotnet-6-0).
- [Tutorial: Create a .NET class library using Visual Studio Code](https://learn.microsoft.com/en-us/dotnet/core/tutorials/library-with-visual-studio-code?pivots=dotnet-6-0).
- [Setting up VS Code for .NET development](https://code.visualstudio.com/docs/languages/dotnet#_setting-up-vs-code-for-net-development) page.
- [Getting Started with C# in VS Code](https://code.visualstudio.com/docs/csharp/get-started) document.
- [How YOU can get started with .NET Core and C# in VS Code](https://softchris.github.io/pages/dotnet-core.html#update-our-library-code) blog post.
- [Tutorial: Containerize a .NET app](https://learn.microsoft.com/en-us/dotnet/core/docker/build-container?tabs=windows&pivots=dotnet-7-0).
- [.NET SDK on DockerHub](https://hub.docker.com/_/microsoft-dotnet-sdk/) platform.
- [.NET Runtime on DockerHub](https://hub.docker.com/_/microsoft-dotnet-runtime/) platform.

For any question related to this article or the RTSDK page, please use the Developer Community [Q&A Forum](https://community.developers.refinitiv.com/).
