#!bin/bash
set -e
dotnet restore
cd  test/custom-serilog-formatter.test
dotnet restore
dotnet xunit -xml $(pwd)/../../testresults/out.xml
cd -
rm -rf $(pwd)/package
dotnet pack src/custom-serilog-formatter/custom-serilog-formatter.csproj -c release -o $(pwd)/package --version-suffix=${BuildNumber}