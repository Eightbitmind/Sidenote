using module File
using module Gumby.Log
using module Gumby.Path
using module Install

param(
	[ValidateSet("Install", "Uninstall")]
	[string] $Action = "Install",

	[ValidateSet("AnyCPU")]
	[string] $Platform = "AnyCPU",

	[ValidateSet("Debug", "Release")]
	[string] $Configuration = "Debug",

	[string] $TargetDir = "$([System.Environment]::GetFolderPath(`"MyDocuments`"))\WindowsPowerShell\Modules\Sidenote"
)

$ProjectRootDir = (Get-Item "$PSScriptRoot\..")
$BuildOutputDir = (Get-Item "$ProjectRootDir\_Target\$Platform\$Configuration")

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

# [string] $logFilePath = "$env:TEMP\$(PathBaseName $PSCommandPath).log")
# if (Test-Path $logFilePath) { Remove-Item $logFilePath }
# $fileLogListener = [FileLogListener]::new($logFilePath)

$consoleLogListener = [ConsoleLogListener]::new()

try {
	[void]([Log]::Listeners.Add($consoleLogListener))

	switch ($Action) {
		"Install" {
			if (!(IsCurrentUserAdmin)) {
				Write-Error "This script action needs to be run with administrative privileges."
				return
			}
			UnloadModule "SideNote"
			CreateDirectoryIfNotExisting $TargetDir
			CreateSymbolicLinkIfNotExisting -Target "$ProjectRootDir\Sidenote.psd1" -Link "$TargetDir\Sidenote.psd1"
			CreateSymbolicLinkIfNotExisting -Target "$BuildOutputDir\Sidenote.dll"  -Link "$TargetDir\Sidenote.dll"
			CreateSymbolicLinkIfNotExisting -Target "$ProjectRootDir\ScriptModule\Sidenote.psm1"  -Link "$TargetDir\Sidenote.psm1"
		}

		"Uninstall" {
			UnloadModule "Sidenote"
			RemoveFileIfExisting "$TargetDir\Sidenote.dll"
			RemoveFileIfExisting "$TargetDir\Sidenote.psd1"
			RemoveDirectoryIfExistingAndEmpty $TargetDir
		}
	}

} finally {
	[Log]::Listeners.Remove($consoleLogListener)
}
