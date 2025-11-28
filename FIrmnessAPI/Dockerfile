# --- Build Stage ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# 1. Copy all project files and the solution file
COPY *.sln .
COPY FIrmnessAPI/*.csproj ./FIrmnessAPI/
COPY Firmness.Web/*.csproj ./Firmness.Web/
COPY Firmness.Tests/*.csproj ./Firmness.Tests/

# 2. Restore dependencies for the entire solution
RUN dotnet restore

# 3. Copy the rest of the source code
COPY . .

# 4. Publish only the specific project for this Dockerfile
WORKDIR /source/FIrmnessAPI
RUN dotnet publish "FIrmnessAPI.csproj" -c Release -o /app/publish --no-restore

# --- Final Stage ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8081
ENTRYPOINT ["dotnet", "FIrmnessAPI.dll"]
