FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["global.json", "./"]
COPY ["src/GoodHamburguer.Api/GoodHamburguer.Api.csproj", "src/GoodHamburguer.Api/"]
COPY ["src/GoodHamburguer.Application/GoodHamburguer.Application.csproj", "src/GoodHamburguer.Application/"]
COPY ["src/GoodHamburguer.Domain/GoodHamburguer.Domain.csproj", "src/GoodHamburguer.Domain/"]
COPY ["src/GoodHamburguer.Infrastructure/GoodHamburguer.Infrastructure.csproj", "src/GoodHamburguer.Infrastructure/"]

RUN dotnet restore "src/GoodHamburguer.Api/GoodHamburguer.Api.csproj"

COPY . .
WORKDIR /src/src/GoodHamburguer.Api
RUN dotnet publish "GoodHamburguer.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "GoodHamburguer.Api.dll"]
