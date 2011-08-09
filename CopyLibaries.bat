REM For this to work, you need palaso as a sibling of this project
copy /Y ..\palaso\..\palaso\output\debug\Palaso.dll lib\dotnet
copy /Y ..\palaso\output\debug\Palaso.xml lib\dotnet
copy /Y ..\palaso\output\debug\Palaso.pdb lib\dotnet
copy /Y ..\palaso\output\debug\Palaso.TestUtilities.* lib\dotnet
copy /Y ..\palaso\output\debug\PalasoUIWindowsForms.dll lib\dotnet
copy /Y ..\palaso\output\debug\Palaso.Media.dll lib\dotnet
copy /Y ..\palaso\output\debug\Palaso.Media.xml lib\dotnet
copy /Y ..\palaso\output\debug\Palaso.Media.pdb lib\dotnet
copy /Y ..\palaso\output\debug\PalasoUIWindowsForms.xml lib\dotnet
copy /Y ..\palaso\output\debug\PalasoUIWindowsForms.pdb lib\dotnet
copy /Y ..\palaso\output\debug\Palaso.BuildTasks.dll build

REM now copy all that stuff to the debug folder
copy /Y lib\dotnet\Palaso*.* output\debug

pause