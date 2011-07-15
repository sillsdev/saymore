pushd .
cd ..\palaso
hg pull -u

call "C:\Program Files (x86)\Microsoft Visual Studio 10.0\VC\vcvarsall.bat"

msbuild "Palaso VS2010.sln" /p:warn=false

popd

call "copyLibaries.bat"
