using System;
using System.Data;

namespace DotNetSDB.SqlServer.FileTable
{
    public partial class SQLServerFileTableExtension
    {
		public partial class FileTablePermissions
		{
            public DataTable get_locked_files()
            {
                try
                {
                    connector.db.add_pure_sql($@"
                    SELECT opened_file_name
                    FROM sys.dm_filestream_non_transacted_handles
                    WHERE fcb_id IN ( SELECT request_owner_id FROM sys.dm_tran_locks )
                    and DB_NAME(database_id) = {connector.db.add_pure_sql_bind(dbName)}");
                    return connector.db.run_return_datatable();
                }
                catch (Exception e)
                {
                    throw errorHandler.CustomErrorOutput(e);
                }
            }

            public DataTable get_base_instance_permissions()
            {
                //select value from sys.configurations where name = 'filestream access level' and value = 2

                connector.db.add_select("sys.configurations", "*");
                connector.db.add_where_normal("sys.configurations", "name", "filestream access level");
                return connector.db.run_return_datatable();
            }

            public DataTable get_all_permissions()
            {
                try
                {
                    //Gets the filegroup and filestream activation information
                    connector.db.add_pure_sql($@"
                    SELECT DB_NAME(database_id),
                    non_transacted_access,
                    non_transacted_access_desc,
                    directory_name,
				    (case when non_transacted_access = 2 then 1 else 0 end) as filestream_activated,
				    (case when EXISTS(SELECT * FROM sys.database_files WHERE type_desc = 'FILESTREAM') then 1 else 0 end) as filegroup_activated,
                    (case when EXISTS(SELECT value FROM sys.configurations WHERE name = 'filestream access level' AND value = 2) then 1 else 0 end) as base_instance_activated
                    FROM sys.database_filestream_options
                    where DB_NAME(database_id) = {connector.db.add_pure_sql_bind(dbName)}");
                    return connector.db.run_return_datatable();
                }
                catch (Exception e)
                {
                    throw errorHandler.CustomErrorOutput(e);
                }
            }

            public bool has_all_required_permissions()
            {
                //Checks if the access is enough for the filetable creation etc
                if (FileStreamBaseEnabled() && FileGroupEnabled() && FileStreamEnabled())
                {
                    return true;
                }

                return false;
            }

            public void enable_all(string directoryName = "DocumentTable")
            {
                try
                {
                    //Checks if the filestream is not enabled on the sql server base instance "NOT DB LEVEL", if so it actives it
                    enable_base_instance();

                    //Checks if the file group is not enabled, if so it activates it
                    if (!FileGroupEnabled())
                    {
                        //Actives the filegroup on the database
                        connector.db.add_pure_sql($@"
                        --Note: This is processed and executed as a string query because otherwise we cannot put the instance path in!!

                        --Gets the default path for sql
                        DECLARE @default_path varchar(1024)
                        select @default_path = CONCAT(CONVERT(sysname, serverproperty('InstanceDefaultDataPath')), '{dbName}')

                        DECLARE @db_name varchar(255)
                        SET @db_name = '{dbName}'

                        --Builds the query with the path
                        DECLARE @query varchar(max)
                        SET @query =
                        '
	                        ALTER DATABASE ' + @db_name + '
	                        ADD FILEGROUP My_Filestream	CONTAINS FILESTREAM

	                        ALTER DATABASE ' + @db_name + '
	                        ADD FILE( NAME = N''My_Filestream'', FILENAME =  N''' + @default_path + ''')
	                        TO FILEGROUP [My_Filestream]
                        '
                        exec(@query)");
                        connector.db.start_new_query();
                    }

                    //Checks if the filestream is not enabled, if so it activates it
                    if (!FileStreamEnabled())
                    {
                        //Actives the filestream on the database
                        connector.db.add_pure_sql($@"
                        ALTER DATABASE {dbName}
                        SET FILESTREAM ( NON_TRANSACTED_ACCESS = FULL, DIRECTORY_NAME = N'{directoryName}' )
                        WITH NO_WAIT -- allows the real error to come out rather then timeout exception");
                        connector.db.run();
                    }
                }
                catch (Exception e)
                {
                    throw errorHandler.CustomErrorOutput(e);
                }
            }
            
            public void enable_base_instance()
            {
                //Checks if the filestream is not enabled on the sql server base instance "NOT DB LEVEL", if so it actives it
                if (!FileStreamBaseEnabled())
                {
                    connector.db.add_pure_sql(@"
                            EXEC sp_configure filestream_access_level, 2;
                            RECONFIGURE;
                        ");
                    connector.db.run();
                }
            }

            public void disable_base_instance()
            {
                //Checks if the filestream is enabled on the sql server base instance "NOT DB LEVEL", if so it disables it
                //Note: this requires sql server instance to be rebooted
                if (FileStreamBaseEnabled())
                {
                    connector.db.add_pure_sql(@"
                            EXEC sp_configure filestream_access_level, 0;
                            RECONFIGURE;
                        ");
                    connector.db.run();
                }
            }
                        
            public void disable_all()
            {
                try
                {
                    //Checks if the file group is enabled, if so it disables it
                    if (FileGroupEnabled())
                    {
                        //Removes the filegroup from the database
                        connector.db.add_pure_sql($@"
                            ALTER DATABASE {dbName}
                            REMOVE FILE My_Filestream

                            ALTER DATABASE {dbName}
                            REMOVE FILEGROUP [My_Filestream]");
                        connector.db.run();
                    }

                    //Checks if the filestream is enabled, if so it disables it
                    if (FileStreamEnabled())
                    {
                        //Removes the filestream access from the database
                        connector.db.add_pure_sql($@"
                            ALTER DATABASE {dbName}
                            SET FILESTREAM ( NON_TRANSACTED_ACCESS = OFF )
                            WITH NO_WAIT -- allows the real error to come out rather then timeout exception");
                        connector.db.run();
                    }

                    //Checks if the filestream is enabled on the sql server base instance "NOT DB LEVEL", if so it disables it
                    //Note: this requires sql server instance to be rebooted
                    disable_base_instance();
                }
                catch (Exception e)
                {
                    throw errorHandler.CustomErrorOutput(e);
                }
            }
		}
    }
    
}