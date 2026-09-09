@echo off
curl.exe --fail --show-error --silent "http://localhost:5062/api/MSFormSync"
exit /b %ERRORLEVEL%
