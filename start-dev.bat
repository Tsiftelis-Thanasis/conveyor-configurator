@echo off
echo Starting Conveyor Configurator Development Environment
echo ========================================================
echo.
echo Starting Backend API on http://localhost:5000
start "Backend API" cmd /c "cd backend && dotnet run --urls=http://localhost:5000"
echo.
echo Starting Blazor Frontend on http://localhost:5050
start "Blazor Frontend" cmd /c "cd frontend && dotnet run --urls=http://localhost:5050"
echo.
echo ========================================================
echo Backend API: http://localhost:5000/api/health
echo Frontend App: http://localhost:5050
echo ========================================================
echo.
pause
