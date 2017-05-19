REM For this to work, you need palaso as a sibling of this project
set palaso=libpalaso
if NOT EXIST ..\%palaso% set palaso=palaso

copy /Y ..\%palaso%\output\debug\icu.net.dll.config lib\dotnet
copy /Y ..\%palaso%\output\debug\icu.net.dll lib\dotnet
copy /Y ..\%palaso%\output\debug\icudt56.dll lib\dotnet
copy /Y ..\%palaso%\output\debug\icuin56.dll lib\dotnet
copy /Y ..\%palaso%\output\debug\icuuc56.dll lib\dotnet
copy /Y ..\%palaso%\output\debug\SIL.Core.dll lib\dotnet
copy /Y ..\%palaso%\output\debug\SIL.Core.pdb lib\dotnet
copy /Y ..\%palaso%\output\debug\Interop.WIA.dll lib\dotnet
copy /Y ..\%palaso%\output\debug\SIL.Media.dll lib\dotnet
copy /Y ..\%palaso%\output\debug\SIL.Media.pdb lib\dotnet
copy /Y ..\%palaso%\output\debug\SIL.TestUtilities.dll lib\dotnet
copy /Y ..\%palaso%\output\debug\SIL.TestUtilities.pdb lib\dotnet
copy /Y ..\%palaso%\output\debug\SIL.Windows.Forms.dll lib\dotnet
copy /Y ..\%palaso%\output\debug\SIL.Windows.Forms.pdb lib\dotnet
copy /Y ..\%palaso%\output\debug\SIL.Archiving.dll lib\dotnet
copy /Y ..\%palaso%\output\debug\SIL.Archiving.pdb lib\dotnet
copy /Y ..\%palaso%\output\debug\Ionic.Zip.dll lib\dotnet
copy /Y ..\%palaso%\output\debug\SIL.Windows.Forms.WritingSystems.dll lib\dotnet
copy /Y ..\%palaso%\output\debug\SIL.Windows.Forms.WritingSystems.pdb lib\dotnet
copy /Y ..\%palaso%\output\debug\SIL.WritingSystems.dll lib\dotnet
copy /Y ..\%palaso%\output\debug\SIL.WritingSystems.pdb lib\dotnet
copy /Y ..\%palaso%\output\debug\icu.net.dll lib\dotnet
copy /Y ..\%palaso%\output\debug\icudt56.dll lib\dotnet
copy /Y ..\%palaso%\output\debug\icuin56.dll lib\dotnet
copy /Y ..\%palaso%\output\debug\icuuc56.dll lib\dotnet

copy /Y ..\L10NSharp\Output\Debug\L10NSharp.dll lib\dotnet
copy /Y ..\L10NSharp\Output\Debug\L10NSharp.pdb lib\dotnet

copy /Y ..\%palaso%\output\debug\Palaso.BuildTasks.dll build

REM now copy all that stuff to the debug folder
copy /Y lib\dotnet\icu*.dll output\debug
copy /Y lib\dotnet\Ionic.Zip.dll output\debug
copy /Y lib\dotnet\SIL.* output\debug
copy /Y lib\dotnet\L10NSharp.* output\debug

pause