# Step By Step Guide EMA C# project with VS Code 

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