FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
ENV TZ=Asia/Shanghai
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src

COPY ["demo/Hangfire.HttpJob.Demo/Hangfire.HttpJob.Demo.csproj", "demo/Hangfire.HttpJob.Demo/"]
RUN dotnet restore "demo/Hangfire.HttpJob.Demo/Hangfire.HttpJob.Demo.csproj"
COPY . .
WORKDIR "/src/demo/Hangfire.HttpJob.Demo"
RUN dotnet build "Hangfire.HttpJob.Demo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Hangfire.HttpJob.Demo.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Hangfire.HttpJob.Demo.dll"]





