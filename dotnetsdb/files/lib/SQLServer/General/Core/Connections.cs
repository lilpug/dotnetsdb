using DotNetSDB.output;
using System;
using System.Data.SqlClient;

namespace DotNetSDB
{
    /*##########################################*/
    /*       Database Connection Classes        */
    /*##########################################*/

    /// <summary>
    /// The sql server user connection class
    /// </summary>
    public class SQLServerUserConnection
    {
        public string server { get; set; }
        public string user { get; set; }
        public string pwd { get; set; }
        public string dbName { get; set; }
        public int port { get; set; }
        public int connectionTime { get; set; }
        

        public OutputManagementVariable logger { get; set; }

        /// <summary>
        /// This function is the initialisation for the sql server user connection class
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="databaseName"></param>        
        /// <param name="connectionTimeout"></param>
        /// <param name="errorLogger"></param>
        public SQLServerUserConnection(string serverName, string username, string password, string databaseName, int connectionTimeout = 30, OutputManagementVariable errorLogger = null)
        {
            server = serverName;
            user = username;
            pwd = password;
            dbName = databaseName;
            port = -1;
            connectionTime = connectionTimeout;
            logger = errorLogger;
        }

        /// <summary>
        /// This function is the initialisation for the sql server user connection class
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="databaseName"></param>
        /// <param name="thePort"></param>
        /// <param name="connectionTimeout"></param>
        /// <param name="errorLogger"></param>
        public SQLServerUserConnection(string serverName, string username, string password, string databaseName, string thePort, int connectionTimeout = 30, OutputManagementVariable errorLogger = null)
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

    /// <summary>
    /// The sql server windows connection class
    /// </summary>
    public class SQLServerWindowsConnection
    {
        public string server { get; set; }
        public string dbName { get; set; }
        public int port { get; set; }
        public int connectionTime { get; set; }
        

        public OutputManagementVariable logger { get; set; }
        
        /// <summary>
        /// This function is the initialisation for the sql server windows connection class
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="database"></param>        
        /// <param name="connectionTimeout"></param>
        /// <param name="errorLogger"></param>
        public SQLServerWindowsConnection(string serverName, string database, int connectionTimeout = 30, OutputManagementVariable errorLogger = null)
        {
            server = serverName;
            dbName = database;
            port = -1;
            connectionTime = connectionTimeout;
            logger = errorLogger;
        }

        /// <summary>
        /// This function is the initialisation for the sql server windows connection class
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="database"></param>
        /// <param name="thePort"></param>
        /// <param name="connectionTimeout"></param>
        /// <param name="errorLogger"></param>
        public SQLServerWindowsConnection(string serverName, string database, string thePort, int connectionTimeout = 30, OutputManagementVariable errorLogger = null)
        {
            server = serverName;
            dbName = database;
            port = Convert.ToInt32(thePort);
            connectionTime = connectionTimeout;
            logger = errorLogger;            
        }
    }

    /// <summary>
    /// The main core class for the sql server
    /// </summary>
    public partial class SqlServerCore
    {
        /*######################################################*/
        /*    Database Connection String Compiling functions    */
        /*######################################################*/

        //Builds the windows authentication string
        protected void WindowsAuthConnectionString()
        {
            if (port == -1)
            {
                connection = string.Format("Server={0};Database={1};Integrated Security=SSPI;connection timeout={2}", server, db, connectionTime);
            }
            else
            {
                connection = string.Format("Server={0},{1};Database={2};Integrated Security=SSPI;connection timeout={3}", server, port, db, connectionTime);
            }
        }

        //Builds the sql authentication string
        protected void SqlAuthConnectionString()
        {
            if (port == -1)
            {
                connection = string.Format("Server={0};Database={2};User Id={3};Password={4};connection timeout={5}", server, db, user, pwd, connectionTime);
            }
            else
            {
                connection = string.Format("Server={0},{1};Database={2};User Id={3};Password={4};connection timeout={5}", server, port, db, user, pwd, connectionTime);
            }
        }

        /*##########################################*/
        /*       Database Compiling functions       */
        /*##########################################*/

        //This function processes the connection string from a sqlconnection object
        protected bool ConnectionDecompile(SqlConnection theConnection)
        {   
            return ConnectionDecompile(theConnection.ConnectionString);            
        }

        //This function processes the connection string and breaks it into its seperate entities
        protected bool ConnectionDecompile(string connectionString)
        {
            //This will be used to find the correct positions which is why its all lower case
            //Note: we do not extract the final data from this as its all been made lower case
            string con = connectionString.ToLower();
            
            //Pulls the variables needed out and ensures they are all valid
            if (DecompileVariables(con))
            {
                //Check which connection type we are compiling and creates the connection string                
                if (isWindowsAuth)
                {   
                    WindowsAuthConnectionString();                    
                }
                else
                {
                    SqlAuthConnectionString();                    
                }

                return true;
            }

            return false;
        }
        
        //This function pulls out all the variable from a connection string
        private bool DecompileVariables(string connectionString)
        {
            try
            {
                //We pull this as its essential in determining what kind of connection is being asked for *sql user or windows auth*
                string windowsAuth = GetBetweenStringValue(connectionString.ToLower(), "integrated security=", ";");
                if (!string.IsNullOrWhiteSpace(windowsAuth) && (windowsAuth == "true" || windowsAuth == "false" || windowsAuth == "sspi"))
                {
                    isWindowsAuth = true;
                }
                else
                {
                    isWindowsAuth = false;
                }

                //Checks if the connection is using windows authentication
                //Note: if so we do not require username and passwords
                if (!isWindowsAuth)
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

                    pwd = GetBetweenStringValue(connectionString, "password=", ";");
                }

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
            if(isWindowsAuth)
            {   
                if (!string.IsNullOrWhiteSpace(server) && !string.IsNullOrWhiteSpace(db) && connectionTime >= 0)
                {
                    return true;
                }
            }
            else
            {   
                if (!string.IsNullOrWhiteSpace(server) && !string.IsNullOrWhiteSpace(db) && !string.IsNullOrWhiteSpace(user) && !string.IsNullOrWhiteSpace(pwd) && connectionTime >= 0)
                {
                    return true;
                }
            }

            return false;
        }

        /*##########################################*/
        /*      Database connection functions       */
        /*##########################################*/

        public SqlServerCore(SQLServerUserConnection connectionInformation)
        {
            switch_connection(connectionInformation);
        }

        public SqlServerCore(SQLServerWindowsConnection connectionInformation)
        {
            switch_connection(connectionInformation);
        }

        public SqlServerCore(SqlConnection theConnection)
        {
            switch_connection(theConnection);
        }

        public SqlServerCore(string sqlConnectionString)
        {
            switch_connection(sqlConnectionString);
        }

        //This function checks to see if the connection information allows connections or not
        public bool is_alive()
        {
            try
            {
                using (myConnection = new SqlConnection(connection))
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
        public bool switch_connection(SQLServerUserConnection connectionInformation)
        {
            //Sets the connection string
            db = connectionInformation.dbName;
            user = connectionInformation.user;
            pwd = connectionInformation.pwd;
            server = connectionInformation.server;
            port = connectionInformation.port;
            connectionTime = connectionInformation.connectionTime;
            loggerDetails = connectionInformation.logger;
            isWindowsAuth = false;

            //Creates the connection string
            SqlAuthConnectionString();

            //Initialises the connection
            connectionInit();

            return true;
        }

        public bool switch_connection(SQLServerWindowsConnection connectionInformation)
        {
            //Sets the connection string
            db = connectionInformation.dbName;
            server = connectionInformation.server;
            port = connectionInformation.port;
            connectionTime = connectionInformation.connectionTime;
            loggerDetails = connectionInformation.logger;
            isWindowsAuth = true;

            //Creates the connection string
            WindowsAuthConnectionString();

            //Initialises the connection
            connectionInit();

            return true;
        }

        public bool switch_connection(SqlConnection theConnection)
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
                using (myConnection = new SqlConnection(connection))
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