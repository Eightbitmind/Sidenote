using module Sidenote

param(
	$StartNode
)

function DumpNode($node) {
	switch ($node.Type) {
		"OutlineElement" {
			if ($node.Parent.Type -eq "Title") { break }

			$indent = "`t" * ($node.Depth - <# Book, Section, Page, Outline #> 4 )
			Write-Output "$indent$($node.Text)"
			break
		}

		"Title" {
			<# ignore #>
			break
		}

		"Page" {
			Write-Output "Page `"$($node.Name)`""
			break
		}
	}
}

function TraverseHierarchy($startNode, $processNode) {
	$processNode.Invoke($startNode)

	# depth-first traversal
	$stack = [System.Collections.Stack]::new()
	$stack.Push($startNode.Children.GetEnumerator());
	while ($stack.Count -gt 0) {
		$enum = $stack.Pop()
		if ($enum.MoveNext()) {
			$processNode.Invoke($enum.Current)
			$stack.Push($enum)
			$stack.Push($enum.Current.Children.GetEnumerator())
		}
	}
}

TraverseHierarchy $StartNode { param($n) DumpNode $n }
