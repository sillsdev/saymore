pushd .\
FOR /F "tokens=*" %%i IN ('"C:\Program Files (x86)\Microsoft Visual Studio\Installer\vswhere.exe" -latest -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe') DO SET msbuildexe="%%i"
dotnet restore ..\SayMore.sln
%msbuildexe% /target:Build  /verbosity:detailed
%msbuildexe% /target:ConvertReleaseNotesToHtml;installer /property:Version=3.8.2000 /verbosity:detailed
popd
GOTO pauseforusertoseeoutput
:pauseforusertoseeoutput
ECHO %CMDCMDLINE% | findstr /i "/c" >nul
IF NOT errorlevel 1 PAUSE
