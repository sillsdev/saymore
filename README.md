# Users

You're in the wrong place. Head over to https://software.sil.org/saymore/.

## Testers

Please see [Tips for Testing Palaso Software](https://docs.google.com/document/d/1dkp0edjJ8iqkrYeXdbQJcz3UicyilLR7GxMRIUAGb1E/edit)

# Developers

## Getting dependencies

1. Ensure you have something that can run bash scripts
1. Run build/getDependencies-windows.sh
1. Ensure you have Nuget
1. Building the solution should automatically pull down the nuget dependencies.

## RoadMap / Day-to-day progress

See the [SayMore Trello Board](https://trello.com/board/saymore/4f1213c597586fed5d005bac)

## Bug Reports

Reports can be entered in https://jira.sil.org/projects/SP/issues.  They can be entered there via email by sending to issues at saymore dot palaso dot org.

## Continuous Build System

Each time code is checked in, an automatic build begins on our [TeamCity build server](http://build.palaso.org/project.html?projectId=project16&tab=projectOverview), running all the unit tests. Similarly, when there is a new version of some SayMore dependency (e.g. Palaso, LocalizationManager), that server automatically rebuilds SayMore . This automatic build doesn't publish a new installer, however. That kind of build is launched manually, by pressing a button on the TeamCity page.  This "publish" process builds SayMore , makes and installer, rsyncs it to the distribution server, and writes out a little bit of html which the [SayMore download page](http://software.sil.org/saymore/download/) then displays to the user.
