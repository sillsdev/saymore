IF NOT "%msbuildpath%"=="" GOTO unexpectedsystemvariableinuse
pushd .
copy "..\..\SaymoreDocumentation\SayMore.chm" ..\DistFiles
IF EXIST "\Program Files (x86)\MSBuild\14.0\Bin" SET msbuildpath="\Program Files (x86)\MSBuild\14.0\Bin\MSbuild"
ELSE IF EXIST "\Program Files (x86)\MSBuild\12.0\Bin" SET msbuildpath="\Program Files (x86)\MSBuild\12.0\Bin\MSbuild"
ELSE SET msbuildpath=MSbuild
%msbuildpath% /target:installer /property:teamcity_build_checkoutDir=..\ /verbosity:detailed /property:teamcity_dotnet_nunitlauncher_msbuild_task="notthere" /property:BUILD_NUMBER="*.*.6.789" /property:Configuration=Release /property:Minor="1"
popd
GOTO pauseforusertoseeoutput

:unexpectedsystemvariableinuse
@ECHO Unexpected system variable msbuildpath is in use. Value: %msbuildpath%
:pauseforusertoseeoutput
PAUSE