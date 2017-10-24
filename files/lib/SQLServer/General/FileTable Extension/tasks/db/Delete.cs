using System;

namespace DotNetSDB.SqlServer.FileTable
{
    public partial class SQLServerFileTableExtension
    {
		public partial class FileTableTasks
		{			
			public void delete_folder(string tableName, string streamID)
			{
				try
				{
                    string locator = get_path_locator(tableName, streamID);
                    if (!string.IsNullOrWhiteSpace(locator))
					{
						//Starts the delete recursion off
						//Note: this is recursive because a folder cannot be delete if it has folders and files within it
						DeleteFolderContentRecursively(tableName, locator);
					}
					else
					{
						throw new Exception("Database FileTable Directory Delete: A path locator could not be retrieved from the stream ID.");
					}
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}

            public void delete_folder_contents(string tableName, string streamID)
            {
                try
                {
                    string locator = get_path_locator(tableName, streamID);
                    if (!string.IsNullOrWhiteSpace(locator))
                    {
                        //Starts the delete recursion off
                        //Note: this is recursive because a folder cannot be delete if it has folders and files within it
                        DeleteFolderContentRecursively(tableName, locator, false);
                    }
                    else
                    {
                        throw new Exception("Database FileTable Directory Content Delete: A path locator could not be retrieved from the stream ID.");
                    }
                }
                catch (Exception e)
                {
                    throw errorHandler.CustomErrorOutput(e);
                }
            }

            public void delete_file(string tableName, string streamID)
			{
				try
				{
					connector.db.add_delete(tableName);
					connector.db.add_where_normal(tableName, "stream_id", streamID);
					connector.db.add_where_normal(tableName, "is_directory", 0);
					connector.db.add_where_normal(tableName, "is_archive", 1);
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