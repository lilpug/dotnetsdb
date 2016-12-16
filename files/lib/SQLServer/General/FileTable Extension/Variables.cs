using DotNetSDB.Connector;
using System;

namespace DotNetSDB.SqlServer.FileTable
{
    public partial class SqlServerFileTableExtension
    {
        //Holds the database connection in a dynamic object *thus we can use all versions of sql server in one class*
        private DatabaseConnector connector;

        //Variables used to active the different filetable functions
		public FileTablePermissions permissions;			
		public FileTableTasks tasks;
			
        public SqlServerFileTableExtension(DatabaseConnector databaseObject, string databaseName)
        {
            //Sets the connection object
            connector = databaseObject;

            //Checks if the connection is a sql server connection and is valid
            if (string.IsNullOrWhiteSpace(databaseName))
            {
                throw new Exception("Database FileTable Initialisation: The database name you have passed is null.");
            }
            else if (connector == null || connector.db == null)
            {
                throw new Exception("Database FileTable Initialisation: The database object you have passed is null.");
            }
            else if (!connector.isDbSqlServer)
            {
                throw new Exception("Database FileTable Initialisation: The database object you have passed is not a sql server object.");
            }
            else if (connector.dbVersion == "sqlserver2008")
            {
                throw new Exception(string.Format("Database FileTable Initialisation: The database object version you have passed does not support filetables '{0}'.", connector.dbVersion));
            }
            else if (!connector.db.is_alive())
            {
                throw new Exception("Database FileTable Initialisation: The database object is not connected.");
            }

            //Inits the permissions and tasks libs
            permissions = new FileTablePermissions(connector, databaseName);				
			tasks = new FileTableTasks(connector);
        }
    }
    
}