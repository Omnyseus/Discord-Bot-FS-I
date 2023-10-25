FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Discord-Bot-FS-I/Discord-Bot-FS-I.csproj", "Discord-Bot-FS-I/"]
RUN dotnet restore "Discord-Bot-FS-I/Discord-Bot-FS-I.csproj"
COPY . .
WORKDIR "/src/Discord-Bot-FS-I"
RUN dotnet build "Discord-Bot-FS-I.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Discord-Bot-FS-I.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Discord-Bot-FS-I.dll"]
