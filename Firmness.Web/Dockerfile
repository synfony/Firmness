# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Firmness.sln", "."]
COPY ["Firmness.Web/Firmness.Web.csproj", "Firmness.Web/"]
COPY ["Firmness.Core/Firmness.Core.csproj", "Firmness.Core/"]
COPY ["Firmness.ViewModels/Firmness.ViewModels.csproj", "Firmness.ViewModels/"]
RUN dotnet restore "Firmness.Web/Firmness.Web.csproj"
COPY . .
WORKDIR "/src/Firmness.Web"
RUN dotnet build "Firmness.Web.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "Firmness.Web.csproj" -c Release -o /app/publish

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Firmness.Web.dll"]