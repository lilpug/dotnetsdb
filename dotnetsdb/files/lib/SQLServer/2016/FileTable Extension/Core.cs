using System;
using System.Data;

namespace DotNetSDB
{
    public partial class SqlServer2016 : SqlServerCore
    {
        public partial class filetable_extension
        {
            private string CreateNewID(string tableName, string streamID)
            {
                try
                {
                    //Gets the path locator for combining with the new ID
                    db.add_select(tableName, "path_locator", "CAST( ", " AS varchar(max)) as path_locator");
                    db.add_where_normal(tableName, "stream_id", streamID);
                    db.add_where_normal(tableName, "is_directory", 1);
                    string folderLocator = db.run_return_string();

                    //Ensures the path locator is not empty
                    if (!string.IsNullOrWhiteSpace(folderLocator))
                    {
                        //Creates the new ID and outputs it
                        db.add_pure_sql(String.Format("select CONCAT('{0}', CONVERT(VARCHAR(20), CONVERT(BIGINT, SUBSTRING(CONVERT(BINARY(16), NEWID()), 1, 6))) +'.'+ CONVERT(VARCHAR(20), CONVERT(BIGINT, SUBSTRING(CONVERT(BINARY(16), NEWID()), 7, 6))) +'.'+ CONVERT(VARCHAR(20), CONVERT(BIGINT, SUBSTRING(CONVERT(BINARY(16), NEWID()), 13, 4))), '/') as path", folderLocator));
                        return db.run_return_string();
                    }
                    else
                    {
                        throw new Exception("Database FileTable Create: The stream_id you have supplied does not exist.");
                    }
                }
                catch (Exception e)
                {
                    throw CustomErrorOutput(e);
                }
            }

            private void DeleteDirectoryRecursively(string tableName, string locator)
            {
                try
                {
                    db.add_select(tableName, "path_locator", "CAST( ", " AS varchar(max)) as path_locator");
                    db.add_where_normal(tableName, "parent_path_locator", locator);
                    db.add_where_normal(tableName, "is_directory", 1);
                    DataTable dt = db.run_return_datatable();
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["path_locator"] != null && !String.IsNullOrWhiteSpace(dr["path_locator"].ToString()))
                        {
                            DeleteDirectoryRecursively(tableName, dr["path_locator"].ToString());
                        }
                        else//Error output
                        {
                            throw new Exception(String.Format("Database FileTable Directory Delete: One of the directories using locator '{0}' has an empty path_locator.", locator));
                        }
                    }

                    //Removes the files in the directory
                    db.add_delete(tableName);
                    db.add_where_normal(tableName, "parent_path_locator", locator);
                    db.add_where_normal(tableName, "is_directory", 0);
                    db.add_where_normal(tableName, "is_archive", 1);
                    db.run();

                    //Removes the directory now its empty
                    db.add_delete(tableName);
                    db.add_where_normal(tableName, "path_locator", locator);
                    db.add_where_normal(tableName, "is_directory", 1);
                    db.run();
                }
                catch (Exception e)
                {
                    throw CustomErrorOutput(e);
                }
            }

            private bool FileGroupEnabled(string databaseName)
            {
                try
                {
                    //Checks if the filegroup has been activated on the database
                    db.add_pure_sql(String.Format(@"
                        --Checks if the filegroup stream is enabled
                        select (case when EXISTS(SELECT * FROM {0}.sys.database_files WHERE type_desc = 'FILESTREAM') then 1 else 0 end) as filegroup_activated
                    ", databaseName));
                    string resultOne = db.run_return_string();
                    if (!string.IsNullOrWhiteSpace(resultOne) && resultOne == "1")

                    {
                        return true;
                    }

                    return false;
                }
                catch (Exception e)
                {
                    throw CustomErrorOutput(e);
                }
            }

            private bool FileStreamEnabled(string databaseName)
            {
                try
                {
                    //Checks if the filegroup has been activated on the database
                    db.add_pure_sql(String.Format(@"
                        SELECT non_transacted_access
                        FROM sys.database_filestream_options
                        where DB_NAME(database_id) = '{0}'
                    ", databaseName));
                    string resultOne = db.run_return_string();
                    if (!string.IsNullOrWhiteSpace(resultOne) && resultOne == "2")

                    {
                        return true;
                    }

                    return false;
                }
                catch (Exception e)
                {
                    throw CustomErrorOutput(e);
                }
            }
        }
    }
}