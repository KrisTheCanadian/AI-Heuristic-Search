FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["eight-puzzle/eight-puzzle.csproj", "eight-puzzle/"]
RUN dotnet restore "eight-puzzle/eight-puzzle.csproj"
COPY . .
WORKDIR "/src/eight-puzzle"
RUN dotnet build "eight-puzzle.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "eight-puzzle.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "eight-puzzle.dll"]
