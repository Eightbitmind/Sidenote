
param(
	[ValidateSet("Install", "Uninstall")]
	[string] $Action = "Install",

	[ValidateSet("AnyCPU")]
	[string] $Platform = "AnyCPU",

	[ValidateSet("Debug", "Release")]
	[string] $Configuration = "Debug",

	[string] $TargetDir = "$HOME\Tools\WindowsPowerShell\Modules\Sidenote"
	)

$SourceDir = (Get-Item "$PSScriptRoot\..\_Target\$Platform\$Configuration")

function MakeDirIfNotExisting($Path) {
	if (!(Test-Path $TargetDir)) {
		[void](mkdir $TargetDir)
		Write-Host "created directory `"$Path`""
	}
}

function RemoveDirIfExistingAndNotEmpty($Path) {
	if (Test-Path $Path) {
		$dirInfo = Get-Item $Path
		if (($dirInfo.GetDirectories().Count + $dirInfo.GetFiles().Count) -eq 0) {
			rmdir $Path
			Write-Host "removed directory `"$Path`""
		}
	}
}

function RemoveFileIfExisting($Path) {
	if (Test-Path $Path) {
		Remove-Item $Path
		Write-Host "removed file '$Path'"
	}
}

function CopyIfTargetNotExistingOrIsOlder($Source, $Target) {
	if (!(Test-Path $Source)) {
		throw "source of copy operation `"$Source`" does not exist, did the build succeed?"
	}

	if (!(Test-Path $Target)) {
		Copy-Item $Source $TargetDir
		Write-Host "copied `"$Source`" to `"$Target`""
	} else {
		$sourceTime = (Get-Item $Source).LastWriteTime
		$targetTime = (Get-Item $Target).LastWriteTime
		if ($sourceTime -gt $targetTime) {
			Copy-Item $Source $TargetDir
			Write-Host "copied `"$Source`" to `"$Target`""
		}
	}
}

function UnloadModule($ModuleName) {
	# TODO:
	# The 'Remove-Module' cmdlet removes the module from the current PS session, but does not cause
	# the PS host (powershell.exe) to unload any module assemblies.
	# We might have to restart the the PS host for that.
	if (Get-Module $ModuleName) {
		Remove-Module $ModuleName
		throw "The current PS session has the '$ModuleName' module loaded. Please restart the session before rerunning the command."
	}
}

switch ($Action) {
	"Install" {
		UnloadModule "SideNote"
		MakeDirIfNotExisting $TargetDir
		CopyIfTargetNotExistingOrIsOlder "$SourceDir\Sidenote.dll" "$TargetDir\Sidenote.dll"
	}

	"Uninstall" {
		UnloadModule "Sidenote"
		RemoveFileIfExisting "$TargetDir\Sidenote.dll"
		RemoveDirIfExistingAndNotEmpty $TargetDir
	}
}
