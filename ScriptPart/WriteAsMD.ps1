using module Sidenote

param(
	$StartNode,
	$File
)

function WriteOutput($Text) {
	Out-File -InputObject $Text -FilePath $File -Append -Encoding ASCII
}

function WritePage($Node) {
	if ($Node.Type -ne "Page") { return $false }

	$notebook = $Node.Parent.Parent
	$section = $Node.Parent
	WriteOutput "___"
	WriteOutput "## $($Node.Name)"
	WriteOutput "<sub><sup>Book `"$($notebook.Name)`" / Section `"$($section.Name)`" / Page `"$($Node.Name)`"</sup></sub>  "
	WriteOutput "<sub><sup>Author: $($Node.Author), CreationTime: $($Node.CreationTime); LastModifiedTime: $($Node.LastModifiedTime)</sup></sub>  "
	# WriteOutput "___"

	foreach ($child in $Node.Children) {
		if (!(
			(WriteTitle $child) -or
			(WriteOutline $child)
		)) {
			Write-Error "unexpected Page child $($child.Type)"
			return $false
		}
	}

	return $true
}

function WriteTitle($Node) { $Node.Type -eq "Title" }

function WriteOutline($Node) {
	if ($Node.Type -ne "Outline") { return $false }

	foreach ($child in $Node.Children) {
		if (!(
			(WriteOutlineElement $Child)
		)) {
			Write-Error "unexpected Outline child $($child.Type)"
			return $false
		}
	}

	return $true
}

$ListItemStack = [System.Collections.Stack]::new()

function WriteOutlineElement($Node) {
	if ($Node.Type -ne "OutlineElement") { return $false }

	# WriteOutput "$($Node.Text)  " # trailing spaces result in a line break

	$indentation = ""
	$listItemPrefix = ""
	$EOLSuffix = "  "
	if ($Node.ListItem) {

		if (($ListItemStack.Count -eq 0) -or ($Node.Depth -gt $ListItemStack.Peek().Depth)) {
			$ListItemStack.Push(@{Depth = $node.Depth; Index = 1})
		} elseif ($Node.Depth -lt $ListItemStack.Peek().Depth) {
			[void]($ListItemStack.Pop())
		}

		$indentation = "   " * ($ListItemStack.Count - 1)
		switch ($Node.ListItem.Type) {
			([Sidenote.DOM.ListItemType]::BulletListItem) { $listItemPrefix = "- " }
			([Sidenote.DOM.ListItemType]::NumberedListItem) {
				$index = $ListItemStack.Peek().Index++
				$listItemPrefix = "$index. "
			}
		}

		$EOLSuffix = ""
	} else {
		$ListItemStack.Clear()
	}

	WriteOutput "$indentation$listItemPrefix$($Node.Text)$EOLSuffix"

	foreach ($child in $Node.Children) {
		if (!(
			(WriteOutlineElement $child) -or
			(WriteTable $child)
		)) {
			Write-Error "unexpected OutlineElement child $($child.Type)"
			return $false
		}
	}

	return $true
}


function WriteTable($Node) {
	if ($Node.Type -ne "Table") { return $false }


	function GetCellText($cell) {
		$sb = [System.Text.StringBuilder]::new()
		foreach ($child in $cell.Children) {
			[void]($sb.Append($child.Text))
		}
		return $sb.ToString()
	}

	function WriteRow($row) {

		$sb = [System.Text.StringBuilder]::new()
		for ($column = 0; $column -lt $Node.ColumnCount; ++$column) {
			[void]($sb.Append((GetCellText $Node.GetCell($row, $column))).Append("|"))
		}

		WriteOutput $sb.ToString(0, $sb.Length - 1)
	}

	# We need at least two rows to make an MD table.

	if ($Node.RowCount -ge 2) {
		WriteRow 0

		$separatorLine = [System.Text.StringBuilder]::new()
		for ($column = 0; $column -lt $Node.ColumnCount; ++$column) {
			[void]($separatorLine.Append("--|"))
		}
		WriteOutput $separatorLine.ToString(0, $separatorLine.Length - 1)

		for ($row = 1; $row -lt $Node.RowCount; ++$row) { WriteRow $row }

	} else {
		Write-Error "implement support for one-row tables"
	}

	return $true
}

if (Test-Path $File) {
	Remove-Item $File
}

# TODO: accommodate different types of start nodes
[void](WritePage $StartNode)
