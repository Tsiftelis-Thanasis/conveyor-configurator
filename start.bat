@echo off
echo ========================================
echo   Conveyor Configurator
echo ========================================
echo.

:: Start .NET backend (serves both API and static files)
echo Starting .NET Server on port 5000...
cd /d "%~dp0backend"
start "Conveyor Configurator" cmd /c "dotnet run --urls=http://localhost:5000"

:: Wait for server to start
timeout /t 3 /nobreak >nul

echo.
echo ========================================
echo   Server Started!
echo ========================================
echo.
echo   Application: http://localhost:5000
echo   API Health:  http://localhost:5000/api/health
echo.
echo   Press any key to open the browser...
pause >nul

start http://localhost:5000
