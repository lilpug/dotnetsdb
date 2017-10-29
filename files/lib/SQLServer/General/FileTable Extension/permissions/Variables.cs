using DotNetSDB.Connector;
using System;

namespace DotNetSDB.SqlServer.FileTable
{
    /// <summary>
    /// This is the SQL Server Filetable Extension class for DotNetSDB
    /// </summary>
    public partial class SQLServerFileTableExtension
    {			
		public partial class FileTablePermissions
		{
            /// <summary>
            /// Variable that holds the connection in a dynamic object state *thus we can use all versions of sql server in one class*
            /// </summary>
            private DatabaseConnector connector;

            /// <summary>
            /// Variable that holds the generic filetable error object
            /// </summary>
            private ErrorHandler errorHandler = new ErrorHandler();

            /// <summary>
            /// Variable that holds the database name
            /// </summary>
            private string dbName;

            /// <summary>
            /// This function initialises the FileTableExtension object using the database connection and database name supplied
            /// </summary>
            /// <param name="databaseObject"></param>
            /// <param name="databaseName"></param>
			public FileTablePermissions(DatabaseConnector databaseObject, string databaseName)
			{
                //Sets the database name the connection is using
                dbName = databaseName;

                //Sets the connection object
                connector = databaseObject;

                //Checks if the connection is a sql server connection and is valid
                if(string.IsNullOrWhiteSpace(dbName))
                {
                    throw new Exception("Database FileTable Permissions Initialisation: The database name you have passed is null.");
                }
                else if (connector == null || connector.DB == null)
                {
                    throw new Exception("Database FileTable Permissions Initialisation: The database object you have passed is null.");
                }
                else if(!connector.IsDbSqlServer)
                {
                    throw new Exception("Database FileTable Permissions Initialisation: The database object you have passed is not a sql server object.");
                }
                else if (connector.DBVersion == "sqlserver2008")
                {
                    throw new Exception($"Database FileTable Permissions Initialisation: The database object version you have passed does not support filetables '{connector.DBVersion}'.");
                }
                else if (!connector.DB.is_alive())
                {
                    throw new Exception("Database FileTable Permissions Initialisation: The database object is not connected.");
                }
            }
		}
    }
    
}