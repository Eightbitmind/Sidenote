using module Gumby.Path
using module Gumby.Test

using module Sidenote

# Shorthands for the GUID-based path elements
$PathElements = @{
	drive                      = 'ON:'
	  SidenoteTest             = "{84247725-824B-42F7-B86D-3971948BAA47}{1}{B0}"
	    UTData                 = "{E8BD8F63-7C83-4A55-B8A3-45E701727063}{1}{B0}"
	      Page1                = "{E8BD8F63-7C83-4A55-B8A3-45E701727063}{1}{E19473654813312029312920169532295347629710431}"
	        Outline1           = "{DB124992-BF07-4883-B8F2-A914F742DCBB}{15}{B0}"
	          USA              = "{DB124992-BF07-4883-B8F2-A914F742DCBB}{16}{B0}"
	            Oregon         = "{DB124992-BF07-4883-B8F2-A914F742DCBB}{18}{B0}"
	            Washington     = "{DB124992-BF07-4883-B8F2-A914F742DCBB}{41}{B0}"
	        Outline2           = "{DB124992-BF07-4883-B8F2-A914F742DCBB}{69}{B0}"
	          Germany          = "{DB124992-BF07-4883-B8F2-A914F742DCBB}{70}{B0}"
	            Brandenburg    = "{DB124992-BF07-4883-B8F2-A914F742DCBB}{71}{B0}"
	            Sachsen        = "{DB124992-BF07-4883-B8F2-A914F742DCBB}{92}{B0}"
	      Page2                = "{E8BD8F63-7C83-4A55-B8A3-45E701727063}{1}{E1951502003092150449381989883320034793440611}"
	      Wikipedia_2020_01_02 = "{E8BD8F63-7C83-4A55-B8A3-45E701727063}{1}{E1948152843360785065021997889921752203513891}"
	    Section2               = "{1D9410FF-2441-4C34-9919-88C047E9A38A}{1}{B0}"
}

function GetPath() {
	$sb = [System.Text.StringBuilder]::new()
	foreach ($elementName in $args) {
		[void]($sb.Append("\").Append($PathElements[$elementName]))
	}
	return $sb.ToString().Substring(1)
}

function GetPathElement($name) {
	return $PathElements[$name]
}

Set-Alias p GetPath

[TestClass()]
class DriveTests {
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
		$sections = Get-ChildItem (p 'drive' 'SidenoteTest')
		[DriveTests]::TestNotebookViaSections($sections)
	}

	[TestMethod()]
	[void] GetChildItem_Section_YieldsExpectedPages() {
		$pages = Get-ChildItem (p 'drive' 'SidenoteTest' 'UTData')
		[DriveTests]::TestSectionViaPages($pages)
	}

	[TestMethod()]
	[void] GetChildItem_Page_YieldsExpectedOutlines() {
		$outlines = Get-ChildItem (p 'drive' 'SidenoteTest' 'UTData' 'Page1')
		[DriveTests]::TestPageViaOutlines($outlines)
	}

	[TestMethod()]
	[void] GetChildItem_Outline_YieldsExpectedOutlineElements() {
		$outlineElements = Get-ChildItem (p 'drive' 'SidenoteTest' 'UTData' 'Page1' 'Outline1')
		Test `
			(ExpectAnd `
				(ExpectCountGreaterOrEqual 1) `
				(ExpectContains @{ID = (GetPathElement "USA"); Text = "USA"})) `
			$outlineElements
	}

	[TestMethod()]
	[void] GetChildItem_OutlineRecurse_YieldsExpectedOutlineElements() {
		$outlineElements = Get-ChildItem -Recurse (p 'drive' 'SidenoteTest' 'UTData' 'Page1' 'Outline1')
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
			Set-Location (p 'drive' 'SidenoteTest')
			$sections = Get-ChildItem
			[DriveTests]::TestNotebookViaSections($sections)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_AbsolutePathToSection() {
		Push-Location
		try {
			Set-Location (p 'drive' 'SidenoteTest' 'UTData')
			$pages = Get-ChildItem
			[DriveTests]::TestSectionViaPages($pages)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_AbsolutePathToPage() {
		Push-Location
		try {
			Set-Location (p 'drive' 'SidenoteTest' 'UTData' 'Page1')
			$outlines = Get-ChildItem
			[DriveTests]::TestPageViaOutlines($outlines)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_AbsolutePathToOutline() {
		Push-Location
		try {
			Set-Location (p 'drive' 'SidenoteTest' 'UTData' 'Page1' 'Outline1')
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
			Set-Location (p 'SidenoteTest')
			$sections = Get-ChildItem
			[DriveTests]::TestNotebookViaSections($sections)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_FromRootToSection() {
		Push-Location
		try {
			Set-Location 'ON:\'
			Set-Location (p 'SidenoteTest' 'UTData')
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
			Set-Location (p 'SidenoteTest' 'UTData' 'Page1')
			Test `
				(ExpectAnd `
					(ExpectContains @{ID = (GetPathElement 'Outline1')}) `
					(ExpectContains @{ID = (GetPathElement 'Outline2')})) `
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
			Set-Location (p 'SidenoteTest' 'UTData' 'Page1' 'Outline1')
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
			Set-Location (p 'SidenoteTest' 'UTData' 'Page1' 'Outline1' 'USA')
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
			Set-Location (p 'drive' 'SidenoteTest')
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
			Set-Location (p 'drive' 'SidenoteTest')
			Set-Location (p 'UTData')
			$pages = Get-ChildItem
			[DriveTests]::TestSectionViaPages($pages)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_FromNotebookToPage() {
		Push-Location
		try {
			Set-Location (p 'drive' 'SidenoteTest')
			Set-Location (p 'UTData' 'Page1')
			Test `
				(ExpectAnd `
					(ExpectContains @{ID = (GetPathElement 'Outline1')}) `
					(ExpectContains @{ID = (GetPathElement 'Outline2')})) `
				(Get-ChildItem)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_FromNotebookToOutline() {
		Push-Location
		try {
			Set-Location (p 'drive' 'SidenoteTest')
			Set-Location (p 'UTData' 'Page1' 'Outline1')
			Test (ExpectContains @{Text = "USA"}) (Get-ChildItem)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_FromNotebookToOE() {
		Push-Location
		try {
			Set-Location (p 'drive' 'SidenoteTest')
			Set-Location (p 'UTData' 'Page1' 'Outline1' 'USA')
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
			Set-Location (p 'drive' 'SidenoteTest' 'UTData')
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
			Set-Location (p 'drive' 'SidenoteTest' 'UTData')
			Set-Location '..'
			$sections = Get-ChildItem
			[DriveTests]::TestNotebookViaSections($sections)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_FromSectionToPage() {
		Push-Location
		try {
			Set-Location (p 'drive' 'SidenoteTest' 'UTData')
			Set-Location (p 'Page1')
			$outlines = Get-ChildItem
			[DriveTests]::TestPageViaOutlines($outlines)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_FromSectionToOutline() {
		Push-Location
		try {
			Set-Location (p 'drive' 'SidenoteTest' 'UTData')
			Set-Location (p 'Page1' 'Outline1')
			Test (ExpectContains @{Text = "USA"}) (Get-ChildItem)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_FromSectionToOE() {
		Push-Location
		try {
			Set-Location (p 'drive' 'SidenoteTest' 'UTData')
			Set-Location (p 'Page1' 'Outline1' 'USA')
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
			Set-Location (p 'drive' 'SidenoteTest' 'UTData' 'Page1')
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
			Set-Location (p 'drive' 'SidenoteTest' 'UTData' 'Page1')
			Set-Location '..\..'
			$sections = Get-ChildItem
			[DriveTests]::TestNotebookViaSections($sections)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_FromPageToSection() {
		Push-Location
		try {
			Set-Location (p 'drive' 'SidenoteTest' 'UTData' 'Page1')
			Set-Location '..'
			$pages = Get-ChildItem
			[DriveTests]::TestSectionViaPages($pages)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_FromPageToOutline() {
		Push-Location
		try {
			Set-Location (p 'drive' 'SidenoteTest' 'UTData' 'Page1')
			Set-Location (p 'Outline1')
			Test (ExpectContains @{Text = 'USA'}) (Get-ChildItem)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_FromOutlineToSection() {
		Push-Location
		try {
			Set-Location (p 'drive' 'SidenoteTest' 'UTData' 'Page1' 'Outline1')
			Set-Location '..\..'
			$pages = Get-ChildItem
			[DriveTests]::TestSectionViaPages($pages)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_FromOutlineToPage() {
		Push-Location
		try {
			Set-Location (p 'drive' 'SidenoteTest' 'UTData' 'Page1' 'Outline1')
			Set-Location '..'
			$outlines = Get-ChildItem
			[DriveTests]::TestPageViaOutlines($outlines)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_FromOutlineToOE() {
		Push-Location
		try {
			Set-Location (p 'drive' 'SidenoteTest' 'UTData' 'Page1' 'Outline1')
			Set-Location (p 'USA')
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
			Set-Location (p 'drive' 'SidenoteTest' 'UTData' 'Page1' 'Outline1' 'USA')
			Set-Location '..\..'
			$outlines = Get-ChildItem
			[DriveTests]::TestPageViaOutlines($outlines)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] SetLocation_FromOEToOutline() {
		Push-Location
		try {
			Set-Location (p 'drive' 'SidenoteTest' 'UTData' 'Page1' 'Outline1' 'USA')
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
			Set-Location (p 'drive' 'SidenoteTest' 'UTData' 'Page1' 'Outline1' 'USA')
			Set-Location ('..\..\' + (p 'Outline2' 'Germany'))
			$outlineElements = Get-ChildItem
			Test `
				(ExpectAnd `
					(ExpectContains @{Text = "Brandenburg"; ID = (GetPathElement 'Brandenburg')}) `
					(ExpectContains @{Text = "Sachsen"; ID = (GetPathElement 'Sachsen')})) `
				$outlineElements
		} finally {
			Pop-Location
		}
	}

	static hidden [void] TestNotebookViaSections($sections) {
		Test (ExpectAnd (ExpectCountGreaterOrEqual 2) (ExpectContains @{Name = "Section1"}) (ExpectContains @{Name = "Section2"})) $sections
	}

	static hidden [void] TestSectionViaPages($pages) {
		Test (ExpectAnd (ExpectCountGreaterOrEqual 2) (ExpectContains @{Name = "Page1"}) (ExpectContains @{Name = "Page2"})) $pages
	}

	static hidden [void] TestPageViaOutlines($outlines) {
		# For the time being, the OE underneath the Title element is not being exposed.
		Test `
			(ExpectAnd `
				(ExpectCountGreaterOrEqual 2) `
				(ExpectContains @{ID = (GetPathElement 'Outline1'); Author="Andreas Eulitz"; AuthorInitials="AE"}) `
				(ExpectContains @{ID = (GetPathElement 'Outline2'); Author="Andreas Eulitz"; AuthorInitials="AE"})) `
			$outlines
	}
}

$standaloneLogFilePath = "$env:TEMP\$(PathFileBaseName $MyInvocation.MyCommand.Path).log"
RunTests $standaloneLogFilePath ([DriveTests])
