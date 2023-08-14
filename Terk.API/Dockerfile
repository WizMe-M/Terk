FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

ADD ["Terk.DB/Terk.DB.csproj", "Terk.DB/"]
ADD ["Terk.API.Contracts/Terk.API.Contracts.csproj", "Terk.API.Contracts/"]
ADD ["Terk.API/Terk.API.csproj", "Terk.API/"]
RUN dotnet restore "Terk.API/Terk.API.csproj"

COPY . .
WORKDIR "/src/Terk.API"
RUN dotnet build "Terk.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Terk.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Terk.API.dll"]
