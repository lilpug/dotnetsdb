using System;

namespace DotNetSDB.SqlServer.FileTable
{
    public partial class SQLServerFileTableExtension
    {
		public partial class FileTableTasks
		{			
			public bool id_exists(string tableName, string streamID)
			{
				try
				{
					connector.db.add_select(tableName, "stream_id");
					connector.db.add_where_normal(tableName, "stream_id", streamID);
					string id = connector.db.run_return_string();
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

            public bool root_file_exists(string tableName, string fileName)
            {
                string value = get_root_file_id(tableName, fileName);
                if(!string.IsNullOrWhiteSpace(value))
                {
                    return true;
                }
                return false;
            }

            public bool file_exists(string tableName, string parentFolderID, string fileName)
            {
                string value = get_file_id(tableName, parentFolderID, fileName);
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return true;
                }
                return false;
            }
            
            public bool root_folder_exists(string tableName, string folderName)
            {
                string value = get_root_folder_id(tableName, folderName);
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return true;
                }
                return false;
            }

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