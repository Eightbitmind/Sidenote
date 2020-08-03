<#
.SYNOPSIS
	Installs or uninstalls development artifacts.

.DESCRIPTION
	This script aims to expedite the inner dev loop by installing development artifacts in such a
	way that they can be used right out of their respective source or target folders. For non-built
	artifacts such as scripts or module manifests, this typically means creating symbolic links from
	the module directory to the source locations; for built artifacts such as DLLs, this typically
	means creating symbolic links from the module directory to a file in the build target folder.
#>

using module Gumby.File
using module Gumby.Install
using module Gumby.Log
using module Gumby.Path

param(
	# Determines whether the script installs or uninstalls development artifacts.
	[ValidateSet("Install", "Uninstall")]
	[string] $Action = "Install",

	# Build target platform (needed for built development artifacts).
	[ValidateSet("AnyCPU")]
	[string] $Platform = "AnyCPU",

	# Build target configuration (needed for built development artifacts).
	[ValidateSet("Debug", "Release")]
	[string] $Configuration = "Debug",

	# Directory into which to install the module. Defaults to "WindowsPowerShell\Modules\Sidenote"
	# under the current users "MyDocuments" folder.
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
			CreateSymbolicLinkIfNotExisting -Target "$ProjectRootDir\Sidenote.psd1"                       -Link "$TargetDir\Sidenote.psd1"
			CreateSymbolicLinkIfNotExisting -Target "$BuildOutputDir\Sidenote.BinaryPart.dll"             -Link "$TargetDir\Sidenote.BinaryPart.dll"
			CreateSymbolicLinkIfNotExisting -Target "$ProjectRootDir\ScriptPart\Sidenote.ScriptPart.psm1" -Link "$TargetDir\Sidenote.ScriptPart.psm1"
		}

		"Uninstall" {
			UnloadModule "Sidenote"
			RemoveFileIfExisting "$TargetDir\Sidenote.psd1"
			RemoveFileIfExisting "$TargetDir\Sidenote.BinaryPart.dll"
			RemoveFileIfExisting "$TargetDir\Sidenote.ScriptPart.psm1"
			RemoveDirectoryIfExistingAndEmpty $TargetDir
		}
	}

} finally {
	[Log]::Listeners.Remove($consoleLogListener)
}
