using module Path
using module TestUtils

[TestClass()]
class DriveTests {
	[TestMethod()]
	[void] Drive_Exists() {
		$driveInfo = Get-PSDrive "ON"
		TestIsNotNull $driveInfo
		TestAreEqual $driveInfo.Root 'ON:\'
	}

	[TestMethod()]
	[void] Drive_ContainsNotebook() {
		$notebooks = Get-ChildItem 'ON:\'
		TestIsGreaterOrEqual $notebooks.Count 1
		$foundNotebook = $false
		foreach ($notebook in $notebooks) {
			if ($notebook.Name -eq 'SidenoteTest') {
				$foundNotebook = $true
				break
			}
		}
		TestIsTrue $foundNotebook
	}

	[TestMethod()]
	[void] Drive_NotebookContainsSections() {

		$sidenoteTest = 'ON:\{84247725-824B-42F7-B86D-3971948BAA47}{1}{B0}'
		$sections = Get-ChildItem $sidenoteTest
		TestIsGreaterOrEqual $sections.Count 2
		$foundSection1 = $false
		$foundSection2 = $false

		foreach ($section in $sections) {
			switch ($section.Name) {
				'Section1' { $foundSection1 = $true; break }
				'Section2' { $foundSection2 = $true; break }
			}
			if ($foundSection1 -and $foundSection2) { break }
		}

		TestIsTrue ($foundSection1 -and $foundSection2)
	}

	[TestMethod()]
	[void] Drive_SectionContainsPages() {

		$sidenoteTest_Section1 = 'ON:\{84247725-824B-42F7-B86D-3971948BAA47}{1}{B0}\{A8BB95B1-5BEE-4BB0-98D4-FB42485B52CB}{1}{B0}'
		$pages = Get-ChildItem $sidenoteTest_Section1
		TestIsGreaterOrEqual $pages.Count 2
		$foundPage1 = $false
		$foundPage2 = $false

		foreach ($page in $pages) {
			switch ($page.Name) {
				'Page1' { $foundPage1 = $true; break }
				'Page2' { $foundPage2 = $true; break }
			}
			if ($foundPage1 -and $foundPage2) { break }
		}

		TestIsTrue ($foundPage1 -and $foundPage2)
	}

	[TestMethod()]
	[void] Drive_PageContainsOutlines() {
		$sidenoteTest_Section1_Page1 = 'ON:\{84247725-824B-42F7-B86D-3971948BAA47}{1}{B0}\{A8BB95B1-5BEE-4BB0-98D4-FB42485B52CB}{1}{B0}\{A8BB95B1-5BEE-4BB0-98D4-FB42485B52CB}{1}{E1953763139101400170501939065809574157669171}'
		$outlines = Get-ChildItem $sidenoteTest_Section1_Page1

		# For the time being, the OE underneath the Title element is not being exposed.
		TestIsGreaterOrEqual $outlines.Count 2

		$foundOutline1 = $false
		$foundOutline2 = $false

		foreach ($outline in $outlines) {
			switch ($outline.ID) {
				'{5DA5AC0E-BF64-490A-A961-2340FF1DAD9B}{10}{B0}' {
					TestAreEqual $outline.Author "Andreas Eulitz"
					TestAreEqual $outline.AuthorInitials "AE"
					$foundOutline1 = $true
					break
				}
				'{5DA5AC0E-BF64-490A-A961-2340FF1DAD9B}{14}{B0}' {
					TestAreEqual $outline.Author "Andreas Eulitz"
					TestAreEqual $outline.AuthorInitials "AE"
					$foundOutline2 = $true
					break
				}
			}
			if ($foundOutline1 -and $foundOutline2) { break }
		}

		TestIsTrue ($foundOutline1 -and $foundOutline2)
	}

	[TestMethod()]
	[void] Drive_OutlineContainsOEs() {
		$sidenoteTest_Section1_Page1_Outline1 = 'ON:\{84247725-824B-42F7-B86D-3971948BAA47}{1}{B0}\{A8BB95B1-5BEE-4BB0-98D4-FB42485B52CB}{1}{B0}\{A8BB95B1-5BEE-4BB0-98D4-FB42485B52CB}{1}{E1953763139101400170501939065809574157669171}\{5DA5AC0E-BF64-490A-A961-2340FF1DAD9B}{10}{B0}'
		$outlineElements = Get-ChildItem $sidenoteTest_Section1_Page1_Outline1

		TestIsGreaterOrEqual $outlineElements.Count 2

		$foundOE1 = $false
		$foundOE2 = $false

		foreach ($outlineElement in $outlineElements) {

			switch ($outlineElement.ID) {
				'{5DA5AC0E-BF64-490A-A961-2340FF1DAD9B}{11}{B0}' {
					TestAreEqual $outlineElement.Text "Outline1"
					$foundOE1 = $true
					break
				}

				'{5DA5AC0E-BF64-490A-A961-2340FF1DAD9B}{20}{B0}' {
					TestAreEqual $outlineElement.Text "Mammals"
					$foundOE2 = $true
					break
				}
			}

			if ($foundOE1 -and $foundOE2) { break }
		}

		TestIsTrue ($foundOE1 -and $foundOE2)
	}

	[TestMethod()]
	[void] Drive_SetLocationToRoot() {
		Push-Location
		try {
			Set-Location 'ON:\'
			$notebooks = Get-ChildItem

			TestIsGreaterOrEqual $notebooks.Count 1
			$foundNotebook = $false
			foreach ($notebook in $notebooks) {
				if ($notebook.Name -eq 'SidenoteTest') {
					$foundNotebook = $true
					break
				}
			}
			TestIsTrue $foundNotebook

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
			TestIsGreaterOrEqual $sections.Count 2
			$foundSection1 = $false
			$foundSection2 = $false
	
			foreach ($section in $sections) {
				switch ($section.Name) {
					'Section1' { $foundSection1 = $true; break }
					'Section2' { $foundSection2 = $true; break }
				}
				if ($foundSection1 -and $foundSection2) { break }
			}
	
			TestIsTrue ($foundSection1 -and $foundSection2)

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
			TestIsGreaterOrEqual $pages.Count 2
			$foundPage1 = $false
			$foundPage2 = $false

			foreach ($page in $pages) {
				switch ($page.Name) {
					'Page1' { $foundPage1 = $true; break }
					'Page2' { $foundPage2 = $true; break }
				}
				if ($foundPage1 -and $foundPage2) { break }
			}

			TestIsTrue ($foundPage1 -and $foundPage2)
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

			# For the time being, the OE underneath the Title element is not being exposed.
			TestIsGreaterOrEqual $outlines.Count 2

			$foundOutline1 = $false
			$foundOutline2 = $false

			foreach ($outline in $outlines) {
				switch ($outline.ID) {
					'{5DA5AC0E-BF64-490A-A961-2340FF1DAD9B}{10}{B0}' {
						TestAreEqual $outline.Author "Andreas Eulitz"
						TestAreEqual $outline.AuthorInitials "AE"
						$foundOutline1 = $true
						break
					}
					'{5DA5AC0E-BF64-490A-A961-2340FF1DAD9B}{14}{B0}' {
						TestAreEqual $outline.Author "Andreas Eulitz"
						TestAreEqual $outline.AuthorInitials "AE"
						$foundOutline2 = $true
						break
					}
				}
				if ($foundOutline1 -and $foundOutline2) { break }
			}

			TestIsTrue ($foundOutline1 -and $foundOutline2)
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
			TestIsGreaterOrEqual $outlineElements.Count 2
			$foundOE1 = $false
			$foundOE2 = $false

			foreach ($outlineElement in $outlineElements) {
				switch ($outlineElement.ID) {
					'{5DA5AC0E-BF64-490A-A961-2340FF1DAD9B}{11}{B0}' {
						TestAreEqual $outlineElement.Text "Outline1"
						$foundOE1 = $true
						break
					}
					'{5DA5AC0E-BF64-490A-A961-2340FF1DAD9B}{20}{B0}' {
						TestAreEqual $outlineElement.Text "Mammals"
						$foundOE2 = $true
						break
					}
				}
				if ($foundOE1 -and $foundOE2) { break }
			}

			TestIsTrue ($foundOE1 -and $foundOE2)
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
			TestIsGreaterOrEqual $notebooks.Count 1
			$foundNotebook = $false
			foreach ($notebook in $notebooks) {
				if ($notebook.Name -eq 'SidenoteTest') {
					$foundNotebook = $true
					break
				}
			}
			TestIsTrue $foundNotebook
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
			TestIsGreaterOrEqual $sections.Count 2
			$foundSection1 = $false
			$foundSection2 = $false
	
			foreach ($section in $sections) {
				switch ($section.Name) {
					'Section1' { $foundSection1 = $true; break }
					'Section2' { $foundSection2 = $true; break }
				}
				if ($foundSection1 -and $foundSection2) { break }
			}
	
			TestIsTrue ($foundSection1 -and $foundSection2)
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
			TestIsGreaterOrEqual $pages.Count 2
			$foundPage1 = $false
			$foundPage2 = $false

			foreach ($page in $pages) {
				switch ($page.Name) {
					'Page1' { $foundPage1 = $true; break }
					'Page2' { $foundPage2 = $true; break }
				}
				if ($foundPage1 -and $foundPage2) { break }
			}

			TestIsTrue ($foundPage1 -and $foundPage2)
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

			# For the time being, the OE underneath the Title element is not being exposed.
			TestIsGreaterOrEqual $outlines.Count 2

			$foundOutline1 = $false
			$foundOutline2 = $false

			foreach ($outline in $outlines) {
				switch ($outline.ID) {
					'{5DA5AC0E-BF64-490A-A961-2340FF1DAD9B}{10}{B0}' {
						TestAreEqual $outline.Author "Andreas Eulitz"
						TestAreEqual $outline.AuthorInitials "AE"
						$foundOutline1 = $true
						break
					}
					'{5DA5AC0E-BF64-490A-A961-2340FF1DAD9B}{14}{B0}' {
						TestAreEqual $outline.Author "Andreas Eulitz"
						TestAreEqual $outline.AuthorInitials "AE"
						$foundOutline2 = $true
						break
					}
				}
				if ($foundOutline1 -and $foundOutline2) { break }
			}

			TestIsTrue ($foundOutline1 -and $foundOutline2)
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
			TestIsGreaterOrEqual $outlineElements.Count 2
			$foundOE1 = $false
			$foundOE2 = $false

			foreach ($outlineElement in $outlineElements) {
				switch ($outlineElement.ID) {
					'{5DA5AC0E-BF64-490A-A961-2340FF1DAD9B}{11}{B0}' {
						TestAreEqual $outlineElement.Text "Outline1"
						$foundOE1 = $true
						break
					}
					'{5DA5AC0E-BF64-490A-A961-2340FF1DAD9B}{20}{B0}' {
						TestAreEqual $outlineElement.Text "Mammals"
						$foundOE2 = $true
						break
					}
				}
				if ($foundOE1 -and $foundOE2) { break }
			}

			TestIsTrue ($foundOE1 -and $foundOE2)
		} finally {
			Pop-Location
		}
	}
}

$standaloneLogFilePath = "$env:TEMP\$(PathFileBaseName $MyInvocation.MyCommand.Path).log"
RunTests $standaloneLogFilePath ([DriveTests])
