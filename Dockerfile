FROM mcr.microsoft.com/dotnet/sdk:6.0 AS development
WORKDIR /app
EXPOSE 80
# Copy everything
COPY . .
# Restore as distinct layers
RUN dotnet restore .
# Build and publish a release
RUN dotnet publish "./src/Edwards.CodeChallenge.API/Edwards.CodeChallenge.API.csproj" -c Release -o out
COPY ./src/Edwards.CodeChallenge.API/data/data.sqlite /app/data.sqlite

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine as runtime
WORKDIR /app
COPY --from=development /app/out .
# Set environment variable(s)

ENV DB_CONNECTION_STRING__USERDB=Data Source=/data/data.sqlite
ENV FileServiceOptions__FilePath=/data/dump_users.json
ENTRYPOINT ["dotnet", "Edwards.CodeChallenge.API.dll"]
