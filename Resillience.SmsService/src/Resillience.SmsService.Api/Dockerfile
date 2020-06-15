FROM registry.cn-hangzhou.aliyuncs.com/yoyosoft/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM registry.cn-hangzhou.aliyuncs.com/yoyosoft/dotnet/core/sdk AS build
WORKDIR /src

COPY ["Resillience.SmsService.Api.csproj", "Resillience.SmsService.Api/"]
#COPY ["Resillience.SmsService.AliSms.SDK.csproj", "Resillience.SmsService.Api/"]
#COPY ["Resillience.SmsService.Abstractions.csproj", "Resillience.SmsService.Api/"]
#COPY ["Resillience.SmsService.TencentSms.SDK.csproj", "Resillience.SmsService.Api/"]
#
RUN dotnet restore "Resillience.SmsService.Api/Resillience.SmsService.Api.csproj"
COPY . /src/Resillience.SmsService.Api
WORKDIR /src/Resillience.SmsService.Api
RUN dotnet build "Resillience.SmsService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Resillience.SmsService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Resillience.SmsService.Api.dll"]