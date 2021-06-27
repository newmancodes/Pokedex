# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

# Copy csprojs and restore as distinct layers
COPY *.sln ./
COPY ./src/Pokedex.Api/Pokedex.Api.csproj ./src/Pokedex.Api/Pokedex.Api.csproj
COPY ./src/Pokedex.Application/Pokedex.Application.csproj ./src/Pokedex.Application/Pokedex.Application.csproj
COPY ./src/Pokedex.Domain/Pokedex.Domain.csproj ./src/Pokedex.Domain/Pokedex.Domain.csproj
COPY ./src/Pokedex.Infrastructure.FunTranslation/Pokedex.Infrastructure.FunTranslation.csproj ./src/Pokedex.Infrastructure.FunTranslation/Pokedex.Infrastructure.FunTranslation.csproj
COPY ./src/Pokedex.Infrastructure.PokeApi/Pokedex.Infrastructure.PokeApi.csproj ./src/Pokedex.Infrastructure.PokeApi/Pokedex.Infrastructure.PokeApi.csproj
COPY ./test/integration/Pokedex.Api.Tests/Pokedex.Api.Tests.csproj ./test/integration/Pokedex.Api.Tests/Pokedex.Api.Tests.csproj
COPY ./test/unit/Pokedex.Application.Tests/Pokedex.Application.Tests.csproj ./test/unit/Pokedex.Application.Tests/Pokedex.Application.Tests.csproj
COPY ./test/unit/Pokedex.Domain.Tests/Pokedex.Domain.Tests.csproj ./test/unit/Pokedex.Domain.Tests/Pokedex.Domain.Tests.csproj
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
EXPOSE 80
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Pokedex.Api.dll"]
