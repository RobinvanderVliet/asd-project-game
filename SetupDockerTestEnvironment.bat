@echo off
echo This script is used to deploy a test environment on Docker
pause
docker ps
IF %ERRORLEVEL% NEQ 0 (
 echo Docker is not running or is not installed
 pause
 exit
)
IF NOT EXIST "ASD-Game" (
 echo The folder ASD-Game does not exist. Can not continue with the script.
 pause
 exit
)

FOR /F "tokens=* USEBACKQ" %%F IN (`docker container inspect -f '{{.State.Status}}' network-switch-dotnet-websocketsharp`) DO (
SET var=%%F
)
IF NOT %var%! == 'running'! (
IF NOT EXIST "../asd-project-networkhandler/.net prototypes/NetworkSwitch WebSocketsSharp/NetworkSwitch" (
 echo The folder ../asd-project-networkhandler/.net prototypes/NetworkSwitch WebSocketsSharp/NetworkSwitch does not exist. Can not continue with the script.
 pause
 exit
)
 docker rm -f network-switch-dotnet-websocketsharp
 docker rmi -f network-switch-dotnet-websocketsharp
 echo Publishing NetworkSwitch!
 cd ../asd-project-networkhandler/.net prototypes/NetworkSwitch WebSocketsSharp/NetworkSwitch
 dotnet publish -c Release 
 echo Building NetworkSwitch Docker Image!
 docker build -t network-switch-dotnet-websocketsharp .
 docker run -itd -e PORT=8088 -p 0.0.0.0:8088:8088 --name network-switch-dotnet-websocketsharp network-switch-dotnet-websocketsharp
) else (
echo Network switch is already running
)

echo Removing old containers and images!
docker rm -f asd-game-host asd-game-client
docker rmi -f asd-game

cd %~dp0
echo Publishing ASD-Game!
cd ./ASD-Game
dotnet publish -c Release
echo Building ASD-Game Docker Image!
docker build -t asd-game .

echo Running images in containers!
docker run -itd -e TEST=HOST --name asd-game-host asd-game
docker run -itd -e TEST=CLIENT --name asd-game-client asd-game

echo Attach to Host & Client!
start cmd.exe /k docker attach asd-game-host
start cmd.exe /k docker attach asd-game-client
