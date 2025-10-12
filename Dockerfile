# Dockerfile para Railway (na raiz do projeto)
# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar projeto da subpasta
COPY backend/PalavraConectada.API/PalavraConectada.API.csproj ./
RUN dotnet restore "PalavraConectada.API.csproj"

# Copiar todo o código
COPY backend/PalavraConectada.API/. ./
RUN dotnet build "PalavraConectada.API.csproj" -c Release -o /app/build

# Publish Stage
FROM build AS publish
RUN dotnet publish "PalavraConectada.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080

# Copiar arquivos publicados
COPY --from=publish /app/publish .

# Copiar banco de dados
COPY backend/PalavraConectada.API/bible.db ./bible.db

ENTRYPOINT ["dotnet", "PalavraConectada.API.dll"]

