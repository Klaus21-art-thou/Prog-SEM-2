# See https://aka.ms/customizecontainer to learn how to customize your debug container
# Stage 1: Base runtime environment for .NET 8
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Stage 2: Build environment using the .NET 8 SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file specified in the solution
COPY ["RaceDayManagementAPI.csproj", "./"]

# Restore external NuGet packages
RUN dotnet restore "RaceDayManagementAPI.csproj"

# Copy the rest of the source code files
COPY . .
WORKDIR "/src"

# Build the application in Release mode
RUN dotnet build "RaceDayManagementAPI.csproj" -c Release -o /app/build

# Stage 3: Publish the application
FROM build AS publish
RUN dotnet publish "RaceDayManagementAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 4: Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RaceDayManagementAPI.dll"]