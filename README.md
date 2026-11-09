# Prog-SEM-2
## YouTube Link
https://youtu.be/N3avQt_TDzE
## YouTube Link part 3
https://youtu.be/xM1GMvuvjWo

## CDI/WorkFlow
<img width="628" height="102" alt="image" src="https://github.com/user-attachments/assets/d8f1394f-1d40-4961-b496-583b9c233f60" />

## ERD and API
<img width="1087" height="586" alt="image" src="https://github.com/user-attachments/assets/2db389ca-167c-4ede-87c9-2307124d9e36" />
<img width="835" height="571" alt="image" src="https://github.com/user-attachments/assets/6bfa9602-af0f-4872-b96f-711790592afc" />

## 🐳 Docker Setup & Usage

Follow the steps below to containerize and run the **RaceDayManagementAPI** using Docker.

### Prerequisites
* Ensure [Docker](https://www.docker.com/) is installed and running on your local machine.

---

### 1. Add the Dockerfile
Create a file named **`Dockerfile`** (with **no** file extension) in the root directory of your `RaceDayManagementAPI` project folder (alongside your `.csproj` file) and paste the following configuration:

```dockerfile
# Base runtime environment for .NET 8
FROM [mcr.microsoft.com/dotnet/aspnet:8.0](https://mcr.microsoft.com/dotnet/aspnet:8.0) AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build environment using the .NET 8 SDK
FROM [mcr.microsoft.com/dotnet/sdk:8.0](https://mcr.microsoft.com/dotnet/sdk:8.0) AS build
WORKDIR /src
COPY ["RaceDayManagementAPI.csproj", "./"]
RUN dotnet restore "RaceDayManagementAPI.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "RaceDayManagementAPI.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "RaceDayManagementAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RaceDayManagementAPI.dll"]
