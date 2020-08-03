<#
.SYNOPSIS
	Installs modules needed for project development.

.DESCRIPTION
	This script should be run after cloning the project repository. Barring changes in the project
	build or test process, there should be no need to run this script again. Ideally, this script
	should be idempotent.
	
	As there is no good way to determine if the modules installed by this script are needed outside
	of project development, the script does not support uninstalling modules.
#>

# Keep at least two blank lines between the above script help and following function declarations.

function InstallModule($ModuleName) {
	Install-Module -Scope CurrentUser -Name $ModuleName
}

InstallModule "Gumby.File"
InstallModule "Gumby.Install"
InstallModule "Gumby.Log"
InstallModule "Gumby.Path"
InstallModule "Gumby.String"
InstallModule "Gumby.Test"
