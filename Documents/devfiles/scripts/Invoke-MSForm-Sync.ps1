# =========================================================
# Script Name: ResendEmail.ps1
# Description: Calls the request-resend API endpoint
# Author: R. Laude
# Date: (update as needed)
# =========================================================

# URL of your API endpoint
$apiUrl = "http://localhost:5062/api/MSFormSync"

# Log file path (optional)
$logFile = "C:\GitLab\joboffer\Documents\devfiles\scripts\msformsynclogs.txt"

# Create log directory if it doesn’t exist
if (!(Test-Path -Path (Split-Path $logFile))) {
    New-Item -Path (Split-Path $logFile) -ItemType Directory -Force | Out-Null
}

# Write start log
$timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
Add-Content -Path $logFile -Value "$timestamp - Starting API call to $apiUrl"

try {
    # Call the API (GET request)
    $response = Invoke-WebRequest -Uri $apiUrl -UseBasicParsing -TimeoutSec 60

    # Log response
    $statusCode = $response.StatusCode
    Add-Content -Path $logFile -Value "$timestamp - Success: HTTP $statusCode"
}
catch {
    # Log error
    $errorMsg = $_.Exception.Message
    Add-Content -Path $logFile -Value "$timestamp - Error: $errorMsg"
}

# Write completion log
Add-Content -Path $logFile -Value "$timestamp - Script finished"
