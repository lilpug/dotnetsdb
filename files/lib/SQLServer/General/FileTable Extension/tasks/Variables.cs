using DotNetSDB.Connector;
using System;

namespace DotNetSDB.SqlServer.FileTable
{
    public partial class SQLServerFileTableExtension
    {			
		public partial class FileTableTasks
		{
            //Holds the database connection in a dynamic object *thus we can use all versions of sql server in one class*
            private DatabaseConnector connector;

            private ErrorHandler errorHandler = new ErrorHandler();

            /// <summary>
            /// This function initialises the FiletableTasks object
            /// </summary>
            /// <param name="databaseObject"></param>
			public FileTableTasks(DatabaseConnector databaseObject)
			{
                connector = databaseObject;

                //Checks if the connection is a sql server connection and is valid
                if (connector == null || connector.DB == null)
                {
                    throw new Exception("Database FileTable Tasks Initialisation: The database object you have passed is null.");
                }
                else if(!connector.IsDbSqlServer)
                {
                    throw new Exception("Database FileTable Tasks Initialisation: The database object you have passed is not a sql server object.");
                }
                else if (connector.DBVersion == "sqlserver2008")
                {
                    throw new Exception($"Database FileTable Tasks Initialisation: The database object version you have passed does not support filetables '{connector.DBVersion}'.");
                }
                else if (!connector.DB.is_alive())
                {
                    throw new Exception("Database FileTable Tasks Initialisation: The database object is not connected.");
                }
            }
		}
    }
    
}