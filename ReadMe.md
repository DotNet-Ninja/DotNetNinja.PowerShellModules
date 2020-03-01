# DotNetNinja.PowerShellModules
A solution for PowerShell Modules written in C#

[![Build Status](https://dev.azure.com/chaosmonkey/DotNetNinja.PowerShell/_apis/build/status/DotNet-Ninja.DotNetNinja.PowerShellModules?branchName=master)](https://dev.azure.com/chaosmonkey/DotNetNinja.PowerShell/_build/latest?definitionId=26&branchName=master)

## DotNetNinja.KubernetesModule

This module contains a single cmdlet that will add/update hosts file entries for the ingresses you have configured in your Kubernetes cluster.  This is helpful when you are (for example) running Minikube locally.  In my case I have ingress set up for the kubernetes dashboard and kiali, but it should work for any ingress that is an http(s) endpoint.

### Installation

* Download the [latest Release](https://github.com/DotNet-Ninja/DotNetNinja.PowerShellModules/releases)
* Extract the archive
* Run Install-Module.ps1
  * This will copy all of the files to your Modules folder under your PowerShell profile.

Or from PowerShell run (Run As Admin):
```
 Invoke-WebRequest https://github.com/DotNet-Ninja/DotNetNinja.PowerShellModules/releases/download/v1.0.0/DotNetNinja.KubernetesModule.zip -OutFile DotNetNinja.KubernetesModule.zip; Unblock-File .\DotNetNinja.KubernetesModule.zip; Expand-Archive .\DotNetNinja.KubernetesModule.zip; Push-Location; Set-Location .\DotNetNinja.KubernetesModule; .\Install-Module.ps1; Pop-Location; Remove-Item .\DotNetNinja.KubernetesModule -Force -Recurse; Remove-Item .\DotNetNinja.KubernetesModule.zip -Force
```

This will:
* Download the release archive file to your current working directory.
* Unblock the downloaded file.
* Extract the archive.
* Change to the extracted directory.
* Run the Install-Module.ps1 script.
  * This script copies all of the files to your Modules folder under your PowerShell profile.
* Switch back to your original working directory.
* Remove the extracted files.
* Remove the downloaded archive.

### Usage

To use the module you will need to be running with elevated priviledges (Admin).  This Cmdlet modifies your hosts file, and it cannot do that without elevated priviledges.

Before you can use the module you will need to import it. 
```
Import-Module DotNetNinja.KubernetesModule
```

To add/update all of the ingresses in your cluster to your hosts file:
```
Update-HostsForIngress
```

You should get a response like:
```
HostName                    Status OldAddress    NewAddress
--------                    ------ ----------    ----------
dashboard.minikube.local Unchanged 192.168.43.51 192.168.43.51
kiali.minikube.local     Unchanged 192.168.43.51 192.168.43.51
```

If you want to only update the entry for a single HostName you can use:
```
 Update-HostsForIngress -HostNames dashboard.minikube.local
```

You should get a response like:
```
HostName                    Status OldAddress    NewAddress
--------                    ------ ----------    ----------
dashboard.minikube.local Unchanged 192.168.43.51 192.168.43.51
```
If you want to update the entries for a specific lsit of HostNames you can use:
```
 Update-HostsForIngress -HostNames dashboard.minikube.local,kiali.minikube.local
```

You should get a response like:
```
HostName                    Status OldAddress    NewAddress
--------                    ------ ----------    ----------
dashboard.minikube.local Unchanged 192.168.43.51 192.168.43.51
kiali.minikube.local     Unchanged 192.168.43.51 192.168.43.51
```