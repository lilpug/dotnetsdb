using System;

namespace DotNetSDB.SqlServer.FileTable
{
    public partial class SqlServerFileTableExtension
    {
		public partial class FileTableTasks
		{			
			public void create_file_at_root(string tableName, string fileName, byte[] fileData)
			{
				try
				{
					connector.db.add_insert(tableName, new string[] { "name", "file_stream" }, new object[] { fileName, fileData });
					connector.db.run();
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}

			public void create_file_in_folder(string tableName, string directoryID, string fileName, byte[] fileData)
			{
				try
				{
					//Gets the new generated ID
					string newID = CreateNewID(tableName, directoryID);
					if (string.IsNullOrWhiteSpace(newID))
					{
						throw new Exception("Database FileTable Create File At Directory: The new hierarchyid could not be created.");
					}

					//Inserts the file into the directory
					connector.db.add_insert(tableName, new string[] { "name", "file_stream", "path_locator" }, new object[] { fileName, fileData, newID });
					connector.db.run();
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}

			public void create_root_folder(string tableName, string folderName)
			{
				try
				{
					connector.db.add_insert(tableName, new string[] { "name", "is_directory", "is_archive" }, new object[] { folderName, 1, 0 });
					connector.db.run();
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}

			public void create_folder(string tableName, string parentDirectoryID, string folderName)
			{
				try
				{
					//Gets the new generated ID
					string newID = CreateNewID(tableName, parentDirectoryID);
					if (string.IsNullOrWhiteSpace(newID))
					{
						throw new Exception("Database FileTable Create File At Directory: The new hierarchyid could not be created.");
					}

					//Creates the new directory within the supplied directory
					connector.db.add_insert(tableName, new string[] { "name", "is_directory", "is_archive", "path_locator" }, new object[] { folderName, 1, 0, newID });
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