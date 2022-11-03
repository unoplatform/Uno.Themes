#!/bin/bash
set -euo pipefail
IFS=$'\n\t'

cd $BUILD_SOURCESDIRECTORY

mono "/Applications/Visual Studio.app/Contents/MonoBundle/MSBuild/Current/bin/MSBuild.dll" /m /r /p:Configuration=Release /p:Platform=$BUILD_PLATFORM /p:PackageVersion=$BUILD_PACKAGEVERSION /p:ApplicationVersion=$BUILD_APPLICATIONVERSION /detailedsummary /p:AotAssemblies=false /p:RestoreConfigFile=$BUILD_SOURCESDIRECTORY/nuget.config /p:PackageReleaseNotesFile=$BUILD_SOURCESDIRECTORY/build/CHANGELOG.md /bl:$BUILD_BINLOG $BUILD_SOLUTION