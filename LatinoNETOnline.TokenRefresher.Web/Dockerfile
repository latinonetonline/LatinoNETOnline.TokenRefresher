#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["LatinoNETOnline.TokenRefresher.Web/LatinoNETOnline.TokenRefresher.Web.csproj", "LatinoNETOnline.TokenRefresher.Web/"]
RUN dotnet restore "LatinoNETOnline.TokenRefresher.Web/LatinoNETOnline.TokenRefresher.Web.csproj"
COPY . .
WORKDIR "/src/LatinoNETOnline.TokenRefresher.Web"
RUN dotnet build "LatinoNETOnline.TokenRefresher.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LatinoNETOnline.TokenRefresher.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD  ASPNETCORE_URLS=http://*:$PORT dotnet LatinoNETOnline.TokenRefresher.Web.dll