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
	[void] Drive_ContainsTestNotebook() {
		$notebooks = Get-ChildItem 'ON:'
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
			if ($section.Name -eq 'Section1') { $foundSection1 = $true }
			if ($section.Name -eq 'Section2') { $foundSection2 = $true }
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
			if ($page.Name -eq 'Page1') { $foundPage1 = $true }
			if ($page.Name -eq 'Page2') { $foundPage2 = $true }
			if ($foundPage1 -and $foundPage2) { break }
		}

		TestIsTrue ($foundPage1 -and $foundPage2)
	}
}

$standaloneLogFilePath = "$env:TEMP\$(PathFileBaseName $MyInvocation.MyCommand.Path).log"
RunTests $standaloneLogFilePath ([DriveTests])
