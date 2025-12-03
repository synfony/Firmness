# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Firmness.sln", "."]
COPY ["FIrmnessAPI/FIrmnessAPI.csproj", "FIrmnessAPI/"]
COPY ["Firmness.Core/Firmness.Core.csproj", "Firmness.Core/"]
COPY ["Firmness.ViewModels/Firmness.ViewModels.csproj", "Firmness.ViewModels/"]
RUN dotnet restore "FIrmnessAPI/FIrmnessAPI.csproj"
COPY . .
WORKDIR "/src/FIrmnessAPI"
RUN dotnet build "FIrmnessAPI.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "FIrmnessAPI.csproj" -c Release -o /app/publish

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FIrmnessAPI.dll"]