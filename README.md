# What is Sidenote?

Sidenote is a PowerShell module to automate OneNote tasks. It provides a OneNote drive (ON:\\) with
virtual folders representing OneNote hierarchy items like notebooks, sections and pages. Standard
PowerShell cmdlets such as Set-Location (aka 'cd'), Get-ChildItem (aka 'dir') and Where-Object (aka
'where') can be used to select scope and query OneNote data.

Sidenote is in an early experimental state.

# Development Prerequisites

Sidenote development needs OneNote 2016 (i.e. the classic or Win32 version of OneNote) to be installed.
[This site](https://support.office.com/en-us/article/Install-or-reinstall-OneNote-2016-for-Windows-c08068d8-b517-4464-9ff2-132cb9c45c08)
provides instructions on how to install OneNote 2016 if it is missing from your current Office
installation.

Development (devinstall, test) requires the following modules to be installed from the PowerShell
gallery:
- Gumby.File
- Gumby.Install
- Gumby.Log
- Gumby.Path

# Usage Prerequisites

Sidenote usage needs OneNote 2016 (i.e. the classic or Win32 version of OneNote) to be installed.
[This site](https://support.office.com/en-us/article/Install-or-reinstall-OneNote-2016-for-Windows-c08068d8-b517-4464-9ff2-132cb9c45c08)
provides instructions on how to install OneNote 2016 if it is missing from your current Office
installation.

Usage (i.e. the stuff in the ScriptPart directory) requires the following modules to be installed
from the PowerShell gallery:
- Gumby.Log
- Gumby.Path
- Gumby.String
- TreeView
- Window
