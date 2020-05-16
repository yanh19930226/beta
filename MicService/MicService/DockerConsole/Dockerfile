FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["DockerConsole.csproj", "DockerConsole/"]
RUN dotnet restore "DockerConsole/DockerConsole.csproj"
COPY . /src/DockerTest
WORKDIR /src/DockerTest
RUN dotnet build "DockerConsole.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DockerConsole.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DockerConsole.dll"]
