cd SimpleBackendServer/
start cmd /k "dotnet run --urls https://localhost:9001"
sleep(1)
start cmd /k "dotnet run --urls https://localhost:9002"
sleep(1)
start cmd /k "dotnet run --urls https://localhost:9003"