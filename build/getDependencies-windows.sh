#!/bin/bash
# server=build.palaso.org
# build_type=bt34
# root_dir=..
# $Id: fddd609c79cf98392f7892a4a56d48a466d329de $

cd "$(dirname "$0")"

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
# build: SayMore-win-default Continuous (bt34)
# project: SayMore
# URL: http://build.palaso.org/viewType.html?buildTypeId=bt34
# VCS: http://hg.palaso.org/saymore [default]
# dependencies:
# [0] build: L10NSharp continuous (bt196)
#     project: L10NSharp
#     URL: http://build.palaso.org/viewType.html?buildTypeId=bt196
#     clean: false
#     revision: latest.lastSuccessful
#     paths: {"*.*"=>"lib/dotnet"}
#     VCS: https://bitbucket.org/hatton/l10nsharp []
# [1] build: palaso-win32-master Continuous (bt440)
#     project: libpalaso
#     URL: http://build.palaso.org/viewType.html?buildTypeId=bt440
#     clean: false
#     revision: latest.lastSuccessful
#     paths: {"Palaso.BuildTasks.dll"=>"build/"}
#     VCS: https://github.com/sillsdev/libpalaso.git []
# [2] build: palaso-win32-master Continuous (bt440)
#     project: libpalaso
#     URL: http://build.palaso.org/viewType.html?buildTypeId=bt440
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
mkdir -p ../lib/dotnet
mkdir -p ../build/
mkdir -p ../DistFiles

# download artifact dependencies
copy_auto http://build.palaso.org/guestAuth/repository/download/bt436/latest.lastSuccessful/Palaso.BuildTasks.dll ../build/Palaso.BuildTasks.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/bt436/latest.lastSuccessful/Ionic.Zip.dll ../lib/dotnet/Ionic.Zip.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/bt436/latest.lastSuccessful/icu.net.dll.config?branch=%3Cdefault%3E ../lib/dotnet/icu.net.dll.config
copy_auto http://build.palaso.org/guestAuth/repository/download/bt436/latest.lastSuccessful/icu.net.dll?branch=%3Cdefault%3E ../lib/dotnet/icu.net.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/bt436/latest.lastSuccessful/icudt54.dll?branch=%3Cdefault%3E ../lib/dotnet/icudt54.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/bt436/latest.lastSuccessful/icuin54.dll?branch=%3Cdefault%3E ../lib/dotnet/icuin54.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/bt436/latest.lastSuccessful/icuuc54.dll?branch=%3Cdefault%3E ../lib/dotnet/icuuc54.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/bt436/latest.lastSuccessful/L10NSharp.dll ../lib/dotnet/L10NSharp.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/bt436/latest.lastSuccessful/L10NSharp.pdb ../lib/dotnet/L10NSharp.pdb
copy_auto http://build.palaso.org/guestAuth/repository/download/bt436/latest.lastSuccessful/SIL.Core.dll ../lib/dotnet/SIL.Core.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/bt436/latest.lastSuccessful/SIL.Core.pdb ../lib/dotnet/SIL.Core.pdb
copy_auto http://build.palaso.org/guestAuth/repository/download/bt436/latest.lastSuccessful/Interop.WIA.dll ../lib/dotnet/Interop.WIA.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/bt436/latest.lastSuccessful/SIL.Media.dll ../lib/dotnet/SIL.Media.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/bt436/latest.lastSuccessful/SIL.Media.pdb ../lib/dotnet/SIL.Media.pdb
copy_auto http://build.palaso.org/guestAuth/repository/download/bt436/latest.lastSuccessful/SIL.TestUtilities.dll ../lib/dotnet/SIL.TestUtilities.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/bt436/latest.lastSuccessful/SIL.TestUtilities.pdb ../lib/dotnet/SIL.TestUtilities.pdb
copy_auto http://build.palaso.org/guestAuth/repository/download/bt436/latest.lastSuccessful/SIL.Windows.Forms.dll ../lib/dotnet/SIL.Windows.Forms.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/bt436/latest.lastSuccessful/SIL.Windows.Forms.pdb ../lib/dotnet/SIL.Windows.Forms.pdb
copy_auto http://build.palaso.org/guestAuth/repository/download/bt436/latest.lastSuccessful/SIL.Archiving.dll ../lib/dotnet/SIL.Archiving.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/bt436/latest.lastSuccessful/SIL.Archiving.pdb ../lib/dotnet/SIL.Archiving.pdb
copy_auto http://build.palaso.org/guestAuth/repository/download/bt436/latest.lastSuccessful/SIL.Windows.Forms.WritingSystems.dll ../lib/dotnet/SIL.Windows.Forms.WritingSystems.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/bt436/latest.lastSuccessful/SIL.Windows.Forms.WritingSystems.pdb ../lib/dotnet/SIL.Windows.Forms.WritingSystems.pdb
copy_auto http://build.palaso.org/guestAuth/repository/download/bt436/latest.lastSuccessful/SIL.WritingSystems.dll ../lib/dotnet/SIL.WritingSystems.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/bt436/latest.lastSuccessful/SIL.WritingSystems.pdb ../lib/dotnet/SIL.WritingSystems.pdb
copy_auto http://build.palaso.org/guestAuth/repository/download/bt436/latest.lastSuccessful/icu.net.dll ../lib/dotnet/icu.net.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/bt436/latest.lastSuccessful/icudt54.dll ../lib/dotnet/icudt54.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/bt436/latest.lastSuccessful/icuin54.dll ../lib/dotnet/icuin54.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/bt436/latest.lastSuccessful/icuuc54.dll ../lib/dotnet/icuuc54.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/bt76/latest.lastSuccessful/SayMore.chm ../DistFiles/SayMore.chm
# End of script
