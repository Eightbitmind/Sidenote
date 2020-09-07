
$originalFileName = "$env:TEMP\Original.xml"
$originalNormalizedFileName = "$env:TEMP\Original-normalized.xml"


$sidenoteFileName = "$env:TEMP\Sidenote.xml"
$sidenoteNormalizedFileName = "$env:TEMP\Sidenote-normalized.xml"

$page = Get-ONTreeNode.ps1
$page | Get-ONXml > $originalFileName

& "$PSScriptRoot\NormalizeXml.ps1" -InFile $originalFileName > $originalNormalizedFileName

# requires modified Save method
$page.Save()

& "$PSScriptRoot\NormalizeXml.ps1" -InFile $sidenoteFileName > $sidenoteNormalizedFileName
code --diff $originalNormalizedFileName $sidenoteNormalizedFileName
