using System;
namespace DotNetSDB.SqlServer.FileTable
{
    public partial class SQLServerFileTableExtension
    {
		public partial class FileTablePermissions
		{
            private bool FileGroupEnabled()
            {
                try
                {
                    //Checks if the filegroup has been activated on the database
                    connector.db.add_pure_sql($@"
                    --Checks if the filegroup stream is enabled
                    select (case when EXISTS(SELECT * FROM {dbName}.sys.database_files WHERE type_desc = 'FILESTREAM') then 1 else 0 end) as filegroup_activated");
                    string resultOne = connector.db.run_return_string();
                    if (!string.IsNullOrWhiteSpace(resultOne) && resultOne == "1")

                    {
                        return true;
                    }

                    return false;
                }
                catch (Exception e)
                {
                    throw errorHandler.CustomErrorOutput(e);
                }
            }

            private bool FileStreamEnabled()
            {
                try
                {
                    //Checks if the filegroup has been activated on the database
                    connector.db.add_pure_sql($@"
                    SELECT non_transacted_access
                    FROM sys.database_filestream_options
                    where DB_NAME(database_id) = '{dbName}'");
                    string resultOne = connector.db.run_return_string();
                    if (!string.IsNullOrWhiteSpace(resultOne) && resultOne == "2")

                    {
                        return true;
                    }

                    return false;
                }
                catch (Exception e)
                {
                    throw errorHandler.CustomErrorOutput(e);
                }
            }

            private bool FileStreamBaseEnabled()
            {
                try
                {
                    //Checks if filestream is enabled at the sql server instance base level "NOT THE DATABASE LEVEL"
                    connector.db.add_select("sys.configurations", "value");
                    connector.db.add_where_normal("sys.configurations", "name", "filestream access level");
                    var filestreamBase = connector.db.run_return_string();

                    //Note: 0 = disabled, 2 = enabled
                    if (!string.IsNullOrWhiteSpace(filestreamBase) && filestreamBase == "2")
                    {
                        return true;
                    }

                    return false;
                }
                catch (Exception e)
                {
                    throw errorHandler.CustomErrorOutput(e);
                }
            }
		}            
    }
    
}