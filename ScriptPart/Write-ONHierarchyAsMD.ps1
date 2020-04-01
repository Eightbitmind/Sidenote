using module Sidenote

[CmdletBinding()]

param(
	[Parameter(ValueFromPipeline)]
	$StartNode,

	$File = $null
)

begin {
	if ($File) {
		if(Test-Path $File) { Remove-Item $File }
	} else {
		$output = [System.Collections.ArrayList]::new()
	}
}

process {

	function WriteOutput($Text) {
		if ($File) {
			Out-File -InputObject $Text -FilePath $File -Append -Encoding ASCII
		} else {
			$output.Add($Text)
		}
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
		return `
			(WriteNonListItemOutlineElement $Node) -or
			(WriteBulletListItem $Node) -or
			(WriteNumberedListItem $Node)
	}
	
	function WriteNonListItemOutlineElement($Node) {
		if (!(
			$Node.Type -eq "OutlineElement" -and
			!($Node.ListItem)
		)) {
			return $false
		}
	
		WriteOutput "$($Node.Text)  "
	
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
	
	function WriteBulletListItem($Node) {
		if (!(
			$Node.Type -eq "OutlineElement" -and
			$Node.ListItem -and
			($Node.ListItem.Type -eq ([Sidenote.DOM.ListItemType]::BulletListItem))
		)) {
			return $false
		}
	
		if (($ListItemStack.Count -eq 0) -or ($Node.Depth -gt $ListItemStack.Peek().Depth)) {
			$ListItemStack.Push(@{Depth = $node.Depth; Index = 1})
		} elseif ($Node.Depth -lt $ListItemStack.Peek().Depth) {
			[void]($ListItemStack.Pop())
		}
	
		$indentation = "   " * ($ListItemStack.Count - 1)
		$listItemPrefix = "- "
	
		WriteOutput "$indentation$listItemPrefix$($Node.Text)"
	
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
	
	function WriteNumberedListItem($Node) {
		if (!(
			$Node.Type -eq "OutlineElement" -and
			$Node.ListItem -and
			($Node.ListItem.Type -eq ([Sidenote.DOM.ListItemType]::NumberedListItem))
		)) {
			return $false
		}
	
		if (($ListItemStack.Count -eq 0) -or ($Node.Depth -gt $ListItemStack.Peek().Depth)) {
			$ListItemStack.Push(@{Depth = $node.Depth; Index = 1})
		} elseif ($Node.Depth -lt $ListItemStack.Peek().Depth) {
			[void]($ListItemStack.Pop())
		}
	
		$indentation = "   " * ($ListItemStack.Count - 1)
		$index = $ListItemStack.Peek().Index++
		$listItemPrefix = "$index. "
	
		WriteOutput "$indentation$listItemPrefix$($Node.Text)"
	
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

	function WritePage($Node) {
		if ($Node.Type -ne "Page") { return $false }

		$ListItemStack.Clear()
	
		$notebook = $Node.Parent.Parent
		$section = $Node.Parent
		WriteOutput "___"
		WriteOutput "### $($Node.Name)"
		WriteOutput "> <sub><sup>Book `"$($notebook.Name)`" / Section `"$($section.Name)`" / Page `"$($Node.Name)`"</sup></sub>  "
		WriteOutput "> <sub><sup>CreationTime: $($Node.CreationTime); LastModifiedTime: $($Node.LastModifiedTime); ID: `"$($Node.ID)`"</sup></sub>  "
		WriteOutput ""
	
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

	function WriteSection($Node) {
		if ($Node.Type -ne "Section") { return $false }

		$notebook = $Node.Parent
		WriteOutput "___"
		WriteOutput "## $($Node.Name)"
		WriteOutput "> <sub><sup>Book `"$($notebook.Name)`" / Section `"$($Node.Name)`" </sup></sub>  "
		WriteOutput "> <sub><sup>LastModifiedTime: $($Node.LastModifiedTime); Path: `"$($Node.Path)`"; Color: $($Node.Color); ID: `"$($Node.ID)`"</sup></sub>  "
		WriteOutput ""

		foreach ($child in $Node.Children) {
			if (!(WritePage $child)) {
				Write-Error "unexpected Section child $($child.Type)"
				return $false
			}
		}

		return $true
	}

	function WriteNotebook($Node) {
		if ($Node.Type -ne "Notebook") { return $false }

		WriteOutput "___"
		WriteOutput "# $($Node.Name)"
		WriteOutput "> <sub><sup>LastModifiedTime: $($Node.LastModifiedTime); Path: `"$($Node.Path)`"; Color: $($Node.Color); ID: `"$($Node.ID)`"</sup></sub>  "
		WriteOutput ""

		foreach ($child in $Node.Children) {
			if (!(WriteSection $child)) {
				Write-Error "unexpected Notebook child $($child.Type)"
				return $false
			}
		}

		return $true
	}

	# TODO: accommodate different types of start nodes
	switch ($StartNode.Type) {
		"Notebook" { [void](WriteNotebook $StartNode) }
		"Outline" {[void](WriteOutline $StartNode)}
		"OutlineElement" { [void](WriteOutline $StartNode) }
		"Page" { [void](WritePage $StartNode) }
		"Section" { [void](WriteSection $StartNode) }
		"Table" { [void](WriteTable $StartNode) }
		default { Write-Error "writing of `"$($StartNode.Type)`" nodes NYI" }
	}
}

end {
	if (!$File) {
		Write-Output $output
	}
}
