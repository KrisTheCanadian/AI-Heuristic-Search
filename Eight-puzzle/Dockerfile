FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Eight-puzzle/Eight-puzzle.csproj", "Eight-puzzle/"]
RUN dotnet restore "Eight-puzzle/Eight-puzzle.csproj"
COPY . .
WORKDIR "/src/Numberlink-puzzle"
RUN dotnet build "Eight-puzzle.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Eight-puzzle.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Eight-puzzle.dll"]
