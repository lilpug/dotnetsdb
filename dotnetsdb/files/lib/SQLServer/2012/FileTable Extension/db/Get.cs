using System;
using System.Collections.Generic;
using System.Data;

namespace DotNetSDB
{
    public partial class SqlServer2012 : SqlServerCore
    {
        public partial class filetable_extension
        {
            public string get_root_directory_id(string tableName, string folderName)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(folderName))
                    {
                        db.add_select(tableName, "stream_id");
                        db.add_where_normal(tableName, "name", folderName);
                        db.add_where_normal(tableName, "is_directory", 1);
                        db.add_where_is(tableName, "parent_path_locator");
                        return db.run_return_string();
                    }

                    return null;
                }
                catch (Exception e)
                {
                    throw CustomErrorOutput(e);
                }
            }

            public string get_root_file_id(string tableName, string fileName)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        db.add_select(tableName, "stream_id");
                        db.add_where_normal(tableName, "name", fileName);
                        db.add_where_normal(tableName, "is_archive", 1);
                        db.add_where_is(tableName, "parent_path_locator");
                        return db.run_return_string();
                    }

                    return null;
                }
                catch (Exception e)
                {
                    throw CustomErrorOutput(e);
                }
            }

            public string get_directory_id(string tableName, string parentFolderID, string folderName)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(parentFolderID))
                    {
                        string locator = get_path_locator(tableName, parentFolderID);
                        if (!string.IsNullOrWhiteSpace(locator))
                        {
                            db.add_select(tableName, "stream_id");
                            db.add_where_normal(tableName, "name", folderName);
                            db.add_where_normal(tableName, "is_directory", 1);
                            db.add_where_normal(tableName, "parent_path_locator", locator);
                            return db.run_return_string();
                        }
                    }

                    return null;
                }
                catch (Exception e)
                {
                    throw CustomErrorOutput(e);
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
                            db.add_select(tableName, "stream_id");
                            db.add_where_normal(tableName, "name", fileName);
                            db.add_where_normal(tableName, "is_archive", 1);
                            db.add_where_normal(tableName, "parent_path_locator", locator);
                            return db.run_return_string();
                        }
                    }
                    return null;
                }
                catch (Exception e)
                {
                    throw CustomErrorOutput(e);
                }
            }

            public Dictionary<string, byte[]> get_all_files_from_root(string tableName)
            {
                try
                {
                    db.add_select(tableName, "*");
                    db.add_where_normal(tableName, "is_archive", 1);
                    DataTable results = db.run_return_datatable();

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
                    throw CustomErrorOutput(e);
                }
            }

            public Dictionary<string, byte[]> get_all_files_from_directory(string tableName, string streamID)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(streamID))
                    {
                        string pathLocator = get_path_locator(tableName, streamID);

                        db.add_select(tableName, "*");
                        db.add_where_normal(tableName, "is_archive", 1);
                        db.add_where_normal(tableName, "parent_path_locator", pathLocator);
                        DataTable results = db.run_return_datatable();

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
                    throw CustomErrorOutput(e);
                }
            }

            public Dictionary<string, byte[]> get_file(string tableName, string streamID)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(streamID))
                    {
                        db.add_select(tableName, "*");
                        db.add_where_normal(tableName, "stream_id", streamID);
                        db.add_where_normal(tableName, "is_archive", 1);
                        DataTable results = db.run_return_datatable();

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
                    throw CustomErrorOutput(e);
                }
            }

            public string get_path_locator(string tableName, string streamID)
            {
                try
                {
                    db.add_select(tableName, "path_locator", "CAST( ", " AS varchar(max)) as path_locator");
                    db.add_where_normal(tableName, "stream_id", streamID);
                    return db.run_return_string();
                }
                catch (Exception e)
                {
                    throw CustomErrorOutput(e);
                }
            }

            public DataTable get_all_from_root(string tableName)
            {
                try
                {
                    db.add_pure_sql(String.Format(@"
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
                    return db.run_return_datatable();
                }
                catch (Exception e)
                {
                    throw CustomErrorOutput(e);
                }
            }

            public DataTable get_all_from_directory(string tableName, string streamID)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(streamID))
                    {
                        string pathLocator = get_path_locator(tableName, streamID);

                        db.add_pure_sql(String.Format(@"
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
                        db.add_where_normal(tableName, "parent_path_locator", pathLocator);
                        return db.run_return_datatable();
                    }

                    return null;
                }
                catch (Exception e)
                {
                    throw CustomErrorOutput(e);
                }
            }

            public DataTable get_all_files_from_root_dt(string tableName)
            {
                try
                {
                    db.add_pure_sql(String.Format(@"
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
                    db.add_where_normal(tableName, "is_archive", 1);
                    return db.run_return_datatable();
                }
                catch (Exception e)
                {
                    throw CustomErrorOutput(e);
                }
            }

            public DataTable get_files_from_directory_dt(string tableName, string streamID)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(streamID))
                    {
                        string pathLocator = get_path_locator(tableName, streamID);

                        db.add_pure_sql(String.Format(@"
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
                        db.add_where_normal(tableName, "is_archive", 1);
                        db.add_where_normal(tableName, "parent_path_locator", pathLocator);
                        return db.run_return_datatable();
                    }

                    return null;
                }
                catch (Exception e)
                {
                    throw CustomErrorOutput(e);
                }
            }

            public DataTable get_root_directories(string tableName)
            {
                try
                {
                    db.add_pure_sql(String.Format(@"
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
                    db.add_where_normal(tableName, "is_directory", 1);
                    return db.run_return_datatable();
                }
                catch (Exception e)
                {
                    throw CustomErrorOutput(e);
                }
            }

            public DataTable get_directories_from_directory(string tableName, string streamID)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(streamID))
                    {
                        string pathLocator = get_path_locator(tableName, streamID);

                        db.add_pure_sql(String.Format(@"
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
                        db.add_where_normal(tableName, "is_directory", 1);
                        db.add_where_normal(tableName, "parent_path_locator", pathLocator);
                        return db.run_return_datatable();
                    }

                    return null;
                }
                catch (Exception e)
                {
                    throw CustomErrorOutput(e);
                }
            }

            //Single functions

            public DataTable get_single_structure_information(string tableName, string name)
            {
                try
                {
                    db.add_pure_sql(String.Format(@"
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
                    db.add_where_normal(tableName, "name", name);
                    DataTable results = db.run_return_datatable();
                    if (results != null && results.Rows.Count == 1)
                    {
                        return results;
                    }
                    else
                    {
                        throw new Exception("Database FileTable Get Single: This function can only be used when there is no duplication in the name of files or directories, NOTE: directory depth is not taken into account.");
                    }
                }
                catch (Exception e)
                {
                    throw CustomErrorOutput(e);
                }
            }

            public string get_single_structure_id(string tableName, string name)
            {
                try
                {
                    db.add_select(tableName, "stream_id");
                    db.add_where_normal(tableName, "name", name);
                    DataTable results = db.run_return_datatable();
                    if (results != null && results.Rows.Count == 1)
                    {
                        return results.Rows[0]["stream_id"].ToString();
                    }
                    else
                    {
                        throw new Exception("Database FileTable Get Single: This function can only be used when there is no duplication in the name of files or directories, NOTE: directory depth is not taken into account.");
                    }
                }
                catch (Exception e)
                {
                    throw CustomErrorOutput(e);
                }
            }

            public Dictionary<string, byte[]> get_single_structure_file(string tableName, string name)
            {
                try
                {
                    db.add_select(tableName, new string[] { "name", "file_stream" });
                    db.add_where_normal(tableName, "name", name);
                    DataTable results = db.run_return_datatable();
                    if (results != null && results.Rows.Count == 1)
                    {
                        Dictionary<string, byte[]> file = new Dictionary<string, byte[]>();
                        file.Add(results.Rows[0]["name"].ToString(), (byte[])results.Rows[0]["file_stream"]);
                        return file;
                    }
                    else
                    {
                        throw new Exception("Database FileTable Get Single: This function can only be used when there is no duplication in the name of files or directories, NOTE: directory depth is not taken into account.");
                    }
                }
                catch (Exception e)
                {
                    throw CustomErrorOutput(e);
                }
            }
        }
    }
}