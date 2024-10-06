#minicover/bin/sh

dotnet restore
dotnet build
minicover instrument
minicover reset
dotnet test --no-build
minicover uninstrument
minicover htmlreport --threshold 90