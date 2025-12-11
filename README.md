# SayMore™

SayMore provides an intuitive and enjoyable way to manage language documentation tasks.

## Users

Instead of reading the rest of this document, please visit https://software.sil.org/saymore/ to download the installer and get general information. Our [community site](https://community.software.sil.org/c/saymore) is a great place to interact with other users and get support for using the program.

### Testers

Please see [Tips for Testing Palaso Software](https://docs.google.com/document/d/1dkp0edjJ8iqkrYeXdbQJcz3UicyilLR7GxMRIUAGb1E/edit)

## Developers

### Getting dependencies

1. If you need/want the Help file, ensure you have something that can run bash scripts, and run build/getDependencies-windows.sh
1. Ensure you have Nuget
1. Building the solution should automatically pull down the nuget dependencies.

### Local Installer Build
The process of building a SayMore installer involves some build steps that are defined in build/SayMore.proj, so it must be built using MSBuild. This involves multiple steps, so the easiest way is to run build/TestInstallerBuild.bat. This batch file should be maintained to keep it in sync with any changes to the overall build process. Note that during the Installer build, a few source files are updated (stamped with the version number and/or date), so these should be reverted after a test build of the installer. Committing and pushing these changes will break the ability of the CI build to correctly set the version number!


### Bug Reports

Reports can be entered in https://jira.sil.org/projects/SP/issues. They can be entered there via email by sending to saymore_issues@sil.org.

## Github Actions CI
Every PR triggers a build and runs tests.

Every commit on the main branch will produce a signed installer for user testing as desired.  Simply go to the Actions tab and find the Actions run for that commit if you want to download the installer.  The installer is attached to the run artifact.

# Release Process
To release a new version, just tag a commit on the main branch e.g. v3.7.5
This will trigger Github Actions to produce a Release with the signed installer attached

# Release TODO
Implement the "publish" process in GHA (currently working only on TeamCity), which builds SayMore, makes and installer, rsyncs it to the distribution server, and writes out a little bit of html which the [SayMore download page](https://software.sil.org/saymore/download/) then displays to the user. It also creates a build artifact that enables the SayMore program to check to see whether a newer version is available.