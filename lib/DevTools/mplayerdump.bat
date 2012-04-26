@ECHO OFF
IF "%~2"=="-all" GOTO All
IF "%~2"=="-ALL" GOTO All
IF "%~2"=="-All" GOTO All

:Loop
IF "%~1"=="" GOTO End
mkdir "%~p1Output"
..\..\DistFiles\MediaInfo\MediaInfo --output=xml "%~1" > "%~p1Output\%~n1.xml"
SHIFT
GOTO Loop

:All
FOR %%f in (%~1\*.*) do (mplayerdump "%%f")

:End
@ECHO ON