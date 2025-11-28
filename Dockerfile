# --- Build Stage ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# 1. Copy all project files and the solution file
COPY *.sln .
COPY Firmness.Web/*.csproj ./Firmness.Web/
COPY Firmness.Tests/*.csproj ./Firmness.Tests/
COPY FIrmnessAPI/*.csproj ./FIrmnessAPI/

# 2. Restore dependencies for the entire solution
RUN dotnet restore

# 3. Copy the rest of the source code
COPY . .

# 4. Run tests (optional, but good practice)
WORKDIR /source/Firmness.Tests
RUN dotnet test

# 5. Publish only the specific project for this Dockerfile
WORKDIR /source/Firmness.Web
RUN dotnet publish "Firmness.Web.csproj" -c Release -o /app/publish --no-restore

# --- Final Stage ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "Firmness.Web.dll"]
