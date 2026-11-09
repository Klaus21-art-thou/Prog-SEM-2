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
# Quick Start Guide: Running RaceDayManagementAPI with Docker

Follow these simple steps to run the API container on your machine:

1. **Install & Start Docker**: Ensure [Docker](https://www.docker.com/) is installed and running on your system.
2. **Load or Pull the Image**: 
   * If you have a `.tar` file archive, run: `docker load -i raceday-api.tar`
   * Or if pulling from a registry, run: `docker pull your-username/raceday-api:latest`
3. **Run the Container**: Open your terminal and execute:
   ```bash
   docker run -d -p 8080:80 --name raceday-api-instance raceday-api:latest

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RaceDayManagementAPI.dll"]
