$ModuleName = "DotNetNinja.KubernetesModule"
$ProfileDirectory = ([System.IO.FileInfo]$PROFILE).Directory
if($ProfileDirectory.Exists){
    $ModulesDirectory = $ProfileDirectory.FullName + "\Modules";
    if(!(Test-Path($ModulesDirectory))){
        New-Item -ItemType Directory $ModulesDirectory
    }
    $MyModuleDirectory = $ModulesDirectory + "\" + $ModuleName
    if(!(Test-Path($MyModuleDirectory))){
        New-Item -ItemType Directory $MyModuleDirectory
    }
    Copy-Item .\*.* $MyModuleDirectory
}
else{
    Write-Warning "PowerShell Profile not found!"
}
