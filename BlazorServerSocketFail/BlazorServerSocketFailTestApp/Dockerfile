#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
RUN apt-get update && apt-get install -y lsof #lsof required for diagnostics
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BlazorServerSocketFailTestApp/BlazorServerSocketFailTestApp.csproj", "BlazorServerSocketFailTestApp/"]
RUN dotnet restore "BlazorServerSocketFailTestApp/BlazorServerSocketFailTestApp.csproj"
COPY . .
WORKDIR "/src/BlazorServerSocketFailTestApp"
RUN dotnet build "BlazorServerSocketFailTestApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlazorServerSocketFailTestApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlazorServerSocketFailTestApp.dll"]