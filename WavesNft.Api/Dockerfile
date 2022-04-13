#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WavesNft.Api/WavesNft.Api.csproj", "WavesNft.Api/"]
COPY ["Waves.standard/Waves.standard.csproj", "Waves.standard/"]
RUN dotnet restore "WavesNft.Api/WavesNft.Api.csproj"
COPY . .
WORKDIR "/src/WavesNft.Api"
RUN dotnet build "WavesNft.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WavesNft.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WavesNft.Api.dll"]