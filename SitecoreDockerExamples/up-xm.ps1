$ErrorActionPreference = "Stop";

# Start the Sitecore instance
Write-Host "Starting Sitecore environment..." -ForegroundColor Green
docker compose -f docker-compose.xm1.yml -f docker-compose.xm1.override.yml up -d