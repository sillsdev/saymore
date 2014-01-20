#!/bin/bash
# server=build.palaso.org
# build_type=bt318
# root_dir=.
# $Id: da666a7e5eb1d63b434514279cd14cacd26c730f $

# *** Functions ***
force=

while getopts f opt; do
	case $opt in
	f)
		force=1
		;;

	esac
done

shift $((OPTIND - 1))


copy_auto() {
	where_curl=$(type -P curl)
	where_wget=$(type -P wget)
	if [ "$where_curl" != "" ]
	then
		copy_curl $1 $2
	elif [ "$where_wget" != "" ]
	then
		copy_wget $1 $2
	else
		echo "Missing curl or wget"
		exit 1
	fi
}

copy_curl() {
	echo "curl: $2 <= $1"
	if [ -e "$2" ] && [ "$force" != "1" ]
	then
		curl -# -L -z $2 -o $2 $1
	else
		curl -# -L -o $2 $1
	fi
}

copy_wget() {
	echo "wget: $2 <= $1"
	f=$(basename $2)
	d=$(dirname $2)
	cd $d
	wget -q -L -N $1
	cd -
}

# *** Results ***
# build: SayMore-win-ArchivingIMDI Continuous (bt318)
# project: SayMore
# URL: http://build.palaso.org/viewType.html?buildTypeId=bt318
# VCS: http://hg.palaso.org/saymore [ArchivingIMDI]
# dependencies:
# [0] build: L10NSharp continuous (bt196)
#     project: L10NSharp
#     URL: http://build.palaso.org/viewType.html?buildTypeId=bt196
#     clean: false
#     revision: latest.lastSuccessful
#     paths: {"*.*"=>"lib/dotnet"}
#     VCS: https://bitbucket.org/hatton/l10nsharp []
# [1] build: palaso-win32-ArchivingIMDI Continuous (bt294)
#     project: libpalaso
#     URL: http://build.palaso.org/viewType.html?buildTypeId=bt294
#     clean: false
#     revision: latest.lastSuccessful
#     paths: {"Palaso.BuildTasks.dll"=>"build/"}
#     VCS: https://github.com/sillsdev/libpalaso.git []
# [2] build: palaso-win32-ArchivingIMDI Continuous (bt294)
#     project: libpalaso
#     URL: http://build.palaso.org/viewType.html?buildTypeId=bt294
#     clean: false
#     revision: latest.lastSuccessful
#     paths: {"*.dll"=>"lib/dotnet"}
#     VCS: https://github.com/sillsdev/libpalaso.git []
# [3] build: SayMore-Documentation (bt76)
#     project: SayMore
#     URL: http://build.palaso.org/viewType.html?buildTypeId=bt76
#     clean: false
#     revision: latest.lastSuccessful
#     paths: {"*.chm"=>"DistFiles"}
#     VCS: http://hg.palaso.org/saymore-doc []

# make sure output directories exist
mkdir -p ./lib/dotnet
mkdir -p ./build/
mkdir -p ./DistFiles

# download artifact dependencies
copy_auto http://build.palaso.org/guestAuth/repository/download/bt196/latest.lastSuccessful/L10NSharp.1.2.5.nupkg ./lib/dotnet/L10NSharp.1.2.5.nupkg
copy_auto http://build.palaso.org/guestAuth/repository/download/bt196/latest.lastSuccessful/L10NSharp.dll ./lib/dotnet/L10NSharp.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/bt196/latest.lastSuccessful/L10NSharp.pdb ./lib/dotnet/L10NSharp.pdb
copy_auto http://build.palaso.org/guestAuth/repository/download/bt196/latest.lastSuccessful/L10NSharp.xml ./lib/dotnet/L10NSharp.xml
copy_auto http://build.palaso.org/guestAuth/repository/download/bt196/latest.lastSuccessful/L10NSharpTests.dll ./lib/dotnet/L10NSharpTests.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/bt196/latest.lastSuccessful/L10NSharpTests.pdb ./lib/dotnet/L10NSharpTests.pdb
copy_auto http://build.palaso.org/guestAuth/repository/download/bt196/latest.lastSuccessful/nunit.framework.dll ./lib/dotnet/nunit.framework.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/bt294/latest.lastSuccessful/Palaso.BuildTasks.dll ./build/Palaso.BuildTasks.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/bt294/latest.lastSuccessful/Interop.WIA.dll ./lib/dotnet/Interop.WIA.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/bt294/latest.lastSuccessful/Ionic.Zip.dll ./lib/dotnet/Ionic.Zip.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/bt294/latest.lastSuccessful/L10NSharp.dll ./lib/dotnet/L10NSharp.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/bt294/latest.lastSuccessful/Palaso.BuildTasks.dll ./lib/dotnet/Palaso.BuildTasks.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/bt294/latest.lastSuccessful/Palaso.DictionaryServices.dll ./lib/dotnet/Palaso.DictionaryServices.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/bt294/latest.lastSuccessful/Palaso.Lift.dll ./lib/dotnet/Palaso.Lift.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/bt294/latest.lastSuccessful/Palaso.Media.dll ./lib/dotnet/Palaso.Media.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/bt294/latest.lastSuccessful/Palaso.TestUtilities.dll ./lib/dotnet/Palaso.TestUtilities.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/bt294/latest.lastSuccessful/Palaso.Tests.dll ./lib/dotnet/Palaso.Tests.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/bt294/latest.lastSuccessful/Palaso.dll ./lib/dotnet/Palaso.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/bt294/latest.lastSuccessful/PalasoUIWindowsForms.dll ./lib/dotnet/PalasoUIWindowsForms.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/bt294/latest.lastSuccessful/SIL.Archiving.dll ./lib/dotnet/SIL.Archiving.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/bt294/latest.lastSuccessful/icu.net.dll ./lib/dotnet/icu.net.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/bt294/latest.lastSuccessful/icudt40.dll ./lib/dotnet/icudt40.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/bt294/latest.lastSuccessful/icuin40.dll ./lib/dotnet/icuin40.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/bt294/latest.lastSuccessful/icuuc40.dll ./lib/dotnet/icuuc40.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/bt76/latest.lastSuccessful/SayMore.chm ./DistFiles/SayMore.chm
# End of script
