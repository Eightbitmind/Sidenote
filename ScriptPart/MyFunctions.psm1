
function GetDescendants(
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

filter CanonicalPeople {
	if (
		($_.Type -eq "OutlineElement") -and
		($_.Parent.Type -ne "Title") -and
		($_.Parent.Parent.Type -eq "Page") -and 
		$_.Parent.Parent.Name.StartsWith("People-")) {
		$_
	}
}

function ContainsMatch($Node, $Pattern) {
	foreach ($descendant in (GetDescendants -StartNode $Node -MinDepth ($Node.Depth + 1) -MaxDepth ([int]::MaxValue))) {
		if ($descendant.Type -eq "OutlineElement" -and $descendant.Text -and ($descendant.Text -match $Pattern)) {
			return $true
		}
	}
	return $false
}

function GetBirthdate($Node) {
	foreach ($descendant in (GetDescendants $Node)) {
		if ($descendant.Type -eq "OutlineElement" -and $descendant.Text -and ($descendant.Text -match "Birthdate: (?<Date>\d{4}-\d\d-\d\d)")) {
			return [System.DateTime]::Parse($Matches.Date)
		}
	}
	return $null
}

# function NotContainsTag($Node, $Tag) {
# 	$containsTag = $false
# 	foreach ($descendant in (GetDescendants -StartNode $Node -MinDepth ($_.Depth + 1) -MaxDepth ([int]::MaxValue))) {
# 		if ($descendant.Type -eq "OutlineElement" -and $descendant.Text -and $descendant.Text.Contains($Tag)) {
# 			$containsTag = $true
# 			break
# 		}
# 	}
# 	if(!$containsTag) { $_ }
# }

# function IsEven($n) { $n % 2 -eq 0 }
# function IsX3($n) { $_ % 3 -eq 0 }

# 1,2,3 | where{ if((IsEven $_) -or (IsX3 $_)) { $_ }}

function WriteAsMD($StartNode) {
	$listItemStack = [System.Collections.Stack]::new()

	GetDescendants $StartNode -MinDepth $StartNode.Depth | ForEach-Object {

		$node = $_

		# Write-Host "dbg: $($node.Type)"
		switch ($node.Type) {
			"Page" {
				$notebook = $node.Parent.Parent
				$section = $node.Parent
				Write-Output "___"
				Write-Output "# Book `"$($notebook.Name)`" / Section `"$($section.Name)`" / Page `"$($node.Name)`""
				Write-Output "<sub><sup>Author: $($node.Author), CreationTime: $($node.CreationTime); LastModifiedTime: $($node.LastModifiedTime)</sup></sub>"
				Write-Output "___"
				break
			}

			"OutlineElement" {
				if ($node.Parent.Type -eq "Title") {
					# TODO: output the path down to the page
					# Write-Output "___"
					# Write-Output "# Page: $($node.Text)"
					# Write-Output ""
					break
				}

				$indentation = ""
				$listItemPrefix = ""
				$EOLSuffix = "  "
				if ($node.ListItem) {

					if (($listItemStack.Count -eq 0) -or ($node.Depth -gt $listItemStack.Peek().Depth)) {
						$listItemStack.Push(@{Depth = $node.Depth; Index = 1})
					} elseif ($node.Depth -lt $listItemStack.Peek().Depth) {
						[void]($listItemStack.Pop())
					}

					$indentation = "   " * ($listItemStack.Count - 1)
					switch ($node.ListItem.Type) {
						([Sidenote.DOM.ListItemType]::BulletListItem) { $listItemPrefix = "- " }
						([Sidenote.DOM.ListItemType]::NumberedListItem) {
							$index = $listItemStack.Peek().Index++
							$listItemPrefix = "$index. "
						}
					}

					$EOLSuffix = ""
				} else {
					$listItemStack.Clear()
				}

				Write-Output "$indentation$listItemPrefix$($node.Text)$EOLSuffix"
				break
			}
		}
	}
}
