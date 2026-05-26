REM IF NOT "%msbuildpath%"=="" GOTO unexpectedsystemvariableinuse
pushd .
copy "..\..\SaymoreDocumentation\SayMore.chm" ..\DistFiles
REM IF EXIST "\Program Files (x86)\MSBuild\14.0\Bin" SET msbuildpath="\Program Files (x86)\MSBuild\14.0\Bin\MSbuild"
REM ELSE IF EXIST "\Program Files (x86)\MSBuild\12.0\Bin" SET msbuildpath="\Program Files (x86)\MSBuild\12.0\Bin\MSbuild"
REM ELSE SET msbuildpath=MSbuild
REM %msbuildpath% /target:installer /property:teamcity_build_checkoutDir=..\ /verbosity:detailed /property:teamcity_dotnet_nunitlauncher_msbuild_task="notthere" /property:BUILD_NUMBER="*.*.6.789" /property:Configuration=Release"

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
msbuild /target:Build  /verbosity:detailed
msbuild /target:ConvertReleaseNotesToHtml;installer /property:Version=3.8.2000 /verbosity:detailed

IF DEFINED REVERT_FILES git checkout -- %REVERT_FILES%

popd
GOTO pauseforusertoseeoutput

REM :unexpectedsystemvariableinuse
REM @ECHO Unexpected system variable msbuildpath is in use. Value: %msbuildpath%
:pauseforusertoseeoutput
ECHO %CMDCMDLINE% | findstr /i /c:"/c" >nul
IF NOT errorlevel 1 PAUSE
