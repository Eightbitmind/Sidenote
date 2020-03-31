using module ListBox 
using module Sidenote
using module Gumby.String

[CmdletBinding()]

param(
	[Parameter(ValueFromPipeline)]
	[Sidenote.DOM.INode[]] $Nodes
)

class NodeLBItem : LBItemBase {
	NodeLBItem($node) {
		$this._node = $node
	}

	[string] Name() {
		if (!$this._name) {
			$this._name = [NodeLBItem]::GetName($this._node)
		}
		return $this._name
	}

	[string] Value() { return $this._node }

	hidden static [string] GetName($node) {
		$sb = [System.Text.StringBuilder]::new()

		for (; $node; $node = $node.Parent) {
			switch ($node.Type) {
				"Notebook" {
					[void]($sb.Insert(0, "Book `"$($node.Name)`"/"))
					break
				}
				"Outline" {
					break
				}
				"OutlineElement" {
					[void]($sb.Insert(0, "`"$(Abbreviate $node.Text 20)`"/"))
					break
				}
				"Page" {
					[void]($sb.Insert(0, "Page `"$($node.Name)`"/"))
					break
				}
				"Section" {
					[void]($sb.Insert(0, "Section `"$($node.Name)`"/"))
					break
				}
				"Table" {
					[void]($sb.Insert(0, "Table/"))
					break
				}
			}
		}

		return $sb.ToString(0, $sb.Length - 1)
	}

	[void] OnSelected(){
		Select-ONObject $this._node
	}

	hidden [Sidenote.DOM.INode] $_node
	hidden [string] $_name
}

$horizontalPercent = 0.8
$verticalPercent = 0.8

$width = [console]::WindowWidth * $horizontalPercent
$left = [int](([console]::WindowWidth - $width) / 2)

$height = [console]::WindowHeight * $verticalPercent
$top = [int](([console]::WindowHeight - $height) / 2)

$lb = [ListBox]::new($input, ([NodeLBItem]), $left, $top, $width, $height, ([console]::BackgroundColor), ([console]::ForegroundColor))
$lb.Title = 'OneNote Items'

[void]($lb.Run())
