# Usar imagem oficial do .NET SDK para build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copiar arquivos do projeto e restaurar dependências
COPY *.sln ./
COPY backend/*.csproj ./backend/
RUN dotnet restore

# Copiar todo o restante do código
COPY . ./
RUN dotnet publish -c Release -o out

# Imagem final para rodar a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 5000
ENTRYPOINT ["dotnet", "backend.dll"]
