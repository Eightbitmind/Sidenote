param(
	$StartNode,
	[string] $Tag
)

function DumpNode($node) {
	switch ($node.Type) {
		"OutlineElement" {
#Write-Output "DM10"
			if ($node.Parent.Type -eq "Title") { break }
#Write-Output "DM20"

			if ($node.Depth -le <# Book, Section, Page, Outline, OE #> 5) { break }
#Write-Output "DM30"

			if ($node.Text -and $node.Text.Contains($Tag)) {
#Write-Output "DM40"
				Write-Output $node.Parent.Text
			}
			break
		}

	}
}
function TraverseHierarchy($startNode, $processNode) {
	$processNode.Invoke($startNode)

	# depth-first traversal
	$stack = [System.Collections.Stack]::new()

	if ($startNode.Children) { $stack.Push($startNode.Children.GetEnumerator()) }
	while ($stack.Count -gt 0) {
		$enum = $stack.Pop()
		if ($enum.MoveNext()) {
			$processNode.Invoke($enum.Current)
			$stack.Push($enum)
			if ($enum.Current.Children) { $stack.Push($enum.Current.Children.GetEnumerator()) }
		}
	}
}

TraverseHierarchy $StartNode { param($n) DumpNode $n }