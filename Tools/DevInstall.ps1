#Requires -RunAsAdministrator

param(
	[ValidateSet("Install", "Uninstall")]
	[string] $Action = "Install",

	[ValidateSet("AnyCPU")]
	[string] $Platform = "AnyCPU",

	[ValidateSet("Debug", "Release")]
	[string] $Configuration = "Debug"

	)

$Error.Clear()

[string] $ScriptDir = Split-Path $PSCommandPath
[string] $ScriptBase = (Get-Item $PSCommandPath).BaseName
[datetime] $ScriptTime = Get-Date

$InstallUtil = Get-Item (Join-Path (Split-Path ([int].GetType().Assembly.Location)) "InstallUtil.exe")

$SnapInBaseName = "Sidenote"
$SnapInFileName = "$SnapInBaseName.dll"
$SnapIn = Get-Item "$ScriptDir\..\_Target\$Platform\$Configuration\$SnapInFileName"

[string] $ProfileFile = $profile.CurrentUserAllHosts


function Assert($Condition, $Message = "assertion failed")
{
	if (!$Condition) { throw $Message }
}

function RegisterSnapIn()
{
	Write-Host "Registering $($SnapIn.FullName)"
	$installLogFile = "$env:TEMP\$SnapInBaseName-$ScriptBase-{0:yyyyMMdd-HHmmss}.InstallLog" -f $ScriptTime
	& $InstallUtil /LogFile=$installLogFile $SnapIn.FullName
}

function UnregisterSnapIn()
{
	Write-Host "Unregistering developer snap-in."
	$uninstallLogFile = "$env:TEMP\$SnapInBaseName-$ScriptBase-{0:yyyyMMdd-HHmmss}.UninstallLog" -f $ScriptTime
	& $InstallUtil /uninstall /LogFile=$uninstallLogFile $SnapIn.FullName
}

function EnsureSnapInIsRegistered()
{
	Write-Host "Checking snap-in registration."

	if (Test-Path "HKLM:\SOFTWARE\Microsoft\PowerShell\1\PowerShellSnapIns\$SnapInBaseName")
	{
		Write-Host "Snap-in registry key found."

		$moduleNameProperty = Get-ItemProperty "HKLM:\SOFTWARE\Microsoft\PowerShell\1\PowerShellSnapIns\$SnapInBaseName" -Name ModuleName
		Assert ($moduleNameProperty -ne $null) "Registration does not have ModuleName property."

		$registeredSnapIn = $moduleNameProperty.ModuleName

		if ($registeredSnapIn -ieq $SnapIn.FullName)
		{
			Write-Host "Developer snap-in appears to be registered."
		}
		else
		{
			throw "A non-developer version of the snap-in (`"$registeredSnapIn`") appears to be installed, please uninstall it prior to rerunning this script."
		}
	}
	else
	{
		Write-Host "Snap-in not registered."
		RegisterSnapIn
	}
}

function EnsureSnapInIsUnregistered()
{
	Write-Host "Checking snap-in registration."

	if (Test-Path "HKLM:\SOFTWARE\Microsoft\PowerShell\1\PowerShellSnapIns\$SnapInBaseName")
	{
		Write-Host "Snap-in registry key found."

		$moduleNameProperty = Get-ItemProperty "HKLM:\SOFTWARE\Microsoft\PowerShell\1\PowerShellSnapIns\$SnapInBaseName" -Name ModuleName
		Assert ($moduleNameProperty -ne $null) "Registration does not have ModuleName property."

		$registeredSnapIn = $moduleNameProperty.ModuleName

		if ($registeredSnapIn -ieq $SnapIn.FullName)
		{
			UnregisterSnapIn
		}
		else
		{
			throw "A non-developer version of the snap-in (`"$registeredSnapIn`") appears to be installed, please uninstall it prior to rerunning this script."
		}
	}
	else
	{
		Write-Host "Snap-in not registered."
	}
}

switch ($Action)
{
	"Install" 
	{
		EnsureSnapInIsRegistered
	}

	"Uninstall"
	{
		EnsureSnapInIsUnregistered
	}
}
