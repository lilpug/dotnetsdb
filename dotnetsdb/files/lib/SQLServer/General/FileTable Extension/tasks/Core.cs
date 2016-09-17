using System;
using System.Data;

namespace DotNetSDB.SqlServer.FileTable
{    
    public partial class SqlServerFileTableExtension
    {
		public partial class FileTableTasks
		{			
			private string CreateNewID(string tableName, string streamID)
			{
				try
				{
					//Gets the path locator for combining with the new ID
					connector.db.add_select(tableName, "path_locator", "CAST( ", " AS varchar(max)) as path_locator");
					connector.db.add_where_normal(tableName, "stream_id", streamID);
					connector.db.add_where_normal(tableName, "is_directory", 1);
					string folderLocator = connector.db.run_return_string();

					//Ensures the path locator is not empty
					if (!string.IsNullOrWhiteSpace(folderLocator))
					{
						//Creates the new ID and outputs it
						connector.db.add_pure_sql(String.Format("select CONCAT('{0}', CONVERT(VARCHAR(20), CONVERT(BIGINT, SUBSTRING(CONVERT(BINARY(16), NEWID()), 1, 6))) +'.'+ CONVERT(VARCHAR(20), CONVERT(BIGINT, SUBSTRING(CONVERT(BINARY(16), NEWID()), 7, 6))) +'.'+ CONVERT(VARCHAR(20), CONVERT(BIGINT, SUBSTRING(CONVERT(BINARY(16), NEWID()), 13, 4))), '/') as path", folderLocator));
						return connector.db.run_return_string();
					}
					else
					{
						throw new Exception("Database FileTable Create: The stream_id you have supplied does not exist.");
					}
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}

			private void DeleteDirectoryRecursively(string tableName, string locator)
			{
				try
				{
					connector.db.add_select(tableName, "path_locator", "CAST( ", " AS varchar(max)) as path_locator");
					connector.db.add_where_normal(tableName, "parent_path_locator", locator);
					connector.db.add_where_normal(tableName, "is_directory", 1);
					DataTable dt = connector.db.run_return_datatable();
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
					connector.db.add_delete(tableName);
					connector.db.add_where_normal(tableName, "parent_path_locator", locator);
					connector.db.add_where_normal(tableName, "is_directory", 0);
					connector.db.add_where_normal(tableName, "is_archive", 1);
					connector.db.run();

					//Removes the directory now its empty
					connector.db.add_delete(tableName);
					connector.db.add_where_normal(tableName, "path_locator", locator);
					connector.db.add_where_normal(tableName, "is_directory", 1);
					connector.db.run();
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}  
		}				
    }
    
}