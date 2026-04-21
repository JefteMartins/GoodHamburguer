FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["global.json", "./"]
COPY ["src/GoodHamburguer.Blazor/GoodHamburguer.Blazor.csproj", "src/GoodHamburguer.Blazor/"]

RUN dotnet restore "src/GoodHamburguer.Blazor/GoodHamburguer.Blazor.csproj"

COPY . .
WORKDIR /src/src/GoodHamburguer.Blazor
RUN dotnet publish "GoodHamburguer.Blazor.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "GoodHamburguer.Blazor.dll"]
