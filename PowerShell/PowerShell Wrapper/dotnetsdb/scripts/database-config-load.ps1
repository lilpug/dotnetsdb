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

# Builds the config class structure
Class Config
{
	[String]$connectionType
	[bool]$windowsAuth
	[String]$host
	[String]$database
	[String]$username
	[String]$password	
	[bool]$errorLogger	
	[int]$timeout	
	[String]$additionalConnectionString
}

# Flags if the configuration file was loaded correctly
$configLoaded = $TRUE;
try
{
	# inits the class structure with the config.json file
	$config = [Config](Get-Content "$ScriptDir\..\config.json" | Out-String | ConvertFrom-Json)
}
catch
{
	$configLoaded = $FALSE;
}