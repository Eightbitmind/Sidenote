using module Path
using module TestUtils

[TestClass()]
class DriveTests {
	[TestMethod()]
	[void] Drive_Exists() {
		$driveInfo = Get-PSDrive "ON"
		Test (ExpectNotNull) $driveInfo
		Test 'ON:\' $driveInfo.Root
	}

	[TestMethod()]
	[void] Drive_ContainsNotebook() {
		$notebooks = Get-ChildItem 'ON:\'
		Test (ExpectAnd (ExpectCountGreaterOrEqual 1) (ExpectContains @{Name = 'SidenoteTest'})) $notebooks
	}

	[TestMethod()]
	[void] Drive_NotebookContainsSections() {
		$sidenoteTest = 'ON:\{84247725-824B-42F7-B86D-3971948BAA47}{1}{B0}'
		$sections = Get-ChildItem $sidenoteTest
		[DriveTests]::TestNotebookViaSections($sections)
	}

	[TestMethod()]
	[void] Drive_SectionContainsPages() {
		$sidenoteTest_Section1 = 'ON:\{84247725-824B-42F7-B86D-3971948BAA47}{1}{B0}\{A8BB95B1-5BEE-4BB0-98D4-FB42485B52CB}{1}{B0}'
		$pages = Get-ChildItem $sidenoteTest_Section1
		[DriveTests]::TestSectionViaPages($pages)
	}

	[TestMethod()]
	[void] Drive_PageContainsOutlines() {
		$sidenoteTest_Section1_Page1 = 'ON:\{84247725-824B-42F7-B86D-3971948BAA47}{1}{B0}\{A8BB95B1-5BEE-4BB0-98D4-FB42485B52CB}{1}{B0}\{A8BB95B1-5BEE-4BB0-98D4-FB42485B52CB}{1}{E1953763139101400170501939065809574157669171}'
		$outlines = Get-ChildItem $sidenoteTest_Section1_Page1
		[DriveTests]::TestPageViaOutlines($outlines)
	}

	[TestMethod()]
	[void] Drive_OutlineContainsOEs() {
		$sidenoteTest_Section1_Page1_Outline1 = 'ON:\{84247725-824B-42F7-B86D-3971948BAA47}{1}{B0}\{A8BB95B1-5BEE-4BB0-98D4-FB42485B52CB}{1}{B0}\{A8BB95B1-5BEE-4BB0-98D4-FB42485B52CB}{1}{E1953763139101400170501939065809574157669171}\{5DA5AC0E-BF64-490A-A961-2340FF1DAD9B}{10}{B0}'
		$outlineElements = Get-ChildItem $sidenoteTest_Section1_Page1_Outline1
		[DriveTests]::TestOutlineViaOutlineElements($outlineElements)
	}

	[TestMethod()]
	[void] Drive_SetLocationToRoot() {
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
	[void] Drive_SetLocationToNotebook() {
		Push-Location
		try {
			$sidenoteTest = 'ON:\{84247725-824B-42F7-B86D-3971948BAA47}{1}{B0}'
			Set-Location $sidenoteTest
			$sections = Get-ChildItem
			[DriveTests]::TestNotebookViaSections($sections)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] Drive_SetLocationToSection() {
		Push-Location
		try {
			$sidenoteTest_Section1 = 'ON:\{84247725-824B-42F7-B86D-3971948BAA47}{1}{B0}\{A8BB95B1-5BEE-4BB0-98D4-FB42485B52CB}{1}{B0}'
			Set-Location $sidenoteTest_Section1
			$pages = Get-ChildItem
			[DriveTests]::TestSectionViaPages($pages)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] Drive_SetLocationToPage() {
		Push-Location
		try {
			$sidenoteTest_Section1_Page1 = 'ON:\{84247725-824B-42F7-B86D-3971948BAA47}{1}{B0}\{A8BB95B1-5BEE-4BB0-98D4-FB42485B52CB}{1}{B0}\{A8BB95B1-5BEE-4BB0-98D4-FB42485B52CB}{1}{E1953763139101400170501939065809574157669171}'
			Set-Location $sidenoteTest_Section1_Page1
			$outlines = Get-ChildItem
			[DriveTests]::TestPageViaOutlines($outlines)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] Drive_SetLocationToOutline() {
		Push-Location
		try {
			$sidenoteTest_Section1_Page1_Outline1 = 'ON:\{84247725-824B-42F7-B86D-3971948BAA47}{1}{B0}\{A8BB95B1-5BEE-4BB0-98D4-FB42485B52CB}{1}{B0}\{A8BB95B1-5BEE-4BB0-98D4-FB42485B52CB}{1}{E1953763139101400170501939065809574157669171}\{5DA5AC0E-BF64-490A-A961-2340FF1DAD9B}{10}{B0}'
			Set-Location $sidenoteTest_Section1_Page1_Outline1
			$outlineElements = Get-ChildItem
			[DriveTests]::TestOutlineViaOutlineElements($outlineElements)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] Drive_GoUpFromNotebook() {
		Push-Location
		try {
			$sidenoteTest = 'ON:\{84247725-824B-42F7-B86D-3971948BAA47}{1}{B0}'
			Set-Location $sidenoteTest
			Set-Location '..'
			$notebooks = Get-ChildItem
			Test (ExpectAnd (ExpectCountGreaterOrEqual 1) (ExpectContains @{Name = 'SidenoteTest'})) $notebooks
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] Drive_GoUpFromSection() {
		Push-Location
		try {
			$sidenoteTest_Section1 = 'ON:\{84247725-824B-42F7-B86D-3971948BAA47}{1}{B0}\{A8BB95B1-5BEE-4BB0-98D4-FB42485B52CB}{1}{B0}'
			Set-Location $sidenoteTest_Section1
			Set-Location '..'
			$sections = Get-ChildItem
			[DriveTests]::TestNotebookViaSections($sections)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] Drive_GoUpFromPage() {
		Push-Location
		try {
			$sidenoteTest_Section1_Page1 = 'ON:\{84247725-824B-42F7-B86D-3971948BAA47}{1}{B0}\{A8BB95B1-5BEE-4BB0-98D4-FB42485B52CB}{1}{B0}\{A8BB95B1-5BEE-4BB0-98D4-FB42485B52CB}{1}{E1953763139101400170501939065809574157669171}'
			Set-Location $sidenoteTest_Section1_Page1
			Set-Location '..'
			$pages = Get-ChildItem
			[DriveTests]::TestSectionViaPages($pages)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] Drive_GoUpFromOutline() {
		Push-Location
		try {
			$sidenoteTest_Section1_Page1_Outline1 = 'ON:\{84247725-824B-42F7-B86D-3971948BAA47}{1}{B0}\{A8BB95B1-5BEE-4BB0-98D4-FB42485B52CB}{1}{B0}\{A8BB95B1-5BEE-4BB0-98D4-FB42485B52CB}{1}{E1953763139101400170501939065809574157669171}\{5DA5AC0E-BF64-490A-A961-2340FF1DAD9B}{10}{B0}'
			Set-Location $sidenoteTest_Section1_Page1_Outline1
			Set-Location '..'
			$outlines = Get-ChildItem
			[DriveTests]::TestPageViaOutlines($outlines)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] Drive_GoUpFromOE() {
		Push-Location
		try {
			$sidenoteTest_Section1_Page1_Outline1_Mammals = 'ON:\{84247725-824B-42F7-B86D-3971948BAA47}{1}{B0}\{A8BB95B1-5BEE-4BB0-98D4-FB42485B52CB}{1}{B0}\{A8BB95B1-5BEE-4BB0-98D4-FB42485B52CB}{1}{E1953763139101400170501939065809574157669171}\{5DA5AC0E-BF64-490A-A961-2340FF1DAD9B}{10}{B0}\{5DA5AC0E-BF64-490A-A961-2340FF1DAD9B}{20}{B0}'
			Set-Location $sidenoteTest_Section1_Page1_Outline1_Mammals
			Set-Location '..'
			$outlineElements = Get-ChildItem
			[DriveTests]::TestOutlineViaOutlineElements($outlineElements)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] Drive_SetLocationToNotebookWithRelativePath() {
		Push-Location
		try {
			Set-Location 'ON:\'
			$sidenoteTestRel = '{84247725-824B-42F7-B86D-3971948BAA47}{1}{B0}'
			Set-Location $sidenoteTestRel
			$sections = Get-ChildItem
			[DriveTests]::TestNotebookViaSections($sections)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] Drive_SetLocationToSectionWithRelativePath() {
		Push-Location
		try {
			$sidenoteTestAbs = 'ON:\{84247725-824B-42F7-B86D-3971948BAA47}{1}{B0}'
			Set-Location $sidenoteTestAbs
			$section1Rel = '{A8BB95B1-5BEE-4BB0-98D4-FB42485B52CB}{1}{B0}'
			Set-Location $section1Rel
			$pages = Get-ChildItem
			[DriveTests]::TestSectionViaPages($pages)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] Drive_SetLocationToPageWithRelativePath() {
		Push-Location
		try {
			$sidenoteTest_Section1_Abs = 'ON:\{84247725-824B-42F7-B86D-3971948BAA47}{1}{B0}\{A8BB95B1-5BEE-4BB0-98D4-FB42485B52CB}{1}{B0}'
			Set-Location $sidenoteTest_Section1_Abs
			$page1_Rel = '{A8BB95B1-5BEE-4BB0-98D4-FB42485B52CB}{1}{E1953763139101400170501939065809574157669171}'
			Set-Location $page1_Rel
			$outlines = Get-ChildItem
			[DriveTests]::TestPageViaOutlines($outlines)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] Drive_SetLocationToOutlineWithRelativePath() {
		Push-Location
		try {
			$sidenoteTest_Section1_Page1_Abs = 'ON:\{84247725-824B-42F7-B86D-3971948BAA47}{1}{B0}\{A8BB95B1-5BEE-4BB0-98D4-FB42485B52CB}{1}{B0}\{A8BB95B1-5BEE-4BB0-98D4-FB42485B52CB}{1}{E1953763139101400170501939065809574157669171}'
			Set-Location $sidenoteTest_Section1_Page1_Abs
			$outline1_Rel = '{5DA5AC0E-BF64-490A-A961-2340FF1DAD9B}{10}{B0}'
			Set-Location $outline1_Rel
			$outlineElements = Get-ChildItem
			[DriveTests]::TestOutlineViaOutlineElements($outlineElements)
		} finally {
			Pop-Location
		}
	}

	[TestMethod()]
	[void] Drive_SetLocationToOutlineElementRelativePath() {
		Push-Location
		try {
			$sidenoteTest_Section1_Page1_Outline1_Abs = 'ON:\{84247725-824B-42F7-B86D-3971948BAA47}{1}{B0}\{A8BB95B1-5BEE-4BB0-98D4-FB42485B52CB}{1}{B0}\{A8BB95B1-5BEE-4BB0-98D4-FB42485B52CB}{1}{E1953763139101400170501939065809574157669171}\{5DA5AC0E-BF64-490A-A961-2340FF1DAD9B}{10}{B0}'
			Set-Location $sidenoteTest_Section1_Page1_Outline1_Abs
			$outlineElement1_Rel = '{5DA5AC0E-BF64-490A-A961-2340FF1DAD9B}{20}{B0}'
			Set-Location $outlineElement1_Rel
			# [DriveTests]::TestOutlineViaOutlineElements($outlineElements)

			Test @{Text = 'Mammals'} (Get-Item .)

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
				(ExpectContains @{ID = "{5DA5AC0E-BF64-490A-A961-2340FF1DAD9B}{10}{B0}"; Author="Andreas Eulitz"; AuthorInitials="AE"}) `
				(ExpectContains @{ID = "{5DA5AC0E-BF64-490A-A961-2340FF1DAD9B}{14}{B0}"; Author="Andreas Eulitz"; AuthorInitials="AE"})) `
			$outlines
	}

	static hidden [void] TestOutlineViaOutlineElements($outlineElements) {
		Test `
			(ExpectAnd `
				(ExpectCountGreaterOrEqual 2) `
				(ExpectContains @{ID = "{5DA5AC0E-BF64-490A-A961-2340FF1DAD9B}{11}{B0}"; Text = "Outline1"}) `
				(ExpectContains @{ID = "{5DA5AC0E-BF64-490A-A961-2340FF1DAD9B}{20}{B0}"; Text = "Mammals"})) `
			$outlineElements
	}
}

$standaloneLogFilePath = "$env:TEMP\$(PathFileBaseName $MyInvocation.MyCommand.Path).log"
RunTests $standaloneLogFilePath ([DriveTests])
