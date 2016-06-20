using DotNetSDB.output;
using MySql.Data.MySqlClient;
using System;

namespace DotNetSDB
{
    /*##########################################*/
    /*       Database Connection Classes        */
    /*##########################################*/

    public class MySQLUserConnection
    {
        public string server { get; set; }
        public string user { get; set; }
        public string pwd { get; set; }
        public string dbName { get; set; }
        public int port { get; set; }
        public int connectionTime { get; set; }

        public OutputManagementVariable logger { get; set; }

        /// <summary>
        /// This function is the initialisation for the mysql user connection class
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="databaseName"></param>
        /// <param name="connectionTimeout"></param>
        /// <param name="errorLogger"></param>
        public MySQLUserConnection(string serverName, string username, string password, string databaseName, int connectionTimeout = 30, OutputManagementVariable errorLogger = null)
        {
            server = serverName;
            user = username;
            pwd = password;
            dbName = databaseName;
            port = -1;//This tells the connection builder not to add a port field
            connectionTime = connectionTimeout;
            logger = errorLogger;
        }

        /// <summary>
        /// This function is the initialisation for the mysql user connection class
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="databaseName"></param>
        /// <param name="thePort"></param>
        /// <param name="connectionTimeout"></param>
        /// <param name="errorLogger"></param>
        public MySQLUserConnection(string serverName, string username, string password, string databaseName, string thePort, int connectionTimeout = 30, OutputManagementVariable errorLogger = null)
        {
            server = serverName;
            user = username;
            pwd = password;
            dbName = databaseName;
            port = Convert.ToInt32(thePort);
            connectionTime = connectionTimeout;
            logger = errorLogger;
        }
    }

    //sql_server_database class (the IDispossible allows the class to be used with using statements)
    public partial class MysqlCore
    {
        /*######################################################*/
        /*    Database Connection String Compiling functions    */
        /*######################################################*/
        
        //Builds the sql authentication string
        protected void SqlAuthConnectionString()
        {
            if (port == -1)
            {
                connection = string.Format("Server={0};Database={2};UId={3};Pwd={4};Connection Timeout={5};AllowZeroDateTime=true;ConvertZeroDatetime=True", server, db, user, pwd, connectionTime.ToString());
            }
            else
            {
                connection = string.Format("Server={0},{1};Database={2};UId={3};Pwd={4};Connection Timeout={5};AllowZeroDateTime=true;ConvertZeroDatetime=True", server, port, db, user, pwd, connectionTime.ToString());
            }
        }
        

        /*##########################################*/
        /*       Database Compiling functions       */
        /*##########################################*/

        //This function processes the connection string from a sqlconnection object
        protected bool ConnectionDecompile(MySqlConnection theConnection)
        {
            return ConnectionDecompile(theConnection.ConnectionString);
        }

        //This function processes the connection string from a sqlconnection object and breaks it into its seperate entities
        protected bool ConnectionDecompile(string connectionString)
        {
            //This will be used to find the correct positions which is why its all lower case
            //Note: we do not extract the final data from this as its all been made lower case
            string con = connectionString.ToLower();

            //Pulls the variables needed out and ensures they are all valid
            if (DecompileVariables(con))
            {
                //creates the connection string
                SqlAuthConnectionString();                
                return true;
            }

            return false;
        }

        //This function pulls out all the variable from a connection string
        private bool DecompileVariables(string connectionString)
        {
            try
            {
                //Checks which version of the user id someone has entered for the connection string
                if (connectionString.IndexOf("user id=") != -1)
                {
                    user = GetBetweenStringValue(connectionString, "user id=", ";");
                }
                else if (connectionString.IndexOf("userid=") != -1)
                {
                    user = GetBetweenStringValue(connectionString, "userid=", ";");
                }
                else if (connectionString.IndexOf("uid=") != -1)
                {
                    user = GetBetweenStringValue(connectionString, "uid=", ";");
                }

                pwd = GetBetweenStringValue(connectionString, "pwd=", ";");

                server = GetBetweenStringValue(connectionString, "server=", ";");

                db = GetBetweenStringValue(connectionString, "database=", ";");

                //Checks if there is a port number supplied
                string[] tempPort = server.Split(',');
                port = (tempPort != null && tempPort.Length > 1) ? Convert.ToInt32(tempPort[1]) : -1;

                //Checks if there is a connection timeout supplied
                string tempConnectionTimeout = GetBetweenStringValue(connectionString, "connection timeout=", ";");
                connectionTime = (!string.IsNullOrWhiteSpace(tempConnectionTimeout)) ? Convert.ToInt32(tempConnectionTimeout) : 30;

                //Checks all values required have been populated and are valid
                return MinimumConnectionValidity();
            }
            catch { }
            return false;
        }

        //This function ensures the minimum required variables for a connection are valid
        private bool MinimumConnectionValidity()
        {
            //Default variable checks
            if (!string.IsNullOrWhiteSpace(server) && !string.IsNullOrWhiteSpace(db) && connectionTime >= 0 && !string.IsNullOrWhiteSpace(user) && !string.IsNullOrWhiteSpace(pwd))
            {
                return true;
            }

            return false;
        }

        /*##########################################*/
        /*      Database connection functions       */
        /*##########################################*/

        public MysqlCore(MySQLUserConnection connectionInformation)
        {
            switch_connection(connectionInformation);
        }

        public MysqlCore(MySqlConnection theConnection)
        {
            switch_connection(theConnection);
        }

        public MysqlCore(string sqlConnectionString)
        {
            switch_connection(sqlConnectionString);
        }

        //This function checks to see if the connection information allows connections or not
        public bool is_alive()
        {
            try
            {
                using (myConnection = new MySqlConnection(connection))
                {
                    myConnection.Open();
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        //These functions allow the user to change the connection string after the object has been built with whichever definition they want
        public bool switch_connection(MySQLUserConnection connectionInformation)
        {
            //Sets the connection string
            db = connectionInformation.dbName;
            user = connectionInformation.user;
            pwd = connectionInformation.pwd;
            server = connectionInformation.server;
            port = connectionInformation.port;
            connectionTime = connectionInformation.connectionTime;
            loggerDetails = connectionInformation.logger;

            //Creates the connection string
            SqlAuthConnectionString();

            //Initialises the connection
            connectionInit();

            return true;
        }

        public bool switch_connection(MySqlConnection theConnection)
        {
            //Checks if anything went wrong with decompiling the connection string from the sqlconnection
            if (ConnectionDecompile(theConnection))
            {
                //Initialises the connection
                connectionInit();
            }
            else
            {
                throw new ArgumentException("Database Connection Error: The connection string passed could not be validated.");
            }

            return true;
        }

        public bool switch_connection(string sqlConnectionString)
        {
            //Checks the validity of the variables
            if (ConnectionDecompile(sqlConnectionString))
            {
                //Initialises the connection
                connectionInit();
            }
            else
            {
                throw new ArgumentException("Database Connection Error: The connection string passed could not be validated.");
            }

            return true;
        }

        /*##########################################*/
        /*    Database connection Initialisation    */
        /*##########################################*/

        private void connectionInit()
        {
            //Trys to connect to see if it will work
            try
            {
                using (myConnection = new MySqlConnection(connection))
                {
                    myConnection.Open();
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException("Database Connection Error: " + e.Message);
            }
            if (loggerDetails != null)
            {
                //Checks all the outputManagement details are correct and can instantiate the object
                try
                {
                    using (OutputManagement debugger = new OutputManagement(loggerDetails))
                    {
                    }
                }
                catch (Exception e)
                {
                    throw new ArgumentException(e.Message);
                }
            }
        }
    }
}