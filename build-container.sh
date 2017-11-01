#!bin/bash
set -e
dotnet restore
dotnet test test/custom-serilog-formatter.test/custom-serilog-formatter.test.csproj -xml $(pwd)/testresults/out.xml
rm -rf $(pwd)/package
dotnet pack src/custom-serilog-formatter/custom-serilog-formatter.csproj -c release -o $(pwd)/package --version-suffix=${BuildNumber}
mkdir $(pwd)/symbols
cp $(pwd)/package/*.symbols.nupkg $(pwd)/symbols
rm $(pwd)/package/*.symbols.nupkg