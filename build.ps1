Set-Location .\Moder.Core\
dotnet publish -c Release /p:PublishDir=bin\publish\x64 -p:Platform=x64 -p:DefineConstants=DISABLE_XAML_GENERATED_MAIN -p:PublishTrimmed=true -p:TrimMode=partial
Set-Location ..\