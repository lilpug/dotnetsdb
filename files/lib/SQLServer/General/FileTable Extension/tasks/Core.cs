using System;
using System.Linq;
using System.Data;
using System.IO;

namespace DotNetSDB.SqlServer.FileTable
{    
    public partial class SqlServerFileTableExtension
    {
		public partial class FileTableTasks
		{
            //This function returns a name which is not in use compared to the array passed
            //Note: this will work for both folders and files as file extension will be blank on the folder version
            private string GetNewName(string name, string[] allNames)
            {
                //Seperates the filename and extension for help building the new filename
                //Note: on folder version this will not be an issue as the extension will return as blank "" so it will work fine.
                string currentFilename = Path.GetFileNameWithoutExtension(name);
                string fileExtension = Path.GetExtension(name);

                //Checks the list is not empty and if it already contains the name we are trying to put in
                if (allNames != null && allNames.Contains(name))
                {
                    //Holds the temporary new name
                    string newName = "";

                    //Variables used to process the loop status and index
                    bool status = false;
                    int index = 1;

                    //Continues to loop till it finds one that does not exist
                    while (!status)
                    {
                        //Formats the new name with the new index increment
                        newName = string.Format("{0} ({1}){2}", currentFilename, index, fileExtension);

                        //Checks if it exists in the filelist
                        if (!allNames.Contains(newName))
                        {
                            //If not we have found the name we can use so set the status to true and come out the loop
                            status = true;
                        }

                        //Increments the index
                        index++;
                    }

                    //Sets the new name
                    name = newName;
                }

                return name;
            }

            //This function generates a new path locator ID based on the parents path locator
            //Note: when injecting files into a filetable via sql query this has to be done as its not auto generated.
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
						connector.db.add_pure_sql(string.Format("select CONCAT('{0}', CONVERT(VARCHAR(20), CONVERT(BIGINT, SUBSTRING(CONVERT(BINARY(16), NEWID()), 1, 6))) +'.'+ CONVERT(VARCHAR(20), CONVERT(BIGINT, SUBSTRING(CONVERT(BINARY(16), NEWID()), 7, 6))) +'.'+ CONVERT(VARCHAR(20), CONVERT(BIGINT, SUBSTRING(CONVERT(BINARY(16), NEWID()), 13, 4))), '/') as path", folderLocator));
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

            //This function recursively deletes a folder structure and all its content within it
			private void DeleteFolderContentRecursively(string tableName, string locator, bool deleteBaseFolder = true)
			{
				try
				{
					connector.db.add_select(tableName, "path_locator", "CAST( ", " AS varchar(max)) as path_locator");
					connector.db.add_where_normal(tableName, "parent_path_locator", locator);
					connector.db.add_where_normal(tableName, "is_directory", 1);
					DataTable dt = connector.db.run_return_datatable();
					foreach (DataRow dr in dt.Rows)
					{
						if (dr["path_locator"] != null && !string.IsNullOrWhiteSpace(dr["path_locator"].ToString()))
						{
                            //The deleteBaseFolder should be true for all recursions other than the start thread
							DeleteFolderContentRecursively(tableName, dr["path_locator"].ToString(), true);
						}
						else//Error output
						{
							throw new Exception(string.Format("Database FileTable Directory Delete: One of the directories using locator '{0}' has an empty path_locator.", locator));
						}
					}

					//Removes the files in the directory
					connector.db.add_delete(tableName);
					connector.db.add_where_normal(tableName, "parent_path_locator", locator);
					connector.db.add_where_normal(tableName, "is_directory", 0);
					connector.db.add_where_normal(tableName, "is_archive", 1);
					connector.db.run();

                    //Checks if we should remove the base folder
                    if (deleteBaseFolder)
                    {
                        //Removes the directory now its empty
                        connector.db.add_delete(tableName);
                        connector.db.add_where_normal(tableName, "path_locator", locator);
                        connector.db.add_where_normal(tableName, "is_directory", 1);
                        connector.db.run();
                    }
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}  
		}				
    }
    
}