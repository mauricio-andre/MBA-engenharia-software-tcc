# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copia tudo e restaura dependências
COPY . .
COPY ../Commons Commons
RUN dotnet restore

# Publica o projeto definido via build-arg
ARG PROJECT
RUN dotnet publish ${PROJECT} -c Release -o /out

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /out .

# Esse valor será substituído via compose
ENTRYPOINT ["dotnet", "App.dll"]