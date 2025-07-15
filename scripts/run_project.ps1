Write-Host "Starting OrderProcessor..." -ForegroundColor Green

# Project paths 
$BackendPath = "..\src\OrderProcessor.Web.API" 
$FrontendPath = "..\src\OrderProcessor.Web.App\web-app"

# Start .NET backend in background
Write-Host "Starting .NET backend..." -ForegroundColor White
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$BackendPath'; dotnet run"

# Wait for backend to start
Start-Sleep -Seconds 3

# Start Vue frontend in background  
Write-Host "Starting Vue frontend..." -ForegroundColor White
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$FrontendPath'; npm run serve"

# Wait for frontend to start
Start-Sleep -Seconds 5
Start-Process "https://localhost:8081"

Write-Host "Done! Both services are starting up..." -ForegroundColor Green
Write-Host "Backend: http://127.0.0.1:7092" -ForegroundColor White
Write-Host "Frontend: https://127.0.0.1:8081" -ForegroundColor White