FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Numberlink-puzzle/Numberlink-puzzle.csproj", "Numberlink-puzzle/"]
RUN dotnet restore "Numberlink-puzzle/Numberlink-puzzle.csproj"
COPY . .
WORKDIR "/src/Numberlink-puzzle"
RUN dotnet build "Numberlink-puzzle.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Numberlink-puzzle.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Numberlink-puzzle.dll"]
