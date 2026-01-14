# ---------- BUILD STAGE ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy entire repository
COPY . .

# Restore dependencies
RUN dotnet restore Ecommerce/Ecommerce.csproj

# Publish the application
RUN dotnet publish Ecommerce/Ecommerce.csproj \
    -c Release \
    -o /app/publish

# ---------- RUNTIME STAGE ----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copy published output
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}
EXPOSE 8080

# Start the app
ENTRYPOINT ["dotnet", "Ecommerce.dll"]
