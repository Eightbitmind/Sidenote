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

[string] $ScriptBase = (Get-Item $PSCommandPath).BaseName
[datetime] $ScriptTime = Get-Date

$InstallUtil = Get-Item (Join-Path (Split-Path ([int].GetType().Assembly.Location)) "InstallUtil.exe")

$SnapInBaseName = "Sidenote"
$SnapInFileName = "$SnapInBaseName.dll"
$SnapIn = Get-Item "$PSScriptRoot\..\_Target\$Platform\$Configuration\$SnapInFileName"

$StartupScript = Get-Item "$PSScriptRoot\$SnapInBaseName.Startup.ps1"

[string] $ProfileFile = $profile.CurrentUserAllHosts

function Assert($Condition, $Message = "assertion failed")
{
	if (!$Condition) { throw $Message }
}

<#
.SYNOPSIS
	Values describing a text file encoding.
#>
enum TextFileEncoding
{
	UTF8
	Unicode
	UTF32
	UTF7
	ASCII
}

<#
.SYNOPSIS
	Gets text file encoding.

.PARAMETER Path
	Path and name of the text file whose encoding is to be determined.

.OUTPUTS
	'TextFileEncoding' enum value describing the text file encoding.

.DESCRIPTION
	The Get-FileEncoding function determines encoding by looking at the Byte Order Mark (BOM).
	It assumes the specified file ('Path' parameter) is an existing, non-zero length file.
#>
function GetTextFileEncoding ([string] $Path)
{
	Assert ((Get-Item $Path).Length -ge 4) "`"$Path`" is less than 4 bytes long"

	[byte[]] $bytes = Get-Content -Encoding byte -ReadCount 4 -TotalCount 4 -Path $Path

	if ($bytes[0] -eq 0xef -and $bytes[1] -eq 0xbb -and $bytes[2] -eq 0xbf)
	{
		return [TextFileEncoding]::UTF8
	}
	elseif ($bytes[0] -eq 0xfe -and $bytes[1] -eq 0xff)
	{
		return [TextFileEncoding]::Unicode
	}
	elseif ($bytes[0] -eq 0 -and $bytes[1] -eq 0 -and $bytes[2] -eq 0xfe -and $bytes[3] -eq 0xff)
	{
		return [TextFileEncoding]::UTF32
	}
	elseif ($bytes[0] -eq 0x2b -and $bytes[1] -eq 0x2f -and $bytes[2] -eq 0x76)
	{
		return [TextFileEncoding]::UTF7
	}
	else
	{
		return [TextFileEncoding]::ASCII
	}
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

function EnsureStartupScriptIsSourced()
{
	$ProfileLine = ". `"$($StartupScript.FullName)`""

	if (Test-Path $ProfileFile)
	{
		Write-Host "Profile file exists."
		$encoding = GetTextFileEncoding $ProfileFile

		foreach ($line in (Get-Content $ProfileFile))
		{
			if ($line -eq $ProfileLine)
			{
				Write-Host "Startup script is already being sourced."
				return
			}
		}

		Write-Host "Sourcing startup script."
		". `"$StartupScript`"" | Out-File -Append -Encoding:$encoding -Width 5000 -FilePath $ProfileFile
	}
	else
	{
		Write-Host "Creating profile file."

		$ProfileFolder = Split-Path $ProfileFile
		if (!(Test-Path $ProfileFolder))
		{
			mkdir $ProfileFolder | Out-Null
		}

		". `"$StartupScript`"" | Out-File -Encoding:unicode -Width 5000 -FilePath $ProfileFile
	}
}

function EnsureStartupScriptIsNotSourced()
{
	$ProfileLine = ". `"$($StartupScript.FullName)`""

	if (Test-Path $ProfileFile)
	{
		Write-Host "Profile file exists."
		$encoding = GetTextFileEncoding $ProfileFile

		$tempFileName = [System.IO.Path]::GetTempFileName() # creates temp file 

		[bool] $profileFileChanged = $false
		foreach ($line in (cat $ProfileFile))
		{
			if ($line -eq $ProfileLine)
			{
				$profileFileChanged = $true
			}
			else
			{
				$line | Out-File -Append -Encoding $encoding -Width 5000 -FilePath $tempFileName
			}
		}

		if ($profileFileChanged)
		{
			# check if profile file is empty after the removal of the startup script sourcing

			$tempFile = Get-Item $tempFileName

			if ($tempFile.Length -gt 0)
			{
				[string] $tempFileContent = Get-Content $tempFileName

				if ([string]::IsNullOrEmpty($tempFileContent) -or [string]::IsNullOrWhiteSpace($tempFileContent))
				{
					Write-Host "Removing empty profile file."
					Remove-Item -force $tempFileName
					Remove-Item -force $ProfileFile
				}
				else
				{
					Write-Host "Removing startup script sourcing from profile file."
					Move-Item -force $tempFileName $ProfileFile
				}
			}
			else
			{
				Write-Host "Removing empty profile file."
				Remove-Item -force $tempFileName
				Remove-Item -force $ProfileFile
			}
		}
		else
		{
			Write-Host "Profile file does not source startup script."
			Remove-Item $tempFileName
		}
	}
	else
	{
		Write-Host "Profile does not exist, nothing to do."
	}
}

switch ($Action)
{
	"Install" 
	{
		EnsureSnapInIsRegistered
		EnsureStartupScriptIsSourced
	}

	"Uninstall"
	{
		EnsureStartupScriptIsNotSourced
		EnsureSnapInIsUnregistered
	}
}
