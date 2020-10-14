FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS dotnetbuild
WORKDIR /src
COPY ["src/TDLib.Api/TDLib.Api.csproj", "TDLib.Api/TDLib.Api.csproj"]
RUN dotnet restore "TDLib.Api/TDLib.Api.csproj"
COPY ["src/TDLib/TDLib.csproj", "TDLib/TDLib.csproj"]
RUN dotnet restore "TDLib/TDLib.csproj"
COPY ["src/Telegram.Governor/Telegram.Governor.csproj", "Telegram.Governor/Telegram.Governor.csproj"]
RUN dotnet restore "Telegram.Governor/Telegram.Governor.csproj"
COPY ["src/audit-admin-app/audit-admin-app.csproj", "audit-admin-app/audit-admin-app.csproj"]
RUN dotnet restore "audit-admin-app/audit-admin-app.csproj"
COPY ./src/. ./
RUN dotnet build "audit-admin-app/audit-admin-app.csproj" -c Release -o /app/build

FROM dotnetbuild AS publish
RUN dotnet publish "audit-admin-app/audit-admin-app.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS final
WORKDIR /app
EXPOSE 80
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "audit-admin-app.dll"]
