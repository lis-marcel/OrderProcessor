# Start debugging in existing Visual Studio instance
Write-Host "Starting debug session in Visual Studio..." -ForegroundColor Green

# Bring Visual Studio to foreground and send F5
$vsProcess = Get-Process -Name "devenv" -ErrorAction SilentlyContinue | Select-Object -First 1
if ($vsProcess) {
    Add-Type -AssemblyName Microsoft.VisualBasic
    [Microsoft.VisualBasic.Interaction]::AppActivate($vsProcess.Id)
    Start-Sleep -Milliseconds 300
}

Add-Type -AssemblyName System.Windows.Forms
[System.Windows.Forms.SendKeys]::SendWait("{F5}")

# Start Vue frontend
Write-Host "Starting Vue frontend..." -ForegroundColor Green
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '..\src\OrderProcessor.Web.App\web-app'; npm run serve"

Write-Host "Ready!" -ForegroundColor Green
Write-Host "Backend: Debugging started in Visual Studio -> https://127.0.0.1:7092" -ForegroundColor White
Write-Host "Frontend: https://127.0.0.1:8081" -ForegroundColor White