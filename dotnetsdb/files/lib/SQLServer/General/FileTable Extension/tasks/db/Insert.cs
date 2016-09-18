using System;

namespace DotNetSDB.SqlServer.FileTable
{
    public partial class SqlServerFileTableExtension
    {
		public partial class FileTableTasks
		{			
			public void create_file_at_root(string tableName, string fileName, byte[] fileData, bool incrementNameIfExists = true)
			{
				try
				{
                    //Checks if we have the increment name flag on
                    if (incrementNameIfExists)
                    {
                        //Gets all the filenames in that folder
                        var names = get_all_file_names_from_root(tableName);

                        //Gets the name we can use
                        fileName = GetNewName(fileName, names);
                    }

                    connector.db.add_insert(tableName, new string[] { "name", "file_stream" }, new object[] { fileName, fileData });
					connector.db.run();
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}

			public void create_file_in_folder(string tableName, string folderID, string fileName, byte[] fileData, bool incrementNameIfExists = true)
			{
				try
				{
					//Gets the new generated ID
					string newID = CreateNewID(tableName, folderID);
					if (string.IsNullOrWhiteSpace(newID))
					{
						throw new Exception("Database FileTable Create File At Directory: The new hierarchyid could not be created.");
					}

                    //Checks if we have the increment name flag on
                    if(incrementNameIfExists)
                    {
                        //Gets all the filenames in that folder
                        var names = get_all_file_names_from_folder(tableName, folderID);

                        //Gets the name we can use
                        fileName = GetNewName(fileName, names);
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

			public void create_root_folder(string tableName, string folderName, bool incrementNameIfExists = true)
			{
				try
				{
                    //Checks if we have the increment name flag on
                    if (incrementNameIfExists)
                    {
                        //Gets all the folder namaes in that folder
                        var names = get_all_folder_names_from_root(tableName);                        

                        //Gets the name we can use
                        folderName = GetNewName(folderName, names);
                    }

                    connector.db.add_insert(tableName, new string[] { "name", "is_directory", "is_archive" }, new object[] { folderName, 1, 0 });
					connector.db.run();
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}

			public void create_folder(string tableName, string folderID, string folderName, bool incrementNameIfExists = true)
			{
				try
				{
					//Gets the new generated ID
					string newID = CreateNewID(tableName, folderID);
					if (string.IsNullOrWhiteSpace(newID))
					{
						throw new Exception("Database FileTable Create File At Directory: The new hierarchyid could not be created.");
					}

                    //Checks if we have the increment name flag on
                    if (incrementNameIfExists)
                    {
                        //Gets all the folder namaes in that folder
                        var names = get_all_folder_names_from_folder(tableName, folderID);
                        
                        //Gets the name we can use
                        folderName = GetNewName(folderName, names);
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