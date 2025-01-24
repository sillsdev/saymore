REM IF NOT "%msbuildpath%"=="" GOTO unexpectedsystemvariableinuse
pushd .
copy "..\..\SaymoreDocumentation\SayMore.chm" ..\DistFiles
REM IF EXIST "\Program Files (x86)\MSBuild\14.0\Bin" SET msbuildpath="\Program Files (x86)\MSBuild\14.0\Bin\MSbuild"
REM ELSE IF EXIST "\Program Files (x86)\MSBuild\12.0\Bin" SET msbuildpath="\Program Files (x86)\MSBuild\12.0\Bin\MSbuild"
REM ELSE SET msbuildpath=MSbuild
REM %msbuildpath% /target:installer /property:teamcity_build_checkoutDir=..\ /verbosity:detailed /property:teamcity_dotnet_nunitlauncher_msbuild_task="notthere" /property:BUILD_NUMBER="*.*.6.789" /property:Configuration=Release /property:Minor="1"
msbuild /target:Build /property:teamcity_build_checkoutDir=..\ /verbosity:detailed /property:BUILD_NUMBER="*.*.6.789" /property:Minor="1"
msbuild /target:ConvertReleaseNotesToHtml;installer /property:teamcity_build_checkoutDir=..\ /verbosity:detailed /property:BUILD_NUMBER="*.*.6.789" /property:Minor="1"
popd
GOTO pauseforusertoseeoutput

REM :unexpectedsystemvariableinuse
REM @ECHO Unexpected system variable msbuildpath is in use. Value: %msbuildpath%
:pauseforusertoseeoutput
PAUSE