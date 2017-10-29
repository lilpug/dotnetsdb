using System;
using System.Collections.Generic;
using System.Data;

namespace DotNetSDB.SqlServer.FileTable
{    
    public partial class SQLServerFileTableExtension
    {
		public partial class FileTableTasks
		{
            //Steam Ids

            /// <summary>
            /// This function gets the name for a supplied id in the filetable
            /// </summary>
            /// <param name="tableName"></param>
            /// <param name="streamID"></param>
            /// <returns></returns>
            public string get_name(string tableName, string streamID)
            {
                try
                { 
                    //Checks if the folder already 
                    connector.DB.add_select(tableName, "name");
                    connector.DB.add_where_normal(tableName, "stream_id", streamID);
                    return connector.DB.run_return_string();
                }
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
            }

            /// <summary>
            /// This function gets the id for a folder in the root of the filetable
            /// </summary>
            /// <param name="tableName"></param>
            /// <param name="folderName"></param>
            /// <returns></returns>
			public string get_root_folder_id(string tableName, string folderName)
			{
				try
				{
					if (!string.IsNullOrWhiteSpace(folderName))
					{
						connector.DB.add_select(tableName, "stream_id");
						connector.DB.add_where_normal(tableName, "name", folderName);
						connector.DB.add_where_normal(tableName, "is_directory", 1);
						connector.DB.add_where_is(tableName, "parent_path_locator");
						return connector.DB.run_return_string();
					}

					return null;
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}

            /// <summary>
            /// This function gets the id for a file in the root of the filetable
            /// </summary>
            /// <param name="tableName"></param>
            /// <param name="fileName"></param>
            /// <returns></returns>
			public string get_root_file_id(string tableName, string fileName)
			{
				try
				{
					if (!string.IsNullOrWhiteSpace(fileName))
					{
						connector.DB.add_select(tableName, "stream_id");
						connector.DB.add_where_normal(tableName, "name", fileName);
						connector.DB.add_where_normal(tableName, "is_archive", 1);
						connector.DB.add_where_is(tableName, "parent_path_locator");
						return connector.DB.run_return_string();
					}

					return null;
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}

            /// <summary>
            /// This function gets a folder id from within another folder in the filetable
            /// </summary>
            /// <param name="tableName"></param>
            /// <param name="parentFolderID"></param>
            /// <param name="folderName"></param>
            /// <returns></returns>
			public string get_folder_id(string tableName, string parentFolderID, string folderName)
			{
				try
				{
					if (!string.IsNullOrWhiteSpace(parentFolderID))
					{
						string locator = get_path_locator(tableName, parentFolderID);
						if (!string.IsNullOrWhiteSpace(locator))
						{
							connector.DB.add_select(tableName, "stream_id");
							connector.DB.add_where_normal(tableName, "name", folderName);
							connector.DB.add_where_normal(tableName, "is_directory", 1);
							connector.DB.add_where_normal(tableName, "parent_path_locator", locator);
							return connector.DB.run_return_string();
						}
					}

					return null;
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}

            /// <summary>
            /// This function gets an id for a file within a folder in the filetable
            /// </summary>
            /// <param name="tableName"></param>
            /// <param name="parentFolderID"></param>
            /// <param name="fileName"></param>
            /// <returns></returns>
			public string get_file_id(string tableName, string parentFolderID, string fileName)
			{
				try
				{
					if (!string.IsNullOrWhiteSpace(parentFolderID))
					{
						string locator = get_path_locator(tableName, parentFolderID);
						if (!string.IsNullOrWhiteSpace(locator))
						{
							connector.DB.add_select(tableName, "stream_id");
							connector.DB.add_where_normal(tableName, "name", fileName);
							connector.DB.add_where_normal(tableName, "is_archive", 1);
							connector.DB.add_where_normal(tableName, "parent_path_locator", locator);
							return connector.DB.run_return_string();
						}
					}
					return null;
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}
            
            
            //File Data

            /// <summary>
            /// This function returns a dictionary of all files and their data from the root of the filetable
            /// </summary>
            /// <param name="tableName"></param>
            /// <returns></returns>
			public Dictionary<string, byte[]> get_all_files_from_root(string tableName)
			{
				try
				{
					connector.DB.add_select(tableName, new string[] { "name", "file_stream" });
					connector.DB.add_where_normal(tableName, "is_archive", 1);
                    connector.DB.add_where_is(tableName, "parent_path_locator");
                    DataTable results = connector.DB.run_return_datatable();

					if (results != null && results.Rows.Count > 0)
					{
						Dictionary<string, byte[]> files = new Dictionary<string, byte[]>();
						foreach (DataRow dr in results.Rows)
						{
							files.Add(dr["name"].ToString(), (byte[])dr["file_stream"]);
						}

						return files;
					}

					return null;
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}

            /// <summary>
            /// This function returns a dictionary of all files and their data from a folder in the filetable
            /// </summary>
            /// <param name="tableName"></param>
            /// <param name="folderID"></param>
            /// <returns></returns>
			public Dictionary<string, byte[]> get_all_files_from_folder(string tableName, string folderID)
			{
				try
				{
					if (!string.IsNullOrWhiteSpace(folderID))
					{
						string pathLocator = get_path_locator(tableName, folderID);

						connector.DB.add_select(tableName, new string[] { "name", "file_stream" });
                        connector.DB.add_where_normal(tableName, "is_archive", 1);
                        connector.DB.add_where_normal(tableName, "parent_path_locator", pathLocator);
						DataTable results = connector.DB.run_return_datatable();

						if (results != null && results.Rows.Count > 0)
						{
							Dictionary<string, byte[]> files = new Dictionary<string, byte[]>();
							foreach (DataRow dr in results.Rows)
							{
								files.Add(dr["name"].ToString(), (byte[])dr["file_stream"]);
							}

							return files;
						}
					}

					return null;
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}

            /// <summary>
            /// This function returns a dictionary of a file and its data from an id in the filetable
            /// </summary>
            /// <param name="tableName"></param>
            /// <param name="fileID"></param>
            /// <returns></returns>
			public Dictionary<string, byte[]> get_file(string tableName, string fileID)
			{
				try
				{
					if (!string.IsNullOrWhiteSpace(fileID))
					{
						connector.DB.add_select(tableName, new string[] { "name", "file_stream" });
						connector.DB.add_where_normal(tableName, "stream_id", fileID);
						connector.DB.add_where_normal(tableName, "is_archive", 1);
						DataTable results = connector.DB.run_return_datatable();

						if (results != null && results.Rows.Count > 0)
						{
							Dictionary<string, byte[]> files = new Dictionary<string, byte[]>();
							foreach (DataRow dr in results.Rows)
							{
								files.Add(dr["name"].ToString(), (byte[])dr["file_stream"]);
							}

							return files;
						}
					}

					return null;
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}



            /// <summary>
            /// This function gets the main path locator for an id in the filetable
            /// </summary>
            /// <param name="tableName"></param>
            /// <param name="streamID"></param>
            /// <returns></returns>
            public string get_path_locator(string tableName, string streamID)
            {
                try
                {
                    connector.DB.add_select(tableName, "path_locator", "CAST( ", " AS varchar(max)) as path_locator");
                    connector.DB.add_where_normal(tableName, "stream_id", streamID);
                    return connector.DB.run_return_string();
                }
                catch (Exception e)
                {
                    throw errorHandler.CustomErrorOutput(e);
                }
            }


            //Get methods to retreive the names

            /// <summary>
            /// This function returns an array of all the filenames in the root of the filetable
            /// </summary>
            /// <param name="tableName"></param>
            /// <returns></returns>
            public string[] get_all_file_names_from_root(string tableName)
            {
                try
                {
                    connector.DB.add_select(tableName, "name");
                    connector.DB.add_where_normal(tableName, "is_archive", 1);
                    connector.DB.add_where_is(tableName, "parent_path_locator");
                    return connector.DB.run_return_string_array();  
                }
                catch (Exception e)
                {
                    throw errorHandler.CustomErrorOutput(e);
                }
            }

            /// <summary>
            /// This function returns an array of all the filenames from within a folder in the root of the filetable
            /// </summary>
            /// <param name="tableName"></param>
            /// <param name="folderID"></param>
            /// <returns></returns>
            public string[] get_all_file_names_from_folder(string tableName, string folderID)
            {
                try
                {
                    string pathLocator = get_path_locator(tableName, folderID);

                    connector.DB.add_select(tableName, "name");
                    connector.DB.add_where_normal(tableName, "is_archive", 1);
                    connector.DB.add_where_normal(tableName, "parent_path_locator", pathLocator);
                    return connector.DB.run_return_string_array();
                }
                catch (Exception e)
                {
                    throw errorHandler.CustomErrorOutput(e);
                }
            }
            
            /// <summary>
            /// This function returns an array of all the folder names from the root of the filetable
            /// </summary>
            /// <param name="tableName"></param>
            /// <returns></returns>
            public string[] get_all_folder_names_from_root(string tableName)
            {
                try
                {
                    connector.DB.add_select(tableName, "name");
                    connector.DB.add_where_normal(tableName, "is_directory", 1);
                    connector.DB.add_where_is(tableName, "parent_path_locator");
                    return connector.DB.run_return_string_array();
                }
                catch (Exception e)
                {
                    throw errorHandler.CustomErrorOutput(e);
                }
            }

            /// <summary>
            /// This function returns an array of all the folder names from within a folder in the root of the filetable
            /// </summary>
            /// <param name="tableName"></param>
            /// <param name="folderID"></param>
            /// <returns></returns>
            public string[] get_all_folder_names_from_folder(string tableName, string folderID)
            {
                try
                {
                    string pathLocator = get_path_locator(tableName, folderID);

                    connector.DB.add_select(tableName, "name");
                    connector.DB.add_where_normal(tableName, "is_directory", 1);
                    connector.DB.add_where_normal(tableName, "parent_path_locator", pathLocator);
                    return connector.DB.run_return_string_array();
                }
                catch (Exception e)
                {
                    throw errorHandler.CustomErrorOutput(e);
                }
            }




            //Main get methods to retreive all data

            /// <summary>
            /// This function obtains all the filetable entries at the root level
            /// </summary>
            /// <param name="tableName"></param>
            /// <returns></returns>
			public DataTable get_all_from_root(string tableName)
			{
				try
				{
					connector.DB.add_pure_sql($@"
							SELECT
								stream_id
								,file_stream
								,name

								--This is required because api output data does not understand the column types
								,CAST(path_locator as varchar(max)) as path_locator
								,CAST(parent_path_locator as varchar(max)) as parent_path_locator

								,file_type
								,cached_file_size
								,creation_time
								,last_write_time
								,last_access_time
								,is_directory
								,is_offline
								,is_hidden
								,is_readonly
								,is_archive
								,is_system
								,is_temporary
							FROM {tableName}");
                    connector.DB.add_where_is(tableName, "parent_path_locator");
                    return connector.DB.run_return_datatable();
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}

            /// <summary>
            /// This function obtains all the filetable entries for a folder id
            /// </summary>
            /// <param name="tableName"></param>
            /// <param name="folderID"></param>
            /// <returns></returns>
			public DataTable get_all_from_folder(string tableName, string folderID)
			{
				try
				{
					if (!string.IsNullOrWhiteSpace(folderID))
					{
						string pathLocator = get_path_locator(tableName, folderID);

						connector.DB.add_pure_sql($@"
							SELECT
								stream_id
								,file_stream
								,name

								--This is required because api output data does not understand the column types
								,CAST(path_locator as varchar(max)) as path_locator
								,CAST(parent_path_locator as varchar(max)) as parent_path_locator

								,file_type
								,cached_file_size
								,creation_time
								,last_write_time
								,last_access_time
								,is_directory
								,is_offline
								,is_hidden
								,is_readonly
								,is_archive
								,is_system
								,is_temporary
							FROM {tableName}");
						connector.DB.add_where_normal(tableName, "parent_path_locator", pathLocator);
						return connector.DB.run_return_datatable();
					}

					return null;
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}

            /// <summary>
            /// This function obtains all the file entries from the root level of the filetable
            /// </summary>
            /// <param name="tableName"></param>
            /// <returns></returns>
			public DataTable get_all_files_from_root_dt(string tableName)
			{
				try
				{
					connector.DB.add_pure_sql($@"
							SELECT
								stream_id
								,file_stream
								,name

								--This is required because api output data does not understand the column types
								,CAST(path_locator as varchar(max)) as path_locator
								,CAST(parent_path_locator as varchar(max)) as parent_path_locator

								,file_type
								,cached_file_size
								,creation_time
								,last_write_time
								,last_access_time
								,is_directory
								,is_offline
								,is_hidden
								,is_readonly
								,is_archive
								,is_system
								,is_temporary
							FROM {tableName}");
					connector.DB.add_where_normal(tableName, "is_archive", 1);
                    connector.DB.add_where_is(tableName, "parent_path_locator");
                    return connector.DB.run_return_datatable();
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}

            /// <summary>
            /// This function obtains all the file entries from a folder id in the filetable
            /// </summary>
            /// <param name="tableName"></param>
            /// <param name="folderID"></param>
            /// <returns></returns>
			public DataTable get_all_files_from_folder_dt(string tableName, string folderID)
			{
				try
				{
					if (!string.IsNullOrWhiteSpace(folderID))
					{
						string pathLocator = get_path_locator(tableName, folderID);

						connector.DB.add_pure_sql($@"
							SELECT
								stream_id
								,file_stream
								,name

								--This is required because api output data does not understand the column types
								,CAST(path_locator as varchar(max)) as path_locator
								,CAST(parent_path_locator as varchar(max)) as parent_path_locator

								,file_type
								,cached_file_size
								,creation_time
								,last_write_time
								,last_access_time
								,is_directory
								,is_offline
								,is_hidden
								,is_readonly
								,is_archive
								,is_system
								,is_temporary
							FROM {tableName}");
						connector.DB.add_where_normal(tableName, "is_archive", 1);
						connector.DB.add_where_normal(tableName, "parent_path_locator", pathLocator);
						return connector.DB.run_return_datatable();
					}

					return null;
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}

            /// <summary>
            /// This function obtains all the folder entries from the root in the filetable
            /// </summary>
            /// <param name="tableName"></param>
            /// <returns></returns>
            public DataTable get_all_folders_from_root_dt(string tableName)
            {
                try
                {
                    connector.DB.add_pure_sql($@"
							SELECT
								stream_id
								,file_stream
								,name

								--This is required because api output data does not understand the column types
								,CAST(path_locator as varchar(max)) as path_locator
								,CAST(parent_path_locator as varchar(max)) as parent_path_locator

								,file_type
								,cached_file_size
								,creation_time
								,last_write_time
								,last_access_time
								,is_directory
								,is_offline
								,is_hidden
								,is_readonly
								,is_archive
								,is_system
								,is_temporary
							FROM {tableName}");
                    connector.DB.add_where_normal(tableName, "is_directory", 1);
                    connector.DB.add_where_is(tableName, "parent_path_locator");
                    return connector.DB.run_return_datatable();
                }
                catch (Exception e)
                {
                    throw errorHandler.CustomErrorOutput(e);
                }
            }
            
            /// <summary>
            /// This function obtains all the folder entries from a folder id in the filetable
            /// </summary>
            /// <param name="tableName"></param>
            /// <param name="folderID"></param>
            /// <returns></returns>
			public DataTable get_all_folders_from_folder(string tableName, string folderID)
			{
				try
				{
					if (!string.IsNullOrWhiteSpace(folderID))
					{
						string pathLocator = get_path_locator(tableName, folderID);

						connector.DB.add_pure_sql($@"
							SELECT
								stream_id
								,file_stream
								,name

								--This is required because api output data does not understand the column types
								,CAST(path_locator as varchar(max)) as path_locator
								,CAST(parent_path_locator as varchar(max)) as parent_path_locator

								,file_type
								,cached_file_size
								,creation_time
								,last_write_time
								,last_access_time
								,is_directory
								,is_offline
								,is_hidden
								,is_readonly
								,is_archive
								,is_system
								,is_temporary
							FROM {tableName}");
						connector.DB.add_where_normal(tableName, "is_directory", 1);
						connector.DB.add_where_normal(tableName, "parent_path_locator", pathLocator);
						return connector.DB.run_return_datatable();
					}

					return null;
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}
		}
    }
    
}