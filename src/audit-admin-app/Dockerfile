#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["audit-admin-app/audit-admin-app.csproj", "audit-admin-app/"]
RUN dotnet restore "audit-admin-app/audit-admin-app.csproj"
COPY . .
WORKDIR "/src/audit-admin-app"
RUN dotnet build "audit-admin-app.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "audit-admin-app.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "audit-admin-app.dll"]