pushd .\
FOR /F "tokens=*" %%i IN ('"C:\Program Files (x86)\Microsoft Visual Studio\Installer\vswhere.exe" -latest -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe') DO SET msbuildexe="%%i"

REM Remember which build-modified files are currently clean so we can restore them afterwards.
REM Files with pre-existing unstaged changes are left alone.
SET REVERT_FILES=
git diff --quiet -- ..\DistFiles\releaseNotes.md
IF NOT ERRORLEVEL 1 SET REVERT_FILES=%REVERT_FILES% ../DistFiles/releaseNotes.md
git diff --quiet -- ..\src\Installer\Installer.wxs
IF NOT ERRORLEVEL 1 SET REVERT_FILES=%REVERT_FILES% ../src/Installer/Installer.wxs
git diff --quiet -- ..\src\AutoSegmenter\Properties\AssemblyInfo.cs
IF NOT ERRORLEVEL 1 SET REVERT_FILES=%REVERT_FILES% ../src/AutoSegmenter/Properties/AssemblyInfo.cs
git diff --quiet -- ..\src\SayMore\Properties\AssemblyInfo.cs
IF NOT ERRORLEVEL 1 SET REVERT_FILES=%REVERT_FILES% ../src/SayMore/Properties/AssemblyInfo.cs
git diff --quiet -- ..\src\SayMoreTests\Properties\AssemblyInfo.cs
IF NOT ERRORLEVEL 1 SET REVERT_FILES=%REVERT_FILES% ../src/SayMoreTests/Properties/AssemblyInfo.cs

dotnet restore ..\SayMore.sln
%msbuildexe% /target:Build  /verbosity:detailed
%msbuildexe% /target:ConvertReleaseNotesToHtml;installer /property:Version=3.8.2000 /verbosity:detailed

IF DEFINED REVERT_FILES git checkout -- %REVERT_FILES%

popd
GOTO pauseforusertoseeoutput
:pauseforusertoseeoutput
ECHO %CMDCMDLINE% | findstr /i /c:"/c" >nul
IF NOT errorlevel 1 PAUSE
