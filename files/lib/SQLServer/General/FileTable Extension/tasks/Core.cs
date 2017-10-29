using System;
using System.Linq;
using System.Data;
using System.IO;

namespace DotNetSDB.SqlServer.FileTable
{    
    public partial class SQLServerFileTableExtension
    {
        /// <summary>
        /// This is the FileTableTasks class
        /// </summary>
		public partial class FileTableTasks
		{
            /// <summary>
            /// This function returns a name which is not in use compared to the array passed
            /// Note: this will work for both folders and files as file extension will be blank on the folder version
            /// </summary>
            /// <param name="name"></param>
            /// <param name="allNames"></param>
            /// <returns></returns>
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
                        newName = $"{currentFilename} ({index}){fileExtension}";

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

            /// <summary>
            /// This function generates a new path locator ID based on the parents path locator
            /// Note: when injecting files into a filetable via sql query this has to be done as its not auto generated
            /// </summary>
            /// <param name="tableName"></param>
            /// <param name="streamID"></param>
            /// <returns></returns>
            private string CreateNewFolderPathLocator(string tableName, string streamID)
			{
				try
				{
					//Gets the path locator for combining with the new ID
					connector.DB.add_select(tableName, "path_locator", "CAST( ", " AS varchar(max)) as path_locator");
					connector.DB.add_where_normal(tableName, "stream_id", streamID);
					connector.DB.add_where_normal(tableName, "is_directory", 1);
					string folderLocator = connector.DB.run_return_string();

					//Ensures the path locator is not empty
					if (!string.IsNullOrWhiteSpace(folderLocator))
					{
						//Creates the new ID and outputs it
						connector.DB.add_pure_sql($"select CONCAT('{folderLocator}', CONVERT(VARCHAR(20), CONVERT(BIGINT, SUBSTRING(CONVERT(BINARY(16), NEWID()), 1, 6))) +'.'+ CONVERT(VARCHAR(20), CONVERT(BIGINT, SUBSTRING(CONVERT(BINARY(16), NEWID()), 7, 6))) +'.'+ CONVERT(VARCHAR(20), CONVERT(BIGINT, SUBSTRING(CONVERT(BINARY(16), NEWID()), 13, 4))), '/') as path");
						return connector.DB.run_return_string();
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

            /// <summary>
            /// This function generates a new path locator ID based on the root folder path locator
            /// Note: when injecting files into a filetable via sql query this has to be done as its not auto generated.
            /// </summary>
            /// <param name="tableName"></param>
            /// <returns></returns>
            private string CreateNewRootPathLocator(string tableName)
            {
                try
                {   
                    //Creates the new ID for the root of the table and outputs it
                    connector.DB.add_pure_sql($"select CONCAT(GetPathLocator(FileTableRootPath('{tableName}')).ToString(), CONVERT(VARCHAR(20), CONVERT(BIGINT, SUBSTRING(CONVERT(BINARY(16), NEWID()), 1, 6))) +'.'+ CONVERT(VARCHAR(20), CONVERT(BIGINT, SUBSTRING(CONVERT(BINARY(16), NEWID()), 7, 6))) +'.'+ CONVERT(VARCHAR(20), CONVERT(BIGINT, SUBSTRING(CONVERT(BINARY(16), NEWID()), 13, 4))), '/') as path");
                    return connector.DB.run_return_string();
                }
                catch (Exception e)
                {
                    throw errorHandler.CustomErrorOutput(e);
                }
            }

            /// <summary>
            /// This function recursively deletes a folder structure and all its content within it
            /// </summary>
            /// <param name="tableName"></param>
            /// <param name="locator"></param>
            /// <param name="deleteBaseFolder"></param>
            private void DeleteFolderContentRecursively(string tableName, string locator, bool deleteBaseFolder = true)
			{
				try
				{
                    //Gets the path_locators for all the folders which are within our current one
                    connector.DB.add_select(tableName, "path_locator", "CAST( ", " AS varchar(max)) as path_locator");
					connector.DB.add_where_normal(tableName, "parent_path_locator", locator);
					connector.DB.add_where_normal(tableName, "is_directory", 1);
					DataTable dt = connector.DB.run_return_datatable();

                    //Loops over all the folders
					foreach (DataRow dr in dt.Rows)
					{
						if (dr["path_locator"] != null && !string.IsNullOrWhiteSpace(dr["path_locator"].ToString()))
						{
                            //Runs the recursive function to go to the base of the folder and then work backwards
                            DeleteFolderContentRecursively(tableName, dr["path_locator"].ToString(), true);
						}
						else//Error output
						{
							throw new Exception($"Database FileTable Directory Delete: One of the directories using locator '{locator}' has an empty path_locator.");
						}
					}

					//Removes the files in the directory
					connector.DB.add_delete(tableName);
					connector.DB.add_where_normal(tableName, "parent_path_locator", locator);
					connector.DB.add_where_normal(tableName, "is_directory", 0);
					connector.DB.add_where_normal(tableName, "is_archive", 1);
					connector.DB.run();

                    //Checks if we should remove the base folder
                    if (deleteBaseFolder)
                    {
                        //Removes the directory now its empty
                        connector.DB.add_delete(tableName);
                        connector.DB.add_where_normal(tableName, "path_locator", locator);
                        connector.DB.add_where_normal(tableName, "is_directory", 1);
                        connector.DB.run();
                    }
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}

            /// <summary>
            /// This function recursively moves a folder structure and all its content within it to a different location
            /// </summary>
            /// <param name="tableName"></param>
            /// <param name="moveLocationID"></param>
            /// <param name="folderName"></param>
            /// <param name="locator"></param>
            private void MoveFolderRecursively(string tableName, string moveLocationID, string folderName, string locator)
            {
                try
                {
                    //Creates the parent folder
                    create_folder(tableName, moveLocationID, folderName, false);

                    //Gets the new stream id for the folder
                    string newFolderID = get_folder_id(tableName, moveLocationID, folderName);

                    //Gets the name and path_locators for all the folders which are within our current one
                    connector.DB.add_select(tableName, new string[] { "name", "path_locator" },
                                                       new string[] { null, "CAST( " },
                                                       new string[] { null, " AS varchar(max)) as path_locator" });
                    connector.DB.add_where_normal(tableName, "parent_path_locator", locator);
                    connector.DB.add_where_normal(tableName, "is_directory", 1);
                    DataTable dt = connector.DB.run_return_datatable();

                    //Loops over all the folder entries
                    foreach (DataRow dr in dt.Rows)
                    {
                        //Checks the path locator exist and is not null "should never be unless something bad has happend"
                        if (dr["path_locator"] != null && !string.IsNullOrWhiteSpace(dr["path_locator"].ToString()))
                        {
                            //Runs the recursive function to go to the base of the folder and then work backwards
                            MoveFolderRecursively(tableName, newFolderID, dr["name"].ToString(), dr["path_locator"].ToString());
                        }
                        else//Error output
                        {
                            throw new Exception($"Database FileTable Update Directory Location: One of the directories using locator '{locator}' has an empty path_locator.");
                        }
                    }

                    //Gets all the file stream IDs for the current folder
                    connector.DB.add_select(tableName, "stream_id");
                    connector.DB.add_where_normal(tableName, "is_archive", 1);
                    connector.DB.add_where_normal(tableName, "parent_path_locator", locator);
                    var fileStreamIDs = connector.DB.run_return_string_array();

                    //This variable is used to determine if we are going over the threshold of query building
                    int counter = 0;

                    //Loops over all the file stream IDs and generates a new path locator ID for the new folder directory, then updates it.
                    //Note: in short this moves the file from one location to another
                    foreach (string fileID in fileStreamIDs)
                    {
                        //Tells the database library to start a new query within the same statement
                        connector.DB.start_new_query();

                        //Generates the new locator ID for that folder
                        string newID = CreateNewFolderPathLocator(tableName, newFolderID);
                        if (string.IsNullOrWhiteSpace(newID))
                        {
                            throw new Exception("Database FileTable Update Directory: The new hierarchyid could not be created.");
                        }

                        //Updates the path locator to point to the new directory
                        connector.DB.add_update(tableName, "path_locator", newID);
                        connector.DB.add_where_normal(tableName, "stream_id", fileID);

                        //Increments the counter
                        counter++;

                        //Executes the query if its hit 1000 or above
                        if (counter >= 1000)
                        {
                            //Runs the built query
                            connector.DB.run();

                            //Resets the counter
                            counter = 0;
                        }
                    }

                    //If the counter has any records then execute them
                    if (counter != 0)
                    {
                        //Runs the built query
                        connector.DB.run();
                    }

                    //Gets the old stream ID
                    connector.DB.add_select(tableName, "stream_id");
                    connector.DB.add_where_normal(tableName, "path_locator", locator);
                    string oldStreamID = connector.DB.run_return_string();

                    //Deletes the old folder and its content
                    delete_folder(tableName, oldStreamID);

                    //Trys to get a value for the locator we just deleted
                    connector.DB.add_select(tableName, "stream_id");
                    connector.DB.add_where_normal(tableName, "path_locator", locator);
                    string check = connector.DB.run_return_string();

                    //Checks the value is empty to ensure it has been deleted before switching the stream IDs
                    if(string.IsNullOrWhiteSpace(check))
                    {
                        //Updates the id back to the original version
                        connector.DB.add_update(tableName, "stream_id", oldStreamID);
                        connector.DB.add_where_normal(tableName, "stream_id", newFolderID);
                        connector.DB.run();
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