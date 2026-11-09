
# Use the official .NET 8 ASP.NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Use the .NET 8 SDK to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the MVC project
COPY ["RaceDayMVC.csproj", "./"]
RUN dotnet restore "RaceDayMVC.csproj"

# Build and publish
COPY . .
RUN dotnet publish "RaceDayMVC.csproj" -c Release -o /app/publish

# Final container
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Connect MVC to the API running on the host computer
ENV RaceDayApiBaseUrl=http://host.docker.internal:5225/

ENTRYPOINT ["dotnet", "RaceDayMVC.dll"]
