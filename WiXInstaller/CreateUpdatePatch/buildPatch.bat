@echo off
REM Change the value below for the installer you wish to build.
REM ideally this number is obtained dynamically from the build process.

SET AppName=%1
SHIFT
SET BaseVersion=%1
SHIFT
SET PatchVersion=%1
SHIFT

SET MASTERBUILDDIR=%1
SHIFT
SET UPDATEBUILDDIR=%1
SHIFT

SET MASTERDATADIR=%1
SHIFT
SET UPDATEDATADIR=%1
SHIFT

SET PRODUCTIDGUID=%1
SHIFT
SET UPGRADECODEGUID=%1
SHIFT
SET COMPGGS=%1
SHIFT

SET CERTPATH=%1
SHIFT
SET CERTPASS=%1
SHIFT
SET Manufacturer=%1

SET Baseline=%AppName%Patch
SET Family=%Baseline%Family

REM Parse version numbers into pieces
for /f "tokens=1,2,3,4 delims=/." %%a in ("%BaseVersion%") do set bmaj=%%a&set bmin=%%b&set bbuild=%%c&set brev=%%d

for /f "tokens=1,2,3,4 delims=/." %%a in ("%PatchVersion%") do set pmaj=%%a&set pmin=%%b&set pbuild=%%c&set prev=%%d

cd %AppName%/WixInstaller/CreateUpdatePatch/

@echo on
@REM Harvest the Paratext MASTER application
heat.exe dir %MASTERBUILDDIR% -cg HarvestedAppFiles -ag -scom -sreg -sfrag -srd -sw5150 -sw5151 -dr APPFOLDER -var var.MASTERBUILDDIR -out ./Master/AppHarvest.wxs
heat.exe dir %MASTERDATADIR% -cg HarvestedDataFiles -ag -scom -sreg -sfrag -srd -sw5150 -sw5151 -dr DATAFOLDER -var var.MASTERDATADIR -out ./Master/DataHarvest.wxs

@REM Build the No-UI msi containing the paratext MASTER files
candle.exe -dApplicationName=%AppName% -dMajorVersion=%bmaj% -dMinorVersion=%bmin% -dManufacturer=%Manufacturer% -dVersionNumber=%BaseVersion% -dMASTERBUILDDIR=%MASTERBUILDDIR% -dMASTERDATADIR=%MASTERDATADIR% -dUpgradeCode=%UPGRADECODEGUID% -dProductCode=%PRODUCTIDGUID% -dCompGGS=%COMPGGS% -out ./Master/ ./Master/AppNoUi.wxs ./Master/AppHarvest.wxs ./Master/DataHarvest.wxs
light.exe ./Master/AppNoUi.wixobj ./Master/AppHarvest.wixobj ./Master/DataHarvest.wixobj -ext WixUtilExtension.dll -sw1076 -out ./Master/%AppName%_%BaseVersion%.msi


@REM Harvest the Paratext UPDATE application
heat.exe dir %UPDATEBUILDDIR% -cg HarvestedAppFiles -ag -scom -sreg -sfrag -srd -sw5150 -sw5151 -dr APPFOLDER -var var.UPDATEBUILDDIR -out ./Update/AppHarvest.wxs
heat.exe dir %UPDATEDATADIR% -cg HarvestedDataFiles -ag -scom -sreg -sfrag -srd -sw5150 -sw5151 -dr DATAFOLDER -var var.UPDATEDATADIR -out ./Update/DataHarvest.wxs

@REM Build the No-UI msi containing the paratext UPDATE files
candle.exe -dApplicationName=%AppName% -dMajorVersion=%pmaj% -dMinorVersion=%pmin% -dManufacturer=%Manufacturer% -dVersionNumber=%PatchVersion% -dBaseVersionNumber=%BaseVersion% -dUPDATEBUILDDIR=%UPDATEBUILDDIR% -dUPDATEDATADIR=%UPDATEDATADIR% -dUpgradeCode=%UPGRADECODEGUID% -dProductCode=%PRODUCTIDGUID% -dCompGGS=%COMPGGS% -out ./Update/ ./Update/AppNoUi.wxs ./Update/AppHarvest.wxs ./Update/DataHarvest.wxs 
light.exe ./Update/AppNoUi.wixobj ./Update/AppHarvest.wixobj ./Update/DataHarvest.wixobj -ext WixUtilExtension.dll -sw1076 -out ./Update/%AppName%_%PatchVersion%.msi

@REM Create the transform between Master and Update
torch.exe -p -xi .\Master\%AppName%_%BaseVersion%.wixpdb .\Update\%AppName%_%PatchVersion%.wixpdb -out patch.wixmst

@REM Build the patch file
candle.exe -dAppName=%AppName% -dVersionNumber=%PatchVersion% -dProductCode=%PRODUCTIDGUID% -dManufacturer=%Manufacturer% -dPatchBaseline=%Baseline% -dPatchFamily=%Family% patch.wxs
light.exe patch.wixobj
pyro.exe patch.wixmsp -out %AppName%_%PatchVersion%.msp -t %Baseline% patch.wixmst

signtool.exe sign /f %CERTPATH% /p %CERTPASS% %AppName%_%PatchVersion%.msp


@echo off
REM Cleanup debris from this build
DEL *.wixobj
DEL *.wixpdb
DEL *.wixmst
DEL *.wixmsp
DEL .\Master\*.msi
DEL .\Master\*.wixobj
DEL .\Master\*.wixpdb
DEL .\Master\AppHarvest.wxs
DEL .\Master\DataHarvest.wxs
DEL .\Update\*.msi
DEL .\Update\*.wixobj
DEL .\Update\*.wixpdb
DEL .\Update\AppHarvest.wxs
DEL .\Update\DataHarvest.wxs
