@echo off
echo Starting Conveyor Configurator...
echo.

REM Start backend in new window
echo [1/2] Starting Backend API (Port 5000)...
start "Backend API" cmd /k "cd backend && dotnet run --urls=http://localhost:5000"

REM Wait a moment for backend to initialize
timeout /t 3 /nobreak >nul

REM Start frontend in new window
echo [2/2] Starting Frontend (Port 5050)...
start "Frontend" cmd /k "cd frontend && dotnet run --urls=http://localhost:5050"

echo.
echo ========================================
echo Backend:  http://localhost:5000/api/health
echo Frontend: http://localhost:5050
echo ========================================
echo.
echo Both services are starting in separate windows...
echo Wait for "Now listening on" messages, then open:
echo    http://localhost:5050
echo.
pause
