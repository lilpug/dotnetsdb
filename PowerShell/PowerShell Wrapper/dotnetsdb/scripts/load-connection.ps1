<#!
    Title: DotNetSDB PowerShell Wrapper
    URL: https://github.com/lilpug/dotnetsdb
    Author: David Whitehead
    Copyright (c) David Whitehead
    Copyright license: MIT
#>

<#*******************************#>
<#  Database Connection loading  #>
<#*******************************#>

$connectionFailure = $FALSE

# Checks the configuration file was loaded correctly
if($configLoaded)
{
	# Gets the current scripts full path location
	$ScriptDir = Split-Path -parent $MyInvocation.MyCommand.Path

	# Loads the system.data library as we are using datatables
	[void] [System.Reflection.Assembly]::LoadWithPartialName("System")

	# Loads the MySQL.Data library dependency using the full path of the script as its in the same folder structure
	[System.Reflection.Assembly]::LoadFile("$ScriptDir\..\dll\MySql.Data.dll")
	
	# Loads the DotNetSDB library using the full path of the script as its in the same folder structure
	[System.Reflection.Assembly]::LoadFile("$ScriptDir\..\dll\dotnetsdb.dll")

	$logger = $null
	if($config.errorLogger)
	{
		# Creates the log directory if it does not exist already
		$logsDirectory = "$ScriptDir\..\error-logs"	
		if(!(Test-Path -Path $logsDirectory ))
		{
			New-Item -ItemType directory -Path $logsDirectory
		}	
		
		#Sets the timezoneinfo for the logger
		$timezoneInfo = [TimeZoneInfo]::FindSystemTimeZoneById("GMT Standard Time");

		# Sets up the database connection
		$logger = New-Object DotNetSDB.output.OutputManagementSettings -argumentlist "$ScriptDir\..\error-logs", "database-log", $timezoneInfo, true
	}

	$con = $null
	if($config.windowsAuth)
	{
		# Sets up the windows auth database connection
		$con = New-Object DotNetSDB.SQLServerConnection -argumentlist $config.host, $config.database, $config.timeout, $logger, $config.additionalConnectionString
	}
	else
	{
		# Sets up the sql user database connection		
		$con = New-Object DotNetSDB.SQLServerConnection -argumentlist $config.host, $config.username, $config.password, $config.database, $config.timeout, $logger, $config.additionalConnectionString
	}

	try
	{
		# Connects to the database with that connection variable
		$db = $null
		if($config.connectionType -eq "SQLServer2008")
		{
			$db = New-Object DotNetSDB.SQLServer2008 $con
		}
		Elseif($config.connectionType -eq "SQLServer2012")
		{
			$db = New-Object DotNetSDB.SQLServer2012 $con
		}
		Elseif($config.connectionType -eq "SQLServer2014")
		{
			$db = New-Object DotNetSDB.SQLServer2014 $con
		}
		Elseif($config.connectionType -eq "SQLServer2016")
		{
			$db = New-Object DotNetSDB.SQLServer2016 $con
		}
		Elseif($config.connectionType -eq "MySQLCore" -And $config.windowsAuth -eq $FALSE)
		{
			$db = New-Object DotNetSDB.SQLServer2016 $con
		}
		else
		{
			$connectionFailure = $TRUE
		}
	}
	catch
	{
		# Sets the failure flag
		$connectionFailure = $TRUE		
		
		# Rethrows the exception
		throw $_.Exception    
	}
}