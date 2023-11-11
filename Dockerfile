FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ConfigValidator.Cli/ConfigValidator.Cli.csproj", "ConfigValidator.Cli/"]
COPY ["ConfigValidator.Providers/ConfigValidator.Providers.Azure/ConfigValidator.Providers.Azure.csproj", "ConfigValidator.Providers/ConfigValidator.Providers.Azure/"]
COPY ["ConfigValidator/ConfigValidator.Contracts/ConfigValidator.Contracts.csproj", "ConfigValidator/ConfigValidator.Contracts/"]
COPY ["ModulR/ModulR.Extensions/ModulR.Extensions.FluentValidation/ModulR.Extensions.FluentValidation/ModulR.Extensions.FluentValidation.csproj", "ModulR/ModulR.Extensions/ModulR.Extensions.FluentValidation/ModulR.Extensions.FluentValidation/"]
COPY ["ModulR/ModulR.Validation.Abstraction/ModulR.Validation.Abstraction/ModulR.Validation.Abstraction.csproj", "ModulR/ModulR.Validation.Abstraction/ModulR.Validation.Abstraction/"]
COPY ["ModulR/ModulR.Validation/ModulR.Validation/ModulR.Validation.csproj", "ModulR/ModulR.Validation/ModulR.Validation/"]
COPY ["ModulR/ModulR.ValueWrapper/ModulR.ValueWrapper/ModulR.ValueWrapper.csproj", "ModulR/ModulR.ValueWrapper/ModulR.ValueWrapper/"]
COPY ["ConfigValidator/ConfigValidator.Console/ConfigValidator.Console.csproj", "ConfigValidator/ConfigValidator.Console/"]
COPY ["ConfigValidator/ConfigValidator.Presentation/ConfigValidator.Presentation.csproj", "ConfigValidator/ConfigValidator.Presentation/"]
COPY ["ConfigValidator/ConfigValidator.Yaml/ConfigValidator.Yaml.csproj", "ConfigValidator/ConfigValidator.Yaml/"]
COPY ["ConfigValidator/ConfigValidator.Fluent/ConfigValidator.Fluent.csproj", "ConfigValidator/ConfigValidator.Fluent/"]
RUN dotnet restore "ConfigValidator.Cli/ConfigValidator.Cli.csproj"
COPY . .
WORKDIR "/src/ConfigValidator.Cli"
RUN dotnet build "ConfigValidator.Cli.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConfigValidator.Cli.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConfigValidator.Cli.dll"]
