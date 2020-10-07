#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-bionic AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-bionic AS build
WORKDIR /src
COPY ["xlog_client_management_api/xlog_client_management_api.csproj", "xlog_client_management_api/"]
RUN dotnet restore "xlog_client_management_api/xlog_client_management_api.csproj"
COPY . .
WORKDIR "/src/xlog_client_management_api"
RUN dotnet build "xlog_client_management_api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "xlog_client_management_api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "xlog_client_management_api.dll"]