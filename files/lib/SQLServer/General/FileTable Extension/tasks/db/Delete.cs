using System;

namespace DotNetSDB.SqlServer.FileTable
{
    public partial class SQLServerFileTableExtension
    {
		public partial class FileTableTasks
		{			
            /// <summary>
            /// This function deletes a folder from a filetable
            /// </summary>
            /// <param name="tableName"></param>
            /// <param name="streamID"></param>
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
						throw new Exception("Database FileTable Folder Delete: A path locator could not be retrieved from the stream ID.");
					}
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}

            /// <summary>
            /// This function deletes all the content in a folder in the filetable
            /// </summary>
            /// <param name="tableName"></param>
            /// <param name="streamID"></param>
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
                        throw new Exception("Database FileTable Folder Content Delete: A path locator could not be retrieved from the stream ID.");
                    }
                }
                catch (Exception e)
                {
                    throw errorHandler.CustomErrorOutput(e);
                }
            }

            /// <summary>
            /// This function deletes a file in the filetable
            /// </summary>
            /// <param name="tableName"></param>
            /// <param name="streamID"></param>
            public void delete_file(string tableName, string streamID)
			{
				try
				{
					connector.DB.add_delete(tableName);
					connector.DB.add_where_normal(tableName, "stream_id", streamID);
					connector.DB.add_where_normal(tableName, "is_directory", 0);
					connector.DB.add_where_normal(tableName, "is_archive", 1);
					connector.DB.run();
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}
		}
    }
    
}