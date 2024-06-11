ARG DOTNET_VERSION=6.0
ARG VARIANT=alpine
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS builder
LABEL maintainer="Developer Relations"

# Create app directory
WORKDIR /App

# Copy project files
COPY ema_solution.sln ./
COPY EMAConsumer/EMAConsumer.csproj /App/EMAConsumer/EMAConsumer.csproj
COPY JSONUtil/JSONUtil.csproj /App/JSONUtil/JSONUtil.csproj
# Restore as distinct layers
RUN dotnet restore ema_solution.sln
# Copy everything else
COPY . ./
# Build and publish a release
RUN dotnet publish ema_solution.sln --configuration Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime:${DOTNET_VERSION}-${VARIANT} as release
# Create app directory
WORKDIR /App
COPY --from=builder /App/out .
COPY EMAConsumer/EmaConfig.xml .
ENTRYPOINT ["dotnet","EMAConsumer.dll"]