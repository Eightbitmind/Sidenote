# What is Sidenote?

Sidenote is a PowerShell module to automate OneNote tasks. It provides a OneNote drive (ON:\\) with
virtual folders representing OneNote hierarchy items like notebooks, sections and pages. Standard
PowerShell cmdlets such as Set-Location (aka 'cd'), Get-ChildItem (aka 'dir') and Where-Object (aka
'where') can be used to select scope and query OneNote data.

Sidenote is in an early experimental state.

# Development Prerequisites

Development (devinstall, test) requires the following modules to be installed from the PowerShell
gallery:
- Gumby.File
- Gumby.Install
- Gumby.Log
- Gumby.Path

# Usage Prerequisites

Usage (i.e. the stuff in the ScriptPart directory) requires the following modules to be installed
from the PowerShell gallery:
- Gumby.Log
- Gumby.Path
- Gumby.String
- TreeView
- Window
