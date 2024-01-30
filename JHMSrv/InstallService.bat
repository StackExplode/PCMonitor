@echo off


:menu
cls
echo Install or delete Jennings PCMonitor service?
SC QUERY JHMSrv > NUL
IF ERRORLEVEL 1060 echo Service is not installed.
if not  ERRORLEVEL 1060 ECHO Service is currently installed.


echo 0 = Exit
echo 1 = Install service
echo 2 = Install and set auto start service
echo 3 = Uninstall service
set /p user_input=Input number:

if %user_input% == 0 (
	exit
) else if %user_input% == 1 (
	sc create "JHMSrv" binpath=%~dp0\JHMSrv.exe DisplayName="Jennings PCMonitor2"
	sc description "JHMSrv" "This is a PCMonitor service version 2 made by Jennings."
	sc start "JHMSrv"
) else if %user_input% == 2 (
	sc create "JHMSrv" binpath=%~dp0\JHMSrv.exe DisplayName="Jennings PCMonitor2" start=auto
	sc description "JHMSrv" "This is a PCMonitor service version 2 made by Jennings."
	sc start "JHMSrv"
	
) else if %user_input% == 3 (
	sc delete "JHMSrv"
	
) else (
  goto menu
) 

echo Operation success!
pause

@echo on