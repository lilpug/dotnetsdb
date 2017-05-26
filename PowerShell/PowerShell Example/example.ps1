<#!
    Title: DotNetSDB PowerShell Example
    URL: https://github.com/lilpug/dotnetsdb
    Author: David Whitehead
    Copyright (c) David Whitehead
    Copyright license: MIT
#>

# Loads the DotNetSDB PowerShell Wrapper
. .\dotnetsdb\load.ps1

# Connection to the database was established
if($connectionFailure -eq $FALSE)
{
	# Builds the query
	$db.add_select("example_table", "*")
	
	# Executes it and returns it in a DataTable format
	$dt = $db.run_return_datatable()
	
	#Outputs the results to the console
	write-Output $dt
}