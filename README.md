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

### Bug Reports

Reports can be entered in https://jira.sil.org/projects/SP/issues. They can be entered there via email by sending to saymore_issues@sil.org.

### Continuous Build System

Each time code is checked in, an automatic build begins on our [TeamCity build server](https://build.palaso.org/project.html?projectId=SayMore&tab=projectOverview), running all the unit tests. This automatic build doesn't publish a new installer, however. That kind of build is launched manually, by pressing a button on the TeamCity page. This "publish" process builds SayMore, makes and installer, rsyncs it to the distribution server, and writes out a little bit of html which the [SayMore download page](https://software.sil.org/saymore/download/) then displays to the user. It also creates a build artifact that enables the SayMore program to check to see whether a newer version is available.
