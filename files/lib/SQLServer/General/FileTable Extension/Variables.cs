using DotNetSDB.Connector;
using System;

namespace DotNetSDB.SqlServer.FileTable
{
    public partial class SQLServerFileTableExtension
    {
        //Holds the database connection in a dynamic object *thus we can use all versions of sql server in one class*
        private DatabaseConnector connector;
        
        /// <summary>
        /// This variable stores the filetable permission object
        /// </summary>
		public FileTablePermissions Permissions { get; set; }

        /// <summary>
        /// This variable stores the filetable tasks object
        /// </summary>
        public FileTableTasks Tasks { get; set; }
			
        /// <summary>
        /// This function initialises the FileTableExtension object
        /// </summary>
        /// <param name="databaseObject"></param>
        /// <param name="databaseName"></param>
        public SQLServerFileTableExtension(DatabaseConnector databaseObject, string databaseName)
        {
            //Sets the connection object
            connector = databaseObject;

            //Checks if the connection is a sql server connection and is valid
            if (string.IsNullOrWhiteSpace(databaseName))
            {
                throw new Exception("Database FileTable Initialisation: The database name you have passed is null.");
            }
            else if (connector == null || connector.DB == null)
            {
                throw new Exception("Database FileTable Initialisation: The database object you have passed is null.");
            }
            else if (!connector.IsDbSqlServer)
            {
                throw new Exception("Database FileTable Initialisation: The database object you have passed is not a sql server object.");
            }
            else if (connector.DBVersion == "sqlserver2008")
            {
                throw new Exception($"Database FileTable Initialisation: The database object version you have passed does not support filetables '{connector.DBVersion}'.");
            }
            else if (!connector.DB.is_alive())
            {
                throw new Exception("Database FileTable Initialisation: The database object is not connected.");
            }

            //Inits the permissions and tasks libs
            Permissions = new FileTablePermissions(connector, databaseName);				
			Tasks = new FileTableTasks(connector);
        }
    }
    
}