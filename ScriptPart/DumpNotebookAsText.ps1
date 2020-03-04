param(
	$Notebook
)

function DumpNode($node) {
	$namedObject = $node -as [Sidenote.DOM.INamedObject]
	if ($namedObject) {
		Write-Output "$($node.Type) `"$($namedObject.Name)`""
		return
	}

	$outlineElement = $node -as [Sidenote.DOM.IOutlineElement]
	if ($outlineElement) {
		$indent = "`t" * ($node.Depth - <# Book, Section, Page, Outline #> 4 )
		Write-Output "$indent$($outlineElement.Text)"
		return
	}

	# $notebook = $node -as [Sidenote.DOM.INotebook]
	# if ($notebook) {
	# 	Write-Output "Notebook `"$($notebook.Nickname)`""
	# 	return
	# }
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

TraverseHierarchy $Notebook { param($n) DumpNode $n }