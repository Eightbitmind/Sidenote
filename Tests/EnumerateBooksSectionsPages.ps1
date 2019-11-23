foreach ($notebook in (Get-NotebookRoot).Notebooks)
{
	Write-Host "notebook `"$($notebook.Name)`""

	foreach ($section in $notebook.Children)
	{
		Write-Host "`tsection `"$($section.Name)`""

		foreach ($page in $section.Children)
		{
			Write-Host "`t`tpage `"$($page.Name)`""
		}
	}
}
