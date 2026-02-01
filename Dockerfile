FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 7236
EXPOSE 8079

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files for restore (assuming they're all in the root level)
COPY ["drf.API/dreamify.API.csproj", "drf.API/"]
COPY ["drf.Application/dreamify.Application.csproj", "drf.Application/"]
COPY ["drf.Domain/dreamify.Domain.csproj", "drf.Domain/"]
COPY ["drf.Infrastructure/dreamify.Infrastructure.csproj", "drf.Infrastructure/"]

# Restore dependencies starting from the API project
RUN dotnet restore "drf.API/drf.API.csproj"

# Copy all source code
COPY . .

# Build the API project
WORKDIR "/src/drf.API"
RUN dotnet build "drf.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "drf.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "drf.API.dll"]