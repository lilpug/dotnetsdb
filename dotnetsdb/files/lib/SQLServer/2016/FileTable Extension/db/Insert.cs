using System;

namespace DotNetSDB
{
    public partial class SqlServer2016 : SqlServerCore
    {
        public partial class filetable_extension
        {
            public void create_file_at_root(string tableName, string fileName, byte[] fileData)
            {
                try
                {
                    db.add_insert(tableName, new string[] { "name", "file_stream" }, new object[] { fileName, fileData });
                    db.run();
                }
                catch (Exception e)
                {
                    throw CustomErrorOutput(e);
                }
            }

            public void create_file_at_directory(string tableName, string directoryID, string fileName, byte[] fileData)
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
                    db.add_insert(tableName, new string[] { "name", "file_stream", "path_locator" }, new object[] { fileName, fileData, newID });
                    db.run();
                }
                catch (Exception e)
                {
                    throw CustomErrorOutput(e);
                }
            }

            public void create_root_directory(string tableName, string folderName)
            {
                try
                {
                    db.add_insert(tableName, new string[] { "name", "is_directory", "is_archive" }, new object[] { folderName, 1, 0 });
                    db.run();
                }
                catch (Exception e)
                {
                    throw CustomErrorOutput(e);
                }
            }

            public void create_additional_directory(string tableName, string parentDirectoryID, string folderName)
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
                    db.add_insert(tableName, new string[] { "name", "is_directory", "is_archive", "path_locator" }, new object[] { folderName, 1, 0, newID });
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