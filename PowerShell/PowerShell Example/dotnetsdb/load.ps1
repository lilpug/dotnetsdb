<#!
    Title: DotNetSDB PowerShell Wrapper
    URL: https://github.com/lilpug/dotnetsdb
    Author: David Whitehead
    Copyright (c) David Whitehead
    Copyright license: MIT
#>

<#***************************#>
<# 		Config loading 		 #>
<#***************************#>

# Gets the current scripts full path location
$ScriptDir = Split-Path -parent $MyInvocation.MyCommand.Path

if((Test-Path -Path "$ScriptDir\config.json" ))
{
	# Builds the paths
	$configPath = "$ScriptDir\scripts\database-config-load.ps1"
	$dbPath = "$ScriptDir\scripts\load-connection.ps1"
	
	#Loads the config
	. $configPath
	
	#Loads the db connection object
	. $dbPath
	
	#Checks if a connection was created or attempted
	if($connectionFailure)
	{
		write-Output "The database connection could not be created."
		write-Output "Note: if your using MySQL you cannot use windows authentication at this time."
	}
}
else
{
	write-Output "The config file does not exist or could not be loaded."
}