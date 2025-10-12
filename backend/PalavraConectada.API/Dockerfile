# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar arquivo do projeto e restaurar dependências
COPY ["PalavraConectada.API.csproj", "./"]
RUN dotnet restore "PalavraConectada.API.csproj"

# Copiar todo o código e fazer o build
COPY . .
RUN dotnet build "PalavraConectada.API.csproj" -c Release -o /app/build

# Publish Stage
FROM build AS publish
RUN dotnet publish "PalavraConectada.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080

# Copiar os arquivos publicados
COPY --from=publish /app/publish .

# Copiar o banco de dados SQLite (se existir)
COPY --from=build /src/bible.db ./bible.db 2>/dev/null || :

ENTRYPOINT ["dotnet", "PalavraConectada.API.dll"]

