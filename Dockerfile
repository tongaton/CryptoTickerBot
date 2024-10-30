#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-bionic AS base
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf
WORKDIR /src
COPY ["CryptoTickerBot.API/CryptoTickerBot.API.csproj", "CryptoTickerBot.API/"]
COPY ["CryptoTickerBot.Data/CryptoTickerBot.Data.csproj", "CryptoTickerBot.Data/"]
COPY ["NLog.Targets.Sentry/NLog.Targets.Sentry.csproj", "NLog.Targets.Sentry/"]
RUN dotnet restore "CryptoTickerBot.API/CryptoTickerBot.API.csproj"
COPY . .
WORKDIR "/src/CryptoTickerBot.API"
RUN dotnet build "CryptoTickerBot.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CryptoTickerBot.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN apt-get update -y
RUN apt-get install -y tzdata
ENV TZ America/Argentina/Buenos_Aires
ENTRYPOINT ["dotnet", "CryptoTickerBot.API.dll"]


