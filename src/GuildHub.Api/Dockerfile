FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5278

ENV ASPNETCORE_URLS=http://+:5000

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["src/GuildHub.Api/GuildHub.Api.csproj", "src/GuildHub.Api/"]
RUN dotnet restore "src/GuildHub.Api/GuildHub.Api.csproj"
COPY . .
WORKDIR "/src/src/GuildHub.Api"
RUN dotnet build "GuildHub.Api.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "GuildHub.Api.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GuildHub.Api.dll", "--launch-profile", "http"]
