using System;
using System.Data;

namespace DotNetSDB.SqlServer.FileTable
{
    public partial class SqlServerFileTableExtension
    {
		public partial class FileTableTasks
		{			
			public void update_name(string tableName, string newName, string streamID)
			{
				try
				{
                    connector.db.add_update(tableName, "name", newName);
                    connector.db.add_where_normal(tableName, "stream_id", streamID);
                    connector.db.run();
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}

            public void update_file_location_to_root(string tableName, string streamID)
            {
                try
                {
                    //Gets the name that belongs to the stream ID
                    string currentName = GetName(tableName, streamID);

                    //Checks to make sure the file does not exist at the root already
                    if (!root_file_exists(tableName, currentName))
                    {
                        //Gets the new generated ID
                        string newID = CreateNewRootPathLocator(tableName);
                        if (string.IsNullOrWhiteSpace(newID))
                        {
                            throw new Exception("Database FileTable update_files_location_to_root: The new hierarchyid could not be created.");
                        }

                        connector.db.add_update(tableName, "path_locator", newID);
                        connector.db.add_where_normal(tableName, "stream_id", streamID);
                        connector.db.run();
                    }
                }
                catch (Exception e)
                {
                    throw errorHandler.CustomErrorOutput(e);
                }
            }

            public void update_folder_location_to_root(string tableName, string streamID)
            {
                try
                {
                    //Gets the name that belongs to the stream ID
                    string currentName = GetName(tableName, streamID);

                    //Checks to make sure the folder does not exist at the root already
                    if (!root_folder_exists(tableName, currentName))
                    {
                        //Gets the required information
                        connector.db.add_select(tableName, new string[] { "name", "path_locator" },
                                                      new string[] { null, "CAST( " },
                                                      new string[] { null, " AS varchar(max)) as path_locator" });
                        connector.db.add_where_normal(tableName, "stream_id", streamID);
                        DataTable results = connector.db.run_return_datatable();

                        //Checks we were able to obtain the required information for setting the process off
                        if (results != null && results.Rows.Count > 0)
                        {
                            //Sets up the required variables
                            string name = results.Rows[0]["name"].ToString();
                            string locator = results.Rows[0]["path_locator"].ToString();

                            //Creates the parent folder
                            create_root_folder(tableName, name, false);

                            //Gets the new stream id for the folder
                            string newFolderID = get_root_folder_id(tableName, name);

                            //Gets the name and path_locators for all the folders which are within our current one
                            connector.db.add_select(tableName, new string[] { "name", "path_locator" },
                                                               new string[] { null, "CAST( " },
                                                               new string[] { null, " AS varchar(max)) as path_locator" });
                            connector.db.add_where_normal(tableName, "parent_path_locator", locator);
                            connector.db.add_where_normal(tableName, "is_directory", 1);
                            DataTable dt = connector.db.run_return_datatable();

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
                            connector.db.add_select(tableName, "stream_id");
                            connector.db.add_where_normal(tableName, "is_archive", 1);
                            connector.db.add_where_normal(tableName, "parent_path_locator", locator);
                            var fileStreamIDs = connector.db.run_return_string_array();
                            
                            //This variable is used to determine if we are going over the threshold of query building
                            int counter = 0;

                            //Loops over all the file stream IDs and generates a new path locator ID for the new folder directory, then updates it.
                            //Note: in short this moves the file from one location to another
                            foreach (string fileID in fileStreamIDs)
                            {
                                //Tells the database library to start a new query within the same statement
                                connector.db.start_new_query();

                                //Generates the new locator ID for that folder
                                string newID = CreateNewFolderPathLocator(tableName, newFolderID);
                                if (string.IsNullOrWhiteSpace(newID))
                                {
                                    throw new Exception("Database FileTable Update Directory: The new hierarchyid could not be created.");
                                }

                                //Updates the path locator to point to the new directory
                                connector.db.add_update(tableName, "path_locator", newID);
                                connector.db.add_where_normal(tableName, "stream_id", fileID);
                                
                                //Increments the counter
                                counter++;

                                //Executes the query if its hit 1000 or above
                                if (counter >= 1000)
                                {
                                    //Runs the built query
                                    connector.db.run();

                                    //Resets the counter
                                    counter = 0;
                                }
                            }

                            //If the counter has any records then execute them
                            if(counter != 0)
                            {
                                //Runs the built query
                                connector.db.run();
                            }

                            //Gets the old stream ID
                            connector.db.add_select(tableName, "stream_id");
                            connector.db.add_where_normal(tableName, "path_locator", locator);
                            string oldStreamID = connector.db.run_return_string();

                            //Deletes the old folder and its content
                            delete_folder(tableName, oldStreamID);

                            //Trys to get a value for the locator we just deleted
                            connector.db.add_select(tableName, "stream_id");
                            connector.db.add_where_normal(tableName, "path_locator", locator);
                            string check = connector.db.run_return_string();

                            //Checks the value is empty to ensure it has been deleted before switching the stream IDs
                            if (string.IsNullOrWhiteSpace(check))
                            {
                                //Updates the id back to the original version
                                connector.db.add_update(tableName, "stream_id", oldStreamID);
                                connector.db.add_where_normal(tableName, "stream_id", newFolderID);
                                connector.db.run();
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    throw errorHandler.CustomErrorOutput(e);
                }
            }
            
            public void update_file_location(string tableName, string newFolderID, string streamID)
            {
                try
                {
                    //Gets the name that belongs to the stream ID
                    string currentName = GetName(tableName, streamID);

                    //Checks to make sure the file does not exist in the new folder location already
                    if (!file_exists(tableName, newFolderID, currentName))
                    {
                        //Gets the new generated ID
                        string newID = CreateNewFolderPathLocator(tableName, newFolderID);
                        if (string.IsNullOrWhiteSpace(newID))
                        {
                            throw new Exception("Database FileTable Update File Directory: The new hierarchyid could not be created.");
                        }

                        connector.db.add_update(tableName, "path_locator", newID);
                        connector.db.add_where_normal(tableName, "stream_id", streamID);
                        connector.db.run();
                    }
                }
                catch (Exception e)
                {
                    throw errorHandler.CustomErrorOutput(e);
                }
            }
            
            public void update_folder_location(string tableName, string newFolderID, string streamID)
            {
                try
                {
                    //Gets the name that belongs to the stream ID
                    string currentName = GetName(tableName, streamID);

                    //Checks to make sure the folder does not exist in the new folder location already
                    if (!folder_exists(tableName, newFolderID, currentName))
                    {
                        connector.db.add_select(tableName, new string[] { "name", "path_locator" },
                                                       new string[] { null, "CAST( " },
                                                       new string[] { null, " AS varchar(max)) as path_locator" });
                        connector.db.add_where_normal(tableName, "stream_id", streamID);
                        DataTable results = connector.db.run_return_datatable();

                        if (results != null && results.Rows.Count > 0)
                        {
                            MoveFolderRecursively(tableName, newFolderID, results.Rows[0]["name"].ToString(), results.Rows[0]["path_locator"].ToString());
                        }
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