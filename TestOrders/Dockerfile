#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TestOrders/TestOrders.csproj", "TestOrders/"]
COPY ["Orders.Core/Orders.Core.csproj", "Orders.Core/"]
COPY ["Orders.Infrastructure/Orders.Infrastructure.csproj", "Orders.Infrastructure/"]
RUN dotnet restore "TestOrders/TestOrders.csproj"
COPY . .
WORKDIR "/src/TestOrders"
RUN dotnet build "TestOrders.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestOrders.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestOrders.dll"]