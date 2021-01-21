
	# Temp. change for demo:
	# org. name Get-ONPeople
	# Get-ONDescendants -StartNode (Get-Item 'ON:\{6BE9F100-1311-468F-9809-1F816F4DE7CD}{1}{B0}\{62827A6E-4C34-0877-3A09-110E33F90A04}{4}{B0}') |
	# Get-ONDescendants -StartNode (Get-Item 'ON:\{84247725-824B-42F7-B86D-3971948BAA47}{1}{B0}\{90DBFF60-54E0-017C-25AE-BE87940AC026}{1}{B0}') |

	function Get-ONContacts {
		dir -r 'ON:\{84247725-824B-42F7-B86D-3971948BAA47}{1}{B0}\{90DBFF60-54E0-017C-25AE-BE87940AC026}{1}{B0}' |
		where {
			if (
				($_.Type -eq "OutlineElement") -and
				($_.Parent.Parent.Type -eq "Page") -and
				($_.Parent.Type -ne "Title")
			) { $_ }
		}
	}
	
	function Get-ONContacts2 {
		Get-ONDescendants -StartNode (Get-Item 'ON:\{6BE9F100-1311-468F-9809-1F816F4DE7CD}{1}{B0}\{62827A6E-4C34-0877-3A09-110E33F90A04}{4}{B0}') |
		Where-Object {
			if (
				($_.Type -eq "OutlineElement") -and
				($_.Parent.Parent.Type -eq "Page") -and
				($_.Parent.Parent.Name.StartsWith("People-")) -and
				($_.Parent.Type -ne "Title")
			) { $_ }
		}
	}
	
	function Get-ONPrivatePhone($Contact) {
		foreach ($descendant in (Get-ONDescendants $Contact)) {
			if ($descendant.Type -eq "OutlineElement" -and $descendant.Text -and ($descendant.Text -match "Private Phone:\s*(?<Number>\+?[\d\.\-]+)")) {
				return $Matches.Number
			}
		}
		return $null
	}
	
	function Get-ONBirthdate($Node) {
		foreach ($descendant in (Get-ONDescendants $Node)) {
			if ($descendant.Type -eq "OutlineElement" -and $descendant.Text -and ($descendant.Text -match "Birthdate: (?<Date>\d{4}-\d\d-\d\d)")) {
				return [System.DateTime]::Parse($Matches.Date)
			}
		}
		return $null
	}