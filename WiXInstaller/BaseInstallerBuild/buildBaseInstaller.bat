@echo off

REM Command line arguments and defined properties.
SET AppName=%1
SHIFT
SET Major=%1
SHIFT
SET Minor=%1
SHIFT
SET Build=%1
SHIFT
SET Revision=%1
SHIFT

REM Version is all four parts of the version number
SET Version=%Major%.%Minor%.%Build%.%Revision%
REM Truncated version is the first three parts of the version number
SET TRUNCATEDVERSION=%Major%.%Minor%.%Build%

REM The Product Id Guid should be changed to a new guid whenever the third (build) number of the version changes.  Also, when the product number changes you should create a new base installer.
SET PRODUCTIDGUID=%1
SHIFT
SET UPGRADECODEGUID=%1
SHIFT

SET APPBUILDDIR=%1
SHIFT
SET APPDATADIR=%1
SHIFT

REM File to be invoked by desktop shortcut.
SET ShortcutTargetName=%AppName%.exe

REM To protect the password for the certfile, you can configure your build process to read the password from an unversioned file and pass it into the CERTPASS variable.
SET CERTPATH=%1
SHIFT
SET CERTPASS=%1
SHIFT
SET CopyrightYear=%1
SHIFT
SET Manufacturer=%1

cd WixInstaller/BaseInstallerBuild/

@echo on
@REM Harvest (heat) the application and data files.
heat.exe dir %APPBUILDDIR% -cg HarvestedAppFiles -gg -scom -sreg -sfrag -srd -sw5150 -sw5151 -dr APPFOLDER -var var.APPBUILDDIR -t KeyPathFix.xsl -out AppHarvest.wxs
heat.exe dir %APPDATADIR% -cg HarvestedDataFiles -gg -scom -sreg -sfrag -srd -sw5150 -sw5151 -dr DATAFOLDER -var var.APPDATADIR -t KeyPathFix.xsl -out DataHarvest.wxs

@REM Compile (candle) and Link (light) the MSI file.
candle.exe -dApplicationName=%AppName% -dManufacturer=%Manufacturer% -dVersionNumber=%Version% -dMajorVersion=%Major% -dMinorVersion=%Minor% -dAPPBUILDDIR=%APPBUILDDIR% -dAPPDATADIR=%APPDATADIR% -dUpgradeCode=%UPGRADECODEGUID% -dProductCode=%PRODUCTIDGUID% -dShortcutTargetName=%ShortcutTargetName% TemplateFramework.wxs AppHarvest.wxs DataHarvest.wxs WixUI_TemplateDialogFlow.wxs TemplateInstallDirDlg.wxs TemplateProgressDlg.wxs TemplateWelcomeDlg.wxs

light.exe TemplateFramework.wixobj AppHarvest.wixobj DataHarvest.wixobj WixUI_TemplateDialogFlow.wixobj TemplateInstallDirDlg.wixobj TemplateProgressDlg.wixobj TemplateWelcomeDlg.wixobj -ext WixUIExtension -ext WixUtilExtension.dll -cultures:en-us -loc TemplateWixUI_en-us.wxl -sw1076 -out %AppName%_%Version%.msi

signtool.exe sign /f %CERTPATH% /p %CERTPASS% %AppName%_%Version%.msi

@REM build the ONLINE EXE bundle.
candle.exe -dApplicationName=%AppName% -dYear=%CopyrightYear% -dManufacturer=%Manufacturer% -dVersionNumber=%Version% -dUpgradeCode=%UPGRADECODEGUID% -dTruncatedVersion=%TRUNCATEDVERSION% -ext WixUtilExtension -ext WixBalExtension -ext WixUIExtension -ext WixNetFxExtension -ext WixDependencyExtension TemplateBundle.wxs

light.exe TemplateBundle.wixobj -ext WixUIExtension -ext WixBalExtension -ext WixUtilExtension -ext WixNetFxExtension -ext WixDependencyExtension -out %AppName%_%Version%_Online.exe

@REM build the OFFLINE EXE bundle.
candle.exe -dApplicationName=%AppName% -dYear=%CopyrightYear% -dManufacturer=%Manufacturer% -dVersionNumber=%Version% -dUpgradeCode=%UPGRADECODEGUID% -dTruncatedVersion=%TRUNCATEDVERSION% -ext WixUtilExtension -ext WixBalExtension -ext WixUIExtension -ext WixNetFxExtension -ext WixDependencyExtension TemplateOfflineBundle.wxs

light.exe TemplateOfflineBundle.wixobj -ext WixUIExtension -ext WixBalExtension -ext WixUtilExtension -ext WixNetFxExtension -ext WixDependencyExtension -out %AppName%_%Version%_Offline.exe

@REM Sign the standard installer.
insignia -ib %AppName%_%Version%_Online.exe -o engine.exe
signtool.exe sign /f %CERTPATH% /p %CERTPASS% engine.exe
insignia -ab engine.exe %AppName%_%Version%_Online.exe -o %AppName%_%Version%_Online.exe
signtool.exe sign /f %CERTPATH% /p %CERTPASS% %AppName%_%Version%_Online.exe

@REM Sign the offline installer.
insignia -ib %AppName%_%Version%_Offline.exe -o engine.exe
signtool.exe sign /f %CERTPATH% /p %CERTPASS% engine.exe
insignia -ab engine.exe %AppName%_%Version%_Offline.exe -o %AppName%_%Version%_Offline.exe
signtool.exe sign /f %CERTPATH% /p %CERTPASS% %AppName%_%Version%_Offline.exe

@echo off
REM Cleanup debris from this build
DEL *.wixobj
DEL *.wixpdb
DEL engine.exe
DEL AppHarvest.wxs
DEL DataHarvest.wxs
