using System;
using System.Collections.Generic;
using System.Data;

namespace DotNetSDB.SqlServer.FileTable
{    
    public partial class SqlServerFileTableExtension
    {
		public partial class FileTableTasks
		{
            //Steam Ids

			public string get_root_folder_id(string tableName, string folderName)
			{
				try
				{
					if (!string.IsNullOrWhiteSpace(folderName))
					{
						connector.db.add_select(tableName, "stream_id");
						connector.db.add_where_normal(tableName, "name", folderName);
						connector.db.add_where_normal(tableName, "is_directory", 1);
						connector.db.add_where_is(tableName, "parent_path_locator");
						return connector.db.run_return_string();
					}

					return null;
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}

			public string get_root_file_id(string tableName, string fileName)
			{
				try
				{
					if (!string.IsNullOrWhiteSpace(fileName))
					{
						connector.db.add_select(tableName, "stream_id");
						connector.db.add_where_normal(tableName, "name", fileName);
						connector.db.add_where_normal(tableName, "is_archive", 1);
						connector.db.add_where_is(tableName, "parent_path_locator");
						return connector.db.run_return_string();
					}

					return null;
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}

			public string get_folder_id(string tableName, string parentFolderID, string folderName)
			{
				try
				{
					if (!string.IsNullOrWhiteSpace(parentFolderID))
					{
						string locator = get_path_locator(tableName, parentFolderID);
						if (!string.IsNullOrWhiteSpace(locator))
						{
							connector.db.add_select(tableName, "stream_id");
							connector.db.add_where_normal(tableName, "name", folderName);
							connector.db.add_where_normal(tableName, "is_directory", 1);
							connector.db.add_where_normal(tableName, "parent_path_locator", locator);
							return connector.db.run_return_string();
						}
					}

					return null;
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}

			public string get_file_id(string tableName, string parentFolderID, string fileName)
			{
				try
				{
					if (!string.IsNullOrWhiteSpace(parentFolderID))
					{
						string locator = get_path_locator(tableName, parentFolderID);
						if (!string.IsNullOrWhiteSpace(locator))
						{
							connector.db.add_select(tableName, "stream_id");
							connector.db.add_where_normal(tableName, "name", fileName);
							connector.db.add_where_normal(tableName, "is_archive", 1);
							connector.db.add_where_normal(tableName, "parent_path_locator", locator);
							return connector.db.run_return_string();
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

			public Dictionary<string, byte[]> get_all_files_from_root(string tableName)
			{
				try
				{
					connector.db.add_select(tableName, new string[] { "name", "file_stream" });
					connector.db.add_where_normal(tableName, "is_archive", 1);
                    connector.db.add_where_is(tableName, "parent_path_locator");
                    DataTable results = connector.db.run_return_datatable();

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

			public Dictionary<string, byte[]> get_all_files_from_folder(string tableName, string folderID)
			{
				try
				{
					if (!string.IsNullOrWhiteSpace(folderID))
					{
						string pathLocator = get_path_locator(tableName, folderID);

						connector.db.add_select(tableName, new string[] { "name", "file_stream" });
                        connector.db.add_where_normal(tableName, "is_archive", 1);
                        connector.db.add_where_normal(tableName, "parent_path_locator", pathLocator);
						DataTable results = connector.db.run_return_datatable();

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

			public Dictionary<string, byte[]> get_file(string tableName, string fileID)
			{
				try
				{
					if (!string.IsNullOrWhiteSpace(fileID))
					{
						connector.db.add_select(tableName, new string[] { "name", "file_stream" });
						connector.db.add_where_normal(tableName, "stream_id", fileID);
						connector.db.add_where_normal(tableName, "is_archive", 1);
						DataTable results = connector.db.run_return_datatable();

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



            //Gets the main path locator for a stream id
            public string get_path_locator(string tableName, string streamID)
            {
                try
                {
                    connector.db.add_select(tableName, "path_locator", "CAST( ", " AS varchar(max)) as path_locator");
                    connector.db.add_where_normal(tableName, "stream_id", streamID);
                    return connector.db.run_return_string();
                }
                catch (Exception e)
                {
                    throw errorHandler.CustomErrorOutput(e);
                }
            }


            //Get methods to retreive the names

            public string[] get_all_file_names_from_root(string tableName)
            {
                try
                {
                    connector.db.add_select(tableName, "name");
                    connector.db.add_where_normal(tableName, "is_archive", 1);
                    connector.db.add_where_is(tableName, "parent_path_locator");
                    return connector.db.run_return_array();                    
                }
                catch (Exception e)
                {
                    throw errorHandler.CustomErrorOutput(e);
                }
            }

            public string[] get_all_file_names_from_folder(string tableName, string folderID)
            {
                try
                {
                    string pathLocator = get_path_locator(tableName, folderID);

                    connector.db.add_select(tableName, "name");
                    connector.db.add_where_normal(tableName, "is_archive", 1);
                    connector.db.add_where_normal(tableName, "parent_path_locator", pathLocator);
                    return connector.db.run_return_array();
                }
                catch (Exception e)
                {
                    throw errorHandler.CustomErrorOutput(e);
                }
            }
            
            public string[] get_all_folder_names_from_root(string tableName)
            {
                try
                {
                    connector.db.add_select(tableName, "name");
                    connector.db.add_where_normal(tableName, "is_directory", 1);
                    connector.db.add_where_is(tableName, "parent_path_locator");
                    return connector.db.run_return_array();
                }
                catch (Exception e)
                {
                    throw errorHandler.CustomErrorOutput(e);
                }
            }

            public string[] get_all_folder_names_from_folder(string tableName, string folderID)
            {
                try
                {
                    string pathLocator = get_path_locator(tableName, folderID);

                    connector.db.add_select(tableName, "name");
                    connector.db.add_where_normal(tableName, "is_directory", 1);
                    connector.db.add_where_normal(tableName, "parent_path_locator", pathLocator);
                    return connector.db.run_return_array();
                }
                catch (Exception e)
                {
                    throw errorHandler.CustomErrorOutput(e);
                }
            }




            //Main get methods to retreive all data

			public DataTable get_all_from_root(string tableName)
			{
				try
				{
					connector.db.add_pure_sql(string.Format(@"
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
							FROM {0}
						", tableName));
                    connector.db.add_where_is(tableName, "parent_path_locator");
                    return connector.db.run_return_datatable();
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}

			public DataTable get_all_from_folder(string tableName, string folderID)
			{
				try
				{
					if (!string.IsNullOrWhiteSpace(folderID))
					{
						string pathLocator = get_path_locator(tableName, folderID);

						connector.db.add_pure_sql(string.Format(@"
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
							FROM {0}
						", tableName));
						connector.db.add_where_normal(tableName, "parent_path_locator", pathLocator);
						return connector.db.run_return_datatable();
					}

					return null;
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}

			public DataTable get_all_files_from_root_dt(string tableName)
			{
				try
				{
					connector.db.add_pure_sql(string.Format(@"
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
							FROM {0}
						", tableName));
					connector.db.add_where_normal(tableName, "is_archive", 1);
                    connector.db.add_where_is(tableName, "parent_path_locator");
                    return connector.db.run_return_datatable();
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}

			public DataTable get_files_from_folder_dt(string tableName, string folderID)
			{
				try
				{
					if (!string.IsNullOrWhiteSpace(folderID))
					{
						string pathLocator = get_path_locator(tableName, folderID);

						connector.db.add_pure_sql(string.Format(@"
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
							FROM {0}
						", tableName));
						connector.db.add_where_normal(tableName, "is_archive", 1);
						connector.db.add_where_normal(tableName, "parent_path_locator", pathLocator);
						return connector.db.run_return_datatable();
					}

					return null;
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}
            
            public DataTable get_all_folders_from_root_dt(string tableName)
            {
                try
                {
                    connector.db.add_pure_sql(string.Format(@"
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
							FROM {0}
						", tableName));
                    connector.db.add_where_normal(tableName, "is_directory", 1);
                    connector.db.add_where_is(tableName, "parent_path_locator");
                    return connector.db.run_return_datatable();
                }
                catch (Exception e)
                {
                    throw errorHandler.CustomErrorOutput(e);
                }
            }

            public DataTable get_folders_from_folder_dt(string tableName, string folderID)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(folderID))
                    {
                        string pathLocator = get_path_locator(tableName, folderID);

                        connector.db.add_pure_sql(string.Format(@"
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
							FROM {0}
						", tableName));
                        connector.db.add_where_normal(tableName, "is_directory", 1);
                        connector.db.add_where_normal(tableName, "parent_path_locator", pathLocator);
                        return connector.db.run_return_datatable();
                    }

                    return null;
                }
                catch (Exception e)
                {
                    throw errorHandler.CustomErrorOutput(e);
                }
            }
            
            public DataTable get_root_folders(string tableName)
			{
				try
				{
					connector.db.add_pure_sql(string.Format(@"
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
							FROM {0}
						", tableName));
					connector.db.add_where_normal(tableName, "is_directory", 1);
                    connector.db.add_where_is(tableName, "parent_path_locator");
                    return connector.db.run_return_datatable();
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}

			public DataTable get_folders_from_folder(string tableName, string folderID)
			{
				try
				{
					if (!string.IsNullOrWhiteSpace(folderID))
					{
						string pathLocator = get_path_locator(tableName, folderID);

						connector.db.add_pure_sql(string.Format(@"
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
							FROM {0}
						", tableName));
						connector.db.add_where_normal(tableName, "is_directory", 1);
						connector.db.add_where_normal(tableName, "parent_path_locator", pathLocator);
						return connector.db.run_return_datatable();
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