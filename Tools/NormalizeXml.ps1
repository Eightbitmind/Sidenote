param(
	$InFile
)
# Workflow
# Get-ONTreeNode.ps1 | Get-ONXml > C:\Users\aeulitz\AppData\Local\Temp\Original.xml
# .\Projects\Sidenote\Tools\NormalizeXml.ps1 -InFile C:\Users\aeulitz\AppData\Local\Temp\Original.xml > C:\Users\aeulitz\AppData\Local\Temp\Original-normalized.xml
# $p = Get-ONTreeNode.ps1
# requires modified Save method
# $p.Save()
# .\Projects\Sidenote\Tools\NormalizeXml.ps1 -InFile C:\Users\aeulitz\AppData\Local\Temp\Sidenote.xml > C:\Users\aeulitz\AppData\Local\Temp\Sidenote-normalized.xml
# code --diff C:\Users\aeulitz\AppData\Local\Temp\Original-normalized.xml C:\Users\aeulitz\AppData\Local\Temp\Sidenote-normalized.xml


function WriteOutput($text){
	Write-Output $text
}


function DepthFirstDescendants ($Root) {
	begin {
		$stack = [System.Collections.Stack]::new()
		if ($root.HasChildNodes) {
			$stack.Push(@{StartEnd = 'End'; Node = $root; Depth = 0})
			$stack.Push(@{StartEnd = 'Start'; Node = $root; Depth = 0})
		} else {
			$stack.Push(@{StartEnd = 'StartEnd'; Node = $root; Depth = 0})
		}
	}

	process {
		while ($stack.Count -gt 0) {
			$current = $stack.Pop()

			Write-Output($current)
			if ($current.StartEnd -eq 'Start') { # synonymous with "HasChildNodes"
				for ($i = $current.Node.ChildNodes.Count - 1; $i -ge 0; --$i) {
					$child = $current.Node.ChildNodes[$i]

					if ($child.HasChildNodes) {
						$stack.Push(@{StartEnd = 'End'; Node = $child; Depth = $current.Depth + 1})
						$stack.Push(@{StartEnd = 'Start'; Node = $child; Depth = $current.Depth + 1})
					} else {
						$stack.Push(@{StartEnd = 'StartEnd'; Node = $child; Depth = $current.Depth + 1})
					}
				}

			}

		}
	}

}

function WriteAttributeValue($value) {

	# round fractional numeric values
	[int] $dummyInt = 0
	[double] $valueAsDouble = 0
	if (!([int]::TryParse($value, [ref] $dummyInt)) -and ([double]::TryParse($value, [ref] $valueAsDouble))) {
		$value = [System.Math]::Round($valueAsDouble, 11)
	}

	$stringWriter = [System.IO.StringWriter]::new()
	$xmlWriter = [System.Xml.XmlTextWriter]::new($stringWriter)
	$xmlWriter.WriteString($value)
	$xmlWriter.Flush()
	return $stringWriter.ToString()
}

function WriteAttributes($attributes, $depth) {
	$attributes | Sort-Object -Property Name | ForEach-Object {
		WriteOutput "$("`t" * $depth)$($_.Name)=`"$(WriteAttributeValue $_.Value)`""
	}
}

function WriteElement($element) {

	if ($element.StartEnd -eq 'End') {
		WriteOutput "$("`t" * ($element.Depth - 1))</$($element.Node.Prefix):$($element.Node.LocalName)>"
		return
	}

	$text = [System.Text.StringBuilder]::new()

	[void]$text.Append("$("`t" * ($element.Depth - 1))<$($element.Node.Prefix):$($element.Node.LocalName)")

	if (!$element.Node.HasAttributes) {
		switch ($element.StartEnd) {
			'Start' {
				[void]$text.AppendLine(">")
			}
		
			'StartEnd' {
				[void]$text.AppendLine("/>")
			}
		}
		WriteOutput $text.ToString()
		return
	}

	WriteOutput $text.ToString()
	WriteAttributes $element.Node.Attributes $element.Depth

	switch ($element.StartEnd) {
		'Start' {
			WriteOutput "$("`t" * $element.Depth)>"
		}

		'StartEnd' {
			WriteOutput "$("`t" * $element.Depth)/>"
		}
	}
}

[xml] $root = Get-Content $InFile # System.Xml.XmlNode

DepthFirstDescendants $root | ForEach-Object {
	$current = $_

	switch ($current.Node.Name) {
		"#document" { <# ignore #> }
		"#cdata-section" {
			WriteOutput "$("`t" * $($current.Depth - 1))<![CDATA[$($current.Node.InnerText)]]>" }
		"xml" { WriteOutput "<?xml version=`"$($current.Node.Version)`"?>" }
		default {
			WriteElement $current
		}
	}
}
