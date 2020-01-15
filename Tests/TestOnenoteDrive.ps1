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
	[void] Drive_ContainsTwoNotebooks() {
		$items = Get-ChildItem 'ON:'
		TestAreEqual $items.Count 2
		TestAreEqual $items[0].Name 'Personal (Web)'
		TestAreEqual $items[1].Name 'Notizbuch'
	}
}

$standaloneLogFilePath = "$env:TEMP\$(PathFileBaseName $MyInvocation.MyCommand.Path).log"
RunTests $standaloneLogFilePath ([DriveTests])
