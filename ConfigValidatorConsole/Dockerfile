FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ConfigValidatorConsole/ConfigValidatorConsole.csproj", "ConfigValidatorConsole/"]
RUN dotnet restore "ConfigValidatorConsole/ConfigValidatorConsole.csproj"
COPY . .
WORKDIR "/src/ConfigValidatorConsole"
RUN dotnet build "ConfigValidatorConsole.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConfigValidatorConsole.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConfigValidatorConsole.dll"]
