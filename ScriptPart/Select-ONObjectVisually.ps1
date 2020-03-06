using module Gumby.Log
using module Gumby.Path
using module Gumby.String
using module Sidenote # script doesn't seem to load if we haven't imported this module beforehand
using module TreeView
using module Window

param(
	[switch] $SelectInGUI = $false
)

$debug = $true

class OneNoteTVItem : TVItemBase {
	OneNoteTVItem([Sidenote.DOM.INode] $node) : base(<# level #> 0) {
		$this.node = $node
	}

	hidden OneNoteTVItem([Sidenote.DOM.INode] $node, [uint32] $level) : base($level) {
		$this.node = $node
	}

	[string] Name() {
		if ($this.node -is ([Sidenote.DOM.INamedObject])) {
			return $this.node.Name
		} elseif ($this.node -is ([Sidenote.DOM.IOutlineElement])) {
			return (Abbreviate $this.node.Text 20)
		} else {
			foreach($child in $this.node.Children) {
				$namedObject = $child -as [Sidenote.DOM.INamedObject]
				if ($namedObject) { return "containing `"$($namedObject.Name)`"" }
				$outlineElement = $child -as [Sidenote.DOM.IOutlineElement]
				if ($outlineElement) { return "containing `"$(Abbreviate $outlineElement.Text 20)`"" }
			}
			return $this.node.GetType().Name
		}
	}

	[object] Value() { return $this.node }

	[bool] IsContainer() {
		# TODO: refine
		return $true
	}

	[TVItemBase] Parent() {
		# to not go up to root, let's limit on level rather than parent availability
		if ($this.Level() -gt 0) {
			return [OneNoteTVItem]::new($this.node.Parent, $this.Level() - 1)
		} else {
			return $null
		}
	}

	[Collections.Generic.IList`1[TVItemBase]] Children() {
		if ($this._children -eq $null) {

			$this._children = [System.Collections.Generic.List`1[TVItemBase]]::new()

			foreach ($child in $this.node.Children) {
				if ($child -is [Sidenote.DOM.IIdentifiableObject]) {
					$this._children.Add([OneNoteTVItem]::new($child, $this.Level() + 1))
				}
			}
		}

		return $this._children
	}

	[void] OnSelected(){
		if ($script:SelectInGUI) { Select-ONObject $this.node }
	}

	hidden [Sidenote.DOM.INode] $node
	hidden [System.Collections.Generic.IList`1[TVItemBase]] $_children = $null
}

function Get-ONObjectPath($Node) {
	$sb = [System.Text.StringBuilder]::new()

	for(; $Node; $Node = $Node.Parent) {
		$identifiableObject = $Node -as [Sidenote.DOM.IIdentifiableObject]
		if ($identifiableObject) {
			[void]($sb.Insert(0, '\').Insert(0, $identifiableObject.ID))
		}
	}

	[void]($sb.Insert(0, 'ON:\'))

	return $sb.ToString(0, $sb.Length - 1)
}

function Select-ONObjectVisually() {
	$fll = $null
	if ($debug) {
		$logFileName = "$env:TEMP\$(PathFileBaseName $PSCommandPath).log"
		if (Test-Path $logFileName) { Remove-Item $logFileName }
		$fll = [FileLogListener]::new($logFileName)
		[void][Log]::Listeners.Add($fll)
	}

	try {
		$items = (Get-ONRoot).Children

		$horizontalPercent = 0.8
		$verticalPercent = 0.8

		$width = [console]::WindowWidth * $horizontalPercent
		$left = [int](([console]::WindowWidth - $width) / 2)

		$height = [console]::WindowHeight * $verticalPercent
		$top = [int](([console]::WindowHeight - $height) / 2)

		$tv = [TreeView]::new($items, ([OneNoteTVItem]), $left, $top, $width, $height, ([console]::BackgroundColor), ([console]::ForegroundColor))
		$tv.Title = 'Select OneNote Object'

		if (($tv.Run() -eq [WindowResult]::OK) -and ($tv.SelectedIndex() -lt $tv.ItemCount())) {
			# Write-Host (Get-ONObjectPath $tv.SelectedItem().Value())
			Set-Location (Get-ONObjectPath $tv.SelectedItem().Value())
		}

	} finally {
		if ($fll -ne $null) { [Log]::Listeners.Remove($fll) }
	}
}

Select-ONObjectVisually
