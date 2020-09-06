using module Gumby.Path
using module Gumby.Test

using module Sidenote

# Shorthands for the GUID-based path elements
$PathElements = @{
	"SidenoteTest" = @{
		"UTData" = @{
			"Page1" = @{
				"Outline1" = @{ __idfunc = { $args[0].ID }
					"USA" = @{ __idfunc = { $args[0].ID }
						"Oregon" = @{}
						"Washington" = @{}
					}
				}
				"Outline2" = @{ __idfunc = { $args[1].ID }
					"Germany" = @{ __idfunc = { $args[0].ID }
						"Brandenburg" = @{}
						"Sachsen" = @{}
					}
				}
			}
			"Page2" = @{}
			"Wikipedia 2020-01-02" = @{}
		}
		"Section2" = @{}
	}
}

function GetPath() {
	# echo "GP: $($args.Count)"
	$path = [System.Text.StringBuilder]::new("ON:")
	$current = $PathElements
	foreach ($elementName in $args) {

		$current = $current[$elementName]

		if ($current.ContainsKey('__id')) {
			[void]($path.Append('/').Append($current.__id))
		} elseif ($current.ContainsKey('__idfunc')) {
			$current.__id = [string]$current.__idfunc.Invoke((Get-ChildItem ($path.ToString() + '/')))
			# Write-Host "GP: $($current.__id.GetType()), $($current.__id)"
			$current.Remove('__idfunc')
			[void]($path.Append('/').Append($current.__id))
		} else {
			foreach ($child in (Get-ChildItem ($path.ToString() + '/'))) {
				if ($child.Name -eq $elementName) {
					[void]($path.Append('/').Append($child.ID))
					$current.__id = $child.ID
					break
				}
			}
		}
	}
	#Write-Host "GPret: $($path.ToString())"
	return $path.ToString()
}

Set-Alias p GetPath

[TestClass()]
class DriveTests {

	[TestClassSetup()]
	[void] Setup() {
		# ensure that UT Notebook is open
		Open-ONHierarchy -Path "$PSScriptRoot\SidenoteTest"
	}

	[TestMethod()]
	[void] Drive_Exists() {
		$driveInfo = Get-PSDrive "ON"
		Test (ExpectNotNull) $driveInfo
		Test 'ON:\' $driveInfo.Root
	}

	[TestMethod()]
	[void] GetChildItem_Drive_YieldsExpectedNotebook() {
		$notebooks = Get-ChildItem 'ON:\'
		Test (ExpectAnd (ExpectCountGreaterOrEqual 1) (ExpectContains @{Name = 'SidenoteTest'})) $notebooks
	}

	[TestMethod()]
	[void] GetChildItem_Notebook_YieldsExpectedSections() {
		$sections = Get-ChildItem (p 'SidenoteTest')
		Test `
			(ExpectAnd `
				(ExpectCountGreaterOrEqual 2) `
				(ExpectContains @{Name = "Section1"}) `
				(ExpectContains @{Name = "UTData"})) `
			$sections
	}

	[TestMethod()]
	[void] GetChildItem_Section_YieldsExpectedPages() {
		$pages = Get-ChildItem (p 'SidenoteTest' 'UTData')
		Test (ExpectAnd (ExpectCountGreaterOrEqual 2) (ExpectContains @{Name = "Page1"}) (ExpectContains @{Name = "Page2"})) $pages
	}

	[TestMethod()]
	[void] GetChildItem_Page_YieldsExpectedOutlines() {
		$outlines = Get-ChildItem (p 'SidenoteTest' 'UTData' 'Page1')

		Test `
			(ExpectAnd `
				(ExpectCountGreaterOrEqual 2) `
				(Expect 'First Outline' { $args[0].Children[0].Text -eq 'USA' }) `
				(Expect 'Second Outline' { $args[1].Children[0].Text -eq 'Germany' }) `
			) `
			$outlines
	}

	[TestMethod()]
	[void] GetChildItem_Outline_YieldsExpectedOutlineElements() {
		$outlineElements = Get-ChildItem (p 'SidenoteTest' 'UTData' 'Page1' 'Outline1')
		Test `
			(ExpectAnd `
				(ExpectCountGreaterOrEqual 1) `
				(ExpectContains @{ Text = "USA" })) `
			$outlineElements
	}

	[TestMethod()]
	[void] GetChildItem_OutlineRecurse_YieldsExpectedOutlineElements() {
		$outlineElements = Get-ChildItem -Recurse (p 'SidenoteTest' 'UTData' 'Page1' 'Outline1')
		Test @(
				@{Text = "USA"},
				@{Text = "Oregon"},
				@{Text = "Washington"},
				@{Text = "Columbia County"},
				@{Text = "Multnomah County"},
				@{Text = "King County"},
				@{Text = "Snohomish County"},
				@{Text = "Clatskanie"},
				@{Text = "Prescott"},
				@{Text = "Portland"},
				@{Text = "Troutdale"},
				@{Text = "Bellevue"},
				@{Text = "Issaquah"},
				@{Text = "Seattle"},
				@{Text = "Arlington"},
				@{Text = "Edmonds"},
				@{Text = "Everett"}
			) `
			$outlineElements
	}

	[TestMethod()]
	[void] SetLocation_AbsolutePathToRoot() {
		Push-Location
		try {
			Set-Location 'ON:\'
			$notebooks = Get-ChildItem
			Test (ExpectAnd (ExpectCountGreaterOrEqual 1) (ExpectContains @{Name = 'SidenoteTest'})) $notebooks
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_AbsolutePathToNotebook() {
		Push-Location
		try {
			Set-Location (p 'SidenoteTest')
			$sections = Get-ChildItem
			Test `
				(ExpectAnd `
					(ExpectCountGreaterOrEqual 2) `
					(ExpectContains @{Name = "Section1"}) `
					(ExpectContains @{Name = "UTData"})) `
				$sections
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_AbsolutePathToSection() {
		Push-Location
		try {
			Set-Location (p 'SidenoteTest' 'UTData')
			$pages = Get-ChildItem
			Test (ExpectAnd (ExpectCountGreaterOrEqual 2) (ExpectContains @{Name = "Page1"}) (ExpectContains @{Name = "Page2"})) $pages
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_AbsolutePathToPage() {
		Push-Location
		try {
			Set-Location (p 'SidenoteTest' 'UTData' 'Page1')
			$outlines = Get-ChildItem
			Test `
				(ExpectAnd `
					(ExpectCountGreaterOrEqual 2) `
					(Expect 'First Outline' { $args[0].Children[0].Text -eq 'USA' }) `
					(Expect 'Second Outline' { $args[1].Children[0].Text -eq 'Germany' }) `
				) `
				$outlines
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_AbsolutePathToOutline() {
		Push-Location
		try {
			Set-Location (p 'SidenoteTest' 'UTData' 'Page1' 'Outline1')
			Test (ExpectContains @{Text = "USA"}) (Get-ChildItem)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_FromRootToNotebook() {
		Push-Location
		try {
			Set-Location 'ON:\'
			Set-Location (PathGetRelative 'ON:\' (p 'SidenoteTest'))
			$sections = Get-ChildItem
			Test `
				(ExpectAnd `
					(ExpectCountGreaterOrEqual 2) `
					(ExpectContains @{Name = "Section1"}) `
					(ExpectContains @{Name = "UTData"})) `
				$sections
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_FromRootToSection() {
		Push-Location
		try {
			Set-Location 'ON:\'
			Set-Location (PathGetRelative 'ON:\' (p 'SidenoteTest' 'UTData'))
			Test (ExpectAnd (ExpectContains @{Name = 'Page1'}) (ExpectContains @{Name = 'Page2'})) (Get-ChildItem)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_FromRootToPage() {
		Push-Location
		try {
			Set-Location 'ON:\'
			Set-Location (PathGetRelative 'ON:\ '(p 'SidenoteTest' 'UTData' 'Page1'))
			Test `
				(ExpectAnd `
					(Expect 'First Outline' { $args[0].Children[0].Text -eq 'USA' }) `
					(Expect 'Second Outline' { $args[1].Children[0].Text -eq 'Germany' }) `
				) `
				(Get-ChildItem)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_FromRootToOutline() {
		Push-Location
		try {
			Set-Location 'ON:\'
			Set-Location (PathGetRelative 'ON:\' (p 'SidenoteTest' 'UTData' 'Page1' 'Outline1'))
			Test (ExpectContains @{Text = "USA"}) (Get-ChildItem)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_FromRootToOE() {
		Push-Location
		try {
			Set-Location 'ON:\'
			Set-Location (PathGetRelative 'ON:\' (p 'SidenoteTest' 'UTData' 'Page1' 'Outline1' 'USA'))
			Test `
				(ExpectAnd `
					(ExpectContains @{Text = "Oregon"}) `
					(ExpectContains @{Text = "Washington"})) `
				(Get-ChildItem)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_FromNotebookToRoot() {
		Push-Location
		try {
			Set-Location (p 'SidenoteTest')
			Set-Location '..'
			$notebooks = Get-ChildItem
			Test (ExpectAnd (ExpectCountGreaterOrEqual 1) (ExpectContains @{Name = 'SidenoteTest'})) $notebooks
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_FromNotebookToSection() {
		Push-Location
		try {
			Set-Location (p 'SidenoteTest')
			Set-Location (PathGetRelative (p 'SidenoteTest') (p 'SidenoteTest' 'UTData'))
			$pages = Get-ChildItem
			Test (ExpectAnd (ExpectCountGreaterOrEqual 2) (ExpectContains @{Name = "Page1"}) (ExpectContains @{Name = "Page2"})) $pages
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_FromNotebookToPage() {
		Push-Location
		try {
			Set-Location (p 'SidenoteTest')
			Set-Location (PathGetRelative (p 'SidenoteTest') (p 'SidenoteTest' 'UTData' 'Page1'))
			Test `
				(ExpectAnd `
					(Expect 'First Outline' { $args[0].Children[0].Text -eq 'USA' }) `
					(Expect 'Second Outline' { $args[1].Children[0].Text -eq 'Germany' }) `
				) `
				(Get-ChildItem)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_FromNotebookToOutline() {
		Push-Location
		try {
			Set-Location (p 'SidenoteTest')
			Set-Location (PathGetRelative (p 'SidenoteTest') (p 'SidenoteTest' 'UTData' 'Page1' 'Outline1'))
			Test (ExpectContains @{Text = "USA"}) (Get-ChildItem)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_FromNotebookToOE() {
		Push-Location
		try {
			Set-Location (p 'SidenoteTest')
			Set-Location (PathGetRelative (p 'SidenoteTest') (p 'SidenoteTest' 'UTData' 'Page1' 'Outline1' 'USA'))
			Test `
				(ExpectAnd `
					(ExpectContains @{Text = "Oregon"}) `
					(ExpectContains @{Text = "Washington"})) `
				(Get-ChildItem)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_FromSectionToRoot() {
		Push-Location
		try {
			Set-Location (p 'SidenoteTest' 'UTData')
			Set-Location '..\..'
			$notebooks = Get-ChildItem
			Test (ExpectAnd (ExpectCountGreaterOrEqual 1) (ExpectContains @{Name = 'SidenoteTest'})) $notebooks
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_FromSectionToNotebook() {
		Push-Location
		try {
			Set-Location (p 'SidenoteTest' 'UTData')
			Set-Location '..'
			$sections = Get-ChildItem
			Test `
				(ExpectAnd `
					(ExpectCountGreaterOrEqual 2) `
					(ExpectContains @{Name = "Section1"}) `
					(ExpectContains @{Name = "UTData"})) `
				$sections
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_FromSectionToPage() {
		Push-Location
		try {
			Set-Location (p 'SidenoteTest' 'UTData')
			Set-Location (PathGetRelative (p 'SidenoteTest' 'UTData') (p 'SidenoteTest' 'UTData' 'Page1'))
			$outlines = Get-ChildItem
			Test `
				(ExpectAnd `
					(ExpectCountGreaterOrEqual 2) `
					(Expect 'First Outline' { $args[0].Children[0].Text -eq 'USA' }) `
					(Expect 'Second Outline' { $args[1].Children[0].Text -eq 'Germany' }) `
				) `
				$outlines
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_FromSectionToOutline() {
		Push-Location
		try {
			Set-Location (p 'SidenoteTest' 'UTData')
			Set-Location (PathGetRelative (p 'SidenoteTest' 'UTData') (p 'SidenoteTest' 'UTData' 'Page1' 'Outline1'))
			Test (ExpectContains @{Text = "USA"}) (Get-ChildItem)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_FromSectionToOE() {
		Push-Location
		try {
			Set-Location (p 'SidenoteTest' 'UTData')
			Set-Location (PathGetRelative (p 'SidenoteTest' 'UTData') (p 'SidenoteTest' 'UTData' 'Page1' 'Outline1' 'USA'))
			Test `
				(ExpectAnd `
					(ExpectContains @{Text = "Oregon"}) `
					(ExpectContains @{Text = 'Washington'})) `
				(Get-ChildItem)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_FromPageToRoot() {
		Push-Location
		try {
			Set-Location (p 'SidenoteTest' 'UTData' 'Page1')
			Set-Location '..\..\..'
			Test (ExpectContains @{Name = "SidenoteTest"}) (Get-ChildItem)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_FromPageToNotebook() {
		Push-Location
		try {
			Set-Location (p 'SidenoteTest' 'UTData' 'Page1')
			Set-Location '..\..'
			$sections = Get-ChildItem
			Test `
				(ExpectAnd `
					(ExpectCountGreaterOrEqual 2) `
					(ExpectContains @{Name = "Section1"}) `
					(ExpectContains @{Name = "UTData"})) `
				$sections
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_FromPageToSection() {
		Push-Location
		try {
			Set-Location (p 'SidenoteTest' 'UTData' 'Page1')
			Set-Location '..'
			$pages = Get-ChildItem
			Test (ExpectAnd (ExpectCountGreaterOrEqual 2) (ExpectContains @{Name = "Page1"}) (ExpectContains @{Name = "Page2"})) $pages
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_FromPageToOutline() {
		Push-Location
		try {
			Set-Location (p 'SidenoteTest' 'UTData' 'Page1')
			Set-Location (PathGetRelative (p 'SidenoteTest' 'UTData' 'Page1') (p 'SidenoteTest' 'UTData' 'Page1' 'Outline1'))
			Test (ExpectContains @{Text = 'USA'}) (Get-ChildItem)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_FromOutlineToSection() {
		Push-Location
		try {
			Set-Location (p 'SidenoteTest' 'UTData' 'Page1' 'Outline1')
			Set-Location '..\..'
			$pages = Get-ChildItem
			Test (ExpectAnd (ExpectCountGreaterOrEqual 2) (ExpectContains @{Name = "Page1"}) (ExpectContains @{Name = "Page2"})) $pages
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_FromOutlineToPage() {
		Push-Location
		try {
			Set-Location (p 'SidenoteTest' 'UTData' 'Page1' 'Outline1')
			Set-Location '..'
			$outlines = Get-ChildItem
			Test `
				(ExpectAnd `
					(ExpectCountGreaterOrEqual 2) `
					(Expect 'First Outline' { $args[0].Children[0].Text -eq 'USA' }) `
					(Expect 'Second Outline' { $args[1].Children[0].Text -eq 'Germany' }) `
				) `
				$outlines
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_FromOutlineToOE() {
		Push-Location
		try {
			Set-Location (p 'SidenoteTest' 'UTData' 'Page1' 'Outline1')
			Set-Location (PathGetRelative (p 'SidenoteTest' 'UTData' 'Page1' 'Outline1') (p 'SidenoteTest' 'UTData' 'Page1' 'Outline1' 'USA'))
			Test `
				(ExpectAnd `
					(ExpectContains @{Text = 'Oregon'}) `
					(ExpectContains @{Text = 'Washington'})) `
				(Get-ChildItem)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_FromOEToPage() {
		Push-Location
		try {
			Set-Location (p 'SidenoteTest' 'UTData' 'Page1' 'Outline1' 'USA')
			Set-Location '..\..'
			$outlines = Get-ChildItem
			Test `
				(ExpectAnd `
					(ExpectCountGreaterOrEqual 2) `
					(Expect 'First Outline' { $args[0].Children[0].Text -eq 'USA' }) `
					(Expect 'Second Outline' { $args[1].Children[0].Text -eq 'Germany' }) `
				) `
				$outlines
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_FromOEToOutline() {
		Push-Location
		try {
			Set-Location (p 'SidenoteTest' 'UTData' 'Page1' 'Outline1' 'USA')
			Set-Location '..'
			Test (ExpectContains @{Text = "USA"}) (Get-ChildItem)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_FromOEToOEInOtherOutline() {
		Push-Location
		try {
			Set-Location (p 'SidenoteTest' 'UTData' 'Page1' 'Outline1' 'USA')
			Set-Location (PathGetRelative (p 'SidenoteTest' 'UTData' 'Page1' 'Outline1' 'USA') (p 'SidenoteTest' 'UTData' 'Page1' 'Outline2' 'Germany'))
			$outlineElements = Get-ChildItem
			Test `
				(ExpectAnd `
					(ExpectContains @{Text = "Brandenburg"}) `
					(ExpectContains @{Text = "Sachsen"})) `
				$outlineElements
		} finally {
			Pop-Location
		}
	}
}

$standaloneLogFilePath = "$env:TEMP\$(PathFileBaseName $MyInvocation.MyCommand.Path).log"
RunTests $standaloneLogFilePath ([DriveTests])
