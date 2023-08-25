FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Dotnet-RateLimiter/Dotnet-RateLimiter.csproj", "Dotnet-RateLimiter/"]
RUN dotnet restore "Dotnet-RateLimiter/Dotnet-RateLimiter.csproj"
COPY . .
WORKDIR "/src/Dotnet-RateLimiter"
RUN dotnet build "Dotnet-RateLimiter.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dotnet-RateLimiter.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dotnet-RateLimiter.dll"]
