FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
ARG PORT=5200
EXPOSE ${PORT}
ENV ASPNETCORE_URLS=http://+:${PORT} 

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AppTalk.API/AppTalk.API.csproj", "AppTalk.API/"]
RUN dotnet restore "AppTalk.API/AppTalk.API.csproj"
COPY . .
WORKDIR "/src/AppTalk.API"
RUN dotnet build "AppTalk.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AppTalk.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AppTalk.API.dll"]
