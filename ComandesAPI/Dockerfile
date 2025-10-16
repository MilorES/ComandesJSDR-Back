# Dockerfile multi-stage per compilar i publicar l'API
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiem csproj i restaurem paquets
COPY ["ComandesAPI/ComandesAPI.csproj", "ComandesAPI/"]
RUN dotnet restore "ComandesAPI/ComandesAPI.csproj"

# Copiem i publiquem tot
COPY . .
WORKDIR /src/ComandesAPI
RUN dotnet publish "ComandesAPI.csproj" -c Release -o /app/publish /p:PublishTrimmed=false

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./
COPY --from=build /src/ComandesAPI/entrypoint.sh ./
RUN chmod +x ./entrypoint.sh

EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["/bin/bash", "./entrypoint.sh"]
