using module Gumby.Log
using module Gumby.Path
using module Object
using module TreeView
using module Window

$debug = $true

class OneNoteTVItem : TVItemBase {
	OneNoteTVItem([Sidenote.DOM.INode] $node) : base(<# level #> 0) {
		$this.node = $node
	}

	hidden OneNoteTVItem([Sidenote.DOM.INode] $node, [uint32] $level) : base($level) {
		$this.node = $node
	}

	[string] Name() { return $this.node.Name }

	[object] Value() { return $this.node }

	[bool] IsContainer() {
		# TODO: refine
		return $true
	}

	[TVItemBase] Parent() {
		if ($this.node.Parent) {
			return [OneNoteTVItem]::new($this.node.Parent, $this.Level() - 1)
		} else {
			return $null
		}
	}

	[Collections.Generic.IList`1[TVItemBase]] Children() {
		if ($this._children -eq $null) {

			$this._children = [Collections.Generic.List`1[TVItemBase]]::new()

			foreach ($child in $this.node.Children) {
				$this._children.Add([OneNoteTVItem]::new($child, $this.Level() + 1))
			}

		}

		return $this._children
	}

	hidden [Sidenote.DOM.INode] $node
	hidden [Collections.Generic.IList`1[TVItemBase]] $_children = $null
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

		$tv.Run()

		echo "Hi there!"

	} finally {
		if ($fll -ne $null) { [Log]::Listeners.Remove($fll) }
	}
}

Select-ONObjectVisually

