
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
	if (!(Test-Path $TargetDir)) { [void](mkdir $TargetDir) }
}

function CopyIfTargetDoesNotExistOrIsOlder($Source, $Target) {
	if (!(Test-Path $Source)) {
		throw "source of copy operation '$Source' does not exist, did the build succeed?"
	}

	if (!(Test-Path $Target)) {
		Copy-Item $Source $TargetDir
	} else {
		$sourceTime = (Get-Item $Source).LastWriteTime
		$targetTime = (Get-Item $Target).LastWriteTime
		if ($sourceTime -gt $targetTime) { Copy-Item $Source $TargetDir }
	}
}

switch ($Action) {
	"Install" {
		MakeDirIfNotExisting $TargetDir
		CopyIfTargetDoesNotExistOrIsOlder "$SourceDir\Sidenote.dll" "$TargetDir\Sidenote.dll"
	}

	"Uninstall" {
		#EnsureStartupScriptIsNotSourced
		#EnsureSnapInIsUnregistered
	}
}
