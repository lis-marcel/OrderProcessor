# Start debugging in existing Visual Studio instance
Write-Host "Starting debug session in Visual Studio..." -ForegroundColor White

# Bring Visual Studio to foreground and send F5
$vsProcess = Get-Process -Name "devenv" -ErrorAction SilentlyContinue | Select-Object -First 1
if ($vsProcess) {
    Add-Type -AssemblyName Microsoft.VisualBasic
    [Microsoft.VisualBasic.Interaction]::AppActivate($vsProcess.Id)
    Start-Sleep -Milliseconds 300
}

Add-Type -AssemblyName System.Windows.Forms
[System.Windows.Forms.SendKeys]::SendWait("{F5}")