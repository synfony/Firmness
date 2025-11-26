# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copy solution and project files to restore dependencies
COPY *.sln .
COPY Firmness.Web/*.csproj ./Firmness.Web/
COPY Firmness.Tests/*.csproj ./Firmness.Tests/
RUN dotnet restore

# Copy the rest of the source code
COPY . .

# Run tests
WORKDIR /source/Firmness.Tests
RUN dotnet test

# Publish the application
WORKDIR /source/Firmness.Web
RUN dotnet publish -c release -o /app --no-restore

# Stage 2: Create the final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./

# Expose the port the app will run on
EXPOSE 8080

# Set the entrypoint for the container
ENTRYPOINT ["dotnet", "Firmness.Web.dll"]
