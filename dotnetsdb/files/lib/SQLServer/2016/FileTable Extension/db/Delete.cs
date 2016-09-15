using System;

namespace DotNetSDB
{
    public partial class SqlServer2016 : SqlServerCore
    {
        public partial class filetable_extension
        {
            public void delete_directory(string tableName, string streamID)
            {
                try
                {
                    db.add_select(tableName, "path_locator");
                    db.add_where_normal(tableName, "stream_id", streamID);
                    string locator = db.run_return_string();
                    if (!string.IsNullOrWhiteSpace(locator))
                    {
                        //Starts the delete recursion off
                        //Note: this is recursive because a folder cannot be delete if it has folders and files within it
                        DeleteDirectoryRecursively(tableName, locator);
                    }
                    else
                    {
                        throw new Exception("Database FileTable Directory Delete: A path locator could not be retrieved from the stream ID.");
                    }
                }
                catch (Exception e)
                {
                    throw CustomErrorOutput(e);
                }
            }

            public void delete_file(string tableName, string streamID)
            {
                try
                {
                    db.add_delete(tableName);
                    db.add_where_normal(tableName, "stream_id", streamID);
                    db.add_where_normal(tableName, "is_directory", 0);
                    db.add_where_normal(tableName, "is_archive", 1);
                    db.run();
                }
                catch (Exception e)
                {
                    throw CustomErrorOutput(e);
                }
            }
        }
    }
}