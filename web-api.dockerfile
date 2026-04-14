#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app

EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY /src .
COPY Directory.Build.props .
COPY Directory.Packages.props .

RUN dotnet restore "Musicality/Musicality.csproj"
COPY /src .

RUN dotnet build "Musicality/Musicality.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Musicality/Musicality.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Eclipse.WebAPI.dll"]
