FROM registry.cn-hangzhou.aliyuncs.com/yoyosoft/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM registry.cn-hangzhou.aliyuncs.com/yoyosoft/dotnet/core/sdk AS build
WORKDIR /src

COPY ["NuGet.Config", "src/NuGet.Config"]
COPY ["src/Resillience.SmsService.Api/Resillience.SmsService.Api.csproj", "src/Resillience.SmsService.Api/"]
COPY ["src/Resillience.SmsService.Abstractions/Resillience.SmsService.Abstractions.csproj", "src/Resillience.SmsService.Abstractions/"]
COPY ["src/Resillience.SmsService.AliSms.SDK/Resillience.SmsService.AliSms.SDK.csproj", "src/Resillience.SmsService.AliSms.SDK/"]
COPY ["src/Resillience.SmsService.TencentSms.SDK/Resillience.SmsService.TencentSms.SDK.csproj", "src/Resillience.SmsService.TencentSms.SDK/"]
RUN dotnet restore  --configfile "src/NuGet.Config" "src/Resillience.SmsService.Api/Resillience.SmsService.Api.csproj"

COPY . .
WORKDIR "/src/src/Resillience.SmsService.Api"
RUN dotnet build "Resillience.SmsService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Resillience.SmsService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Resillience.SmsService.Api.dll"]