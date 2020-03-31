using module Sidenote
#using module Gumby.Log
#using module Gumby.Path

function Get-ONPath {
	[CmdletBinding()]

	param (
		[Parameter(ValueFromPipeline)]
		[Sidenote.DOM.INode]$Node
	)

	process {
		$sb = [System.Text.StringBuilder]::new()

		for (; $Node; $Node = $Node.Parent) {
			$identifiableObject = $Node -as [Sidenote.DOM.IIdentifiableObject]
			if ($identifiableObject) {
				[void]($sb.Insert(0, '\').Insert(0, $identifiableObject.ID))
			}
		}

		[void]($sb.Insert(0, 'ON:\'))

		Write-Output $sb.ToString(0, $sb.Length - 1)
	}
}

function Set-ONLocation {
	[CmdletBinding()]

	param (
		[Parameter(ValueFromPipeline)]
		$Object
	)

	process {
		if ($Object -is [Sidenote.DOM.INode]) {
			Set-Location (Get-ONPath $Object)
		} else {
			Set-Location $Object
		}
	}
}

function Get-ONDescendants(
	$StartNode,
	[int] $MinDepth = ($StartNode.Depth + 1),
	[int] $MaxDepth = ([int]::MaxValue)) {
	# depth-first traversal
	$stack = [System.Collections.Stack]::new()

	if (($StartNode.Depth -ge $MinDepth) -and ($StartNode.Depth -le $MaxDepth)) {
		Write-Output $StartNode
	}

	if (($StartNode.Depth -lt $MaxDepth) -and $StartNode.Children) {
		$stack.Push($StartNode.Children.GetEnumerator())
	}

	while ($stack.Count -gt 0) {
		$enum = $stack.Pop()
		if (!$enum.MoveNext()) { continue }

		if (($enum.Current.Depth -ge $MinDepth) -and ($enum.Current.Depth -le $MaxDepth)) {
			Write-Output $enum.Current
		}

		$stack.Push($enum)
		if (($enum.Current.Depth -lt $MaxDepth) -and $enum.Current.Children) {
			$stack.Push($enum.Current.Children.GetEnumerator()) 
		}
	}
}

function Test-ONNodeContainsMatch($Node, $Pattern) {
	foreach ($descendant in (Get-ONDescendants -StartNode $Node -MinDepth ($Node.Depth + 1) -MaxDepth ([int]::MaxValue))) {
		if ($descendant.Type -eq "OutlineElement" -and $descendant.Text -and ($descendant.Text -match $Pattern)) {
			return $true
		}
	}
	return $false
}
