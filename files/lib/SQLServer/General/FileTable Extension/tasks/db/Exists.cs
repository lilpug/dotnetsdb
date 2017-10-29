using System;

namespace DotNetSDB.SqlServer.FileTable
{
    public partial class SQLServerFileTableExtension
    {
		public partial class FileTableTasks
		{			
            /// <summary>
            /// This function checks if the following ID exists in the filetable
            /// </summary>
            /// <param name="tableName"></param>
            /// <param name="streamID"></param>
            /// <returns></returns>
			public bool id_exists(string tableName, string streamID)
			{
				try
				{
					connector.DB.add_select(tableName, "stream_id");
					connector.DB.add_where_normal(tableName, "stream_id", streamID);
					string id = connector.DB.run_return_string();
					if (!string.IsNullOrWhiteSpace(id))
					{
						return true;
					}
					else
					{
						return false;
					}
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}

            /// <summary>
            /// This function checks if a filename exist at the root level of a filetable
            /// </summary>
            /// <param name="tableName"></param>
            /// <param name="fileName"></param>
            /// <returns></returns>
            public bool root_file_exists(string tableName, string fileName)
            {
                string value = get_root_file_id(tableName, fileName);
                if(!string.IsNullOrWhiteSpace(value))
                {
                    return true;
                }
                return false;
            }

            /// <summary>
            /// This function checks if a file exists in a folder within the filetable
            /// </summary>
            /// <param name="tableName"></param>
            /// <param name="parentFolderID"></param>
            /// <param name="fileName"></param>
            /// <returns></returns>
            public bool file_exists(string tableName, string parentFolderID, string fileName)
            {
                string value = get_file_id(tableName, parentFolderID, fileName);
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return true;
                }
                return false;
            }
            
            /// <summary>
            /// This function checks if a folder exists at the root of the filetable
            /// </summary>
            /// <param name="tableName"></param>
            /// <param name="folderName"></param>
            /// <returns></returns>
            public bool root_folder_exists(string tableName, string folderName)
            {
                string value = get_root_folder_id(tableName, folderName);
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return true;
                }
                return false;
            }

            /// <summary>
            /// This function checks if a folder exists within another folder within a filetable
            /// </summary>
            /// <param name="tableName"></param>
            /// <param name="parentFolderID"></param>
            /// <param name="folderName"></param>
            /// <returns></returns>
            public bool folder_exists(string tableName, string parentFolderID, string folderName)
            {
                string value = get_folder_id(tableName, parentFolderID, folderName);
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return true;
                }
                return false;
            }
        }
    }
    
}