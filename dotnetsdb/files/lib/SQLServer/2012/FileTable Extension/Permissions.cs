using System;
using System.Data;

namespace DotNetSDB
{
    public partial class SqlServer2012 : SqlServerCore
    {
        public partial class filetable_extension
        {
            public DataTable database_get_files_locked(string databaseName)
            {
                try
                {
                    db.add_pure_sql(String.Format(@"
                    SELECT opened_file_name
                    FROM sys.dm_filestream_non_transacted_handles
                    WHERE fcb_id IN ( SELECT request_owner_id FROM sys.dm_tran_locks )
                    and DB_NAME(database_id) = '{0}'
                    ", databaseName));
                    return db.run_return_datatable();
                }
                catch (Exception e)
                {
                    throw CustomErrorOutput(e);
                }
            }

            public DataTable database_get_filetable_permissions(string databaseName)
            {
                try
                {
                    //Gets the filegroup and filestream activation information
                    db.add_pure_sql(String.Format(@"
                    SELECT DB_NAME(database_id),
                    non_transacted_access,
                    non_transacted_access_desc,
                    directory_name,
				    (case when non_transacted_access = 2 then 1 else 0 end) as filestream_activated,
				    (case when EXISTS(SELECT * FROM sys.database_files WHERE type_desc = 'FILESTREAM') then 1 else 0 end) as filegroup_activated
                    FROM sys.database_filestream_options
                    where DB_NAME(database_id) = {0}
                    ", db.add_pure_sql_bind(databaseName)));
                    return db.run_return_datatable();
                }
                catch (Exception e)
                {
                    throw CustomErrorOutput(e);
                }
            }

            public bool database_has_filetable_permission(string databaseName)
            {
                //Checks if the access is enough for the filetable creation etc
                if (FileGroupEnabled(databaseName) && FileStreamEnabled(databaseName))
                {
                    return true;
                }

                return false;
            }

            public void database_enable_filetable_permissions(string databaseName, string directoryName = "DocumentTable")
            {
                try
                {
                    //Checks if the file group is not enabled, if so it activates it
                    if (!FileGroupEnabled(databaseName))
                    {
                        //Actives the filegroup on the database
                        db.add_pure_sql(String.Format(@"
                        --Note: This is processed and executed as a string query because otherwise we cannot put the instance path in!!

                        --Gets the default path for sql
                        DECLARE @default_path varchar(1024)
                        select @default_path = CONCAT(CONVERT(sysname, serverproperty('InstanceDefaultDataPath')), '{0}')

                        DECLARE @db_name varchar(255)
                        SET @db_name = '{0}'

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
                        exec(@query)
                        ", databaseName));
                        db.start_new_query();
                    }

                    //Checks if the filestream is not enabled, if so it activates it
                    if (!FileStreamEnabled(databaseName))
                    {
                        //Actives the filestream on the database
                        db.add_pure_sql(String.Format(@"
                        ALTER DATABASE {0}
                        SET FILESTREAM ( NON_TRANSACTED_ACCESS = FULL, DIRECTORY_NAME = N'{1}' )
                        WITH NO_WAIT -- allows the real error to come out rather then timeout exception
                        ", databaseName, directoryName));
                        db.run();
                    }
                }
                catch (Exception e)
                {
                    throw CustomErrorOutput(e);
                }
            }

            public void database_disable_filetable_permissions(string databaseName)
            {
                try
                {
                    //Checks if the file group is enabled, if so it disables it
                    if (FileGroupEnabled(databaseName))
                    {
                        //Removes the filegroup from the database
                        db.add_pure_sql(String.Format(@"
                            ALTER DATABASE {0}
                            REMOVE FILE My_Filestream

                            ALTER DATABASE {0}
                            REMOVE FILEGROUP [My_Filestream]
                        ", databaseName));
                        db.run();
                    }

                    //Checks if the filestream is enabled, if so it disables it
                    if (FileStreamEnabled(databaseName))
                    {
                        //Removes the filestream access from the database
                        db.add_pure_sql(String.Format(@"
                            ALTER DATABASE {0}
                            SET FILESTREAM ( NON_TRANSACTED_ACCESS = OFF )
                            WITH NO_WAIT -- allows the real error to come out rather then timeout exception
                        ", databaseName));
                        db.run();
                    }
                }
                catch (Exception e)
                {
                    throw CustomErrorOutput(e);
                }
            }
        }
    }
}