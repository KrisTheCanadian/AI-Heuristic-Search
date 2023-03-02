FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Heuristics/Heuristics.csproj", "Heuristics/"]
RUN dotnet restore "Heuristics/Heuristics.csproj"
COPY . .
WORKDIR "/src/Heuristics"
RUN dotnet build "Heuristics.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Heuristics.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Heuristics.dll"]
