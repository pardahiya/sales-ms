FROM mcr.microsoft.com/dotnet/sdk:7.0
ARG BUILD_CONFIGURATION=Debug
ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_USE_POLLING_FILE_WATCHER=true
EXPOSE 80

WORKDIR /src

COPY ["Sales.EventBus/Sales.EventBus.csproj", "Sales.EventBus/"]
COPY ["Sales.EventBus.RabbitMQ/Sales.EventBus.RabbitMQ.csproj", "Sales.EventBus.RabbitMQ/"]
COPY ["IntegrationEventLogEF/Sales.IntegrationEventLogEF.csproj", "IntegrationEventLogEF/"]
COPY ["Sales.API/Sales.API.csproj", "Sales.API/"]
COPY ["NuGet.config", "NuGet.config"]

RUN dotnet restore Sales.API/Sales.API.csproj -nowarn:msb3202,nu1503
COPY . .
WORKDIR "/src/Sales.API"
RUN dotnet build -c Debug


ENTRYPOINT ["dotnet", "run", "--no-build", "--no-launch-profile", "-c", "Debug", "--"]