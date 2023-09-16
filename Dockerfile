FROM mcr.microsoft.com/dotnet/sdk:6.0 AS development
WORKDIR /app

# Copy everything
COPY . .
# Restore as distinct layers
RUN dotnet restore .
# Build and publish a release
RUN dotnet publish "./src/Edwards.CodeChallenge.API/Edwards.CodeChallenge.API.csproj" -c Release -o out
 

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine as runtime
WORKDIR /app
COPY --from=development /app/out .
ENTRYPOINT ["dotnet", "Edwards.CodeChallenge.API.dll"]
