$ModuleName = "DotNetNinja.KubernetesModule"
$ArchiveName = "$ModuleName.zip"

Compress-Archive -Path .\bin\Release\*.* -DestinationPath $ArchiveName -Force
