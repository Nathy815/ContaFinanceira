FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ContaFinanceira.API/ContaFinanceira.API.csproj", "ContaFinanceira.API/"]
RUN dotnet restore "ContaFinanceira.API/ContaFinanceira.API.csproj"
COPY . .
WORKDIR "/src/ContaFinanceira.API"
RUN dotnet build "ContaFinanceira.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ContaFinanceira.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ContaFinanceira.API.dll"]