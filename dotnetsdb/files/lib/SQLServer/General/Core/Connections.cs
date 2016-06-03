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
            connectionTime = connectionTimeout;
            logger = errorLogger;
        }
    }

    /// <summary>
    /// The main core class for the sql server
    /// </summary>
    public partial class SqlServerCore
    {
        /*##########################################*/
        /*       Database Compiling functions       */
        /*##########################################*/

        //This function processes the connection string from a sqlconnection object and breaks it into its seperate entities
        protected bool connection_decompile(SqlConnection theConnection)
        {
            //This will be used to find the correct positions which is why its all lower case
            //Note: we do not extract the final data from this as its all been made lower case
            string temp = theConnection.ConnectionString.ToLower();

            //We pull this as its essential in determining what kind of connection is being asked for
            string windowsAuth = GetBetweenStringValue(temp, "integrated security=", ";");

            //Sets all the variables up
            DecompileVariables(temp, windowsAuth);

            //Checks the validity of the variables
            if (DecompileMinimumValidity(windowsAuth))
            {
                //Check which connection type we are compiling
                if (string.IsNullOrWhiteSpace(user) && string.IsNullOrWhiteSpace(pwd))
                {
                    //Builds the windows authentication string
                    connection = "Server=" + server + "; Database=" + db + ";Integrated Security=SSPI;connection timeout=" + connectionTime.ToString();
                    return true;
                }
                else
                {
                    //Builds the sql authentication string
                    connection = "Server=" + server + "; Database=" + db + "; User Id=" + user + "; Password=" + pwd + ";connection timeout=" + connectionTime.ToString();
                    return true;
                }
            }

            return false;
        }

        //This function pulls out all the variable from a connection string
        private void DecompileVariables(string connectionString, string windowsAuth)
        {
            //Checks if the connection is using windows authentication
            //Note: if so we do not require username and passwords
            if (string.IsNullOrWhiteSpace(windowsAuth) || (windowsAuth != "true" && windowsAuth != "sspi"))
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

            string tempConnectionTimeout = GetBetweenStringValue(connectionString, "connection timeout=", ";");
            connectionTime = (!string.IsNullOrWhiteSpace(tempConnectionTimeout)) ? Convert.ToInt32(tempConnectionTimeout) : 30;
        }

        //This function ensures the minimum required variables for a connection are valid
        private bool DecompileMinimumValidity(string windowsAuth)
        {
            //Windows authentication validity check
            if (string.IsNullOrWhiteSpace(windowsAuth) || windowsAuth == "true" || windowsAuth == "false" || windowsAuth == "sspi")
            {
                //Default variable checks
                if (!string.IsNullOrWhiteSpace(server) && !string.IsNullOrWhiteSpace(db) && connectionTime >= 0)
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
            connectionTime = connectionInformation.connectionTime;
            loggerDetails = connectionInformation.logger;

            //Recreates the connection string
            connection = string.Format("Server={0};Database={1};User Id={2};Password={3};Connection Timeout={4};", server, db, user, pwd, connectionTime);

            //Initialises the connection
            connectionInit();

            return true;
        }

        public bool switch_connection(SQLServerWindowsConnection connectionInformation)
        {
            //Sets the connection string
            db = connectionInformation.dbName;
            server = connectionInformation.server;
            connectionTime = connectionInformation.connectionTime;
            loggerDetails = connectionInformation.logger;

            //Recreates the connection string
            connection = string.Format("Server={0};Database={1};Integrated Security=SSPI;Connection Timeout={2};", server, db, connectionTime);

            //Initialises the connection
            connectionInit();

            return true;
        }

        public bool switch_connection(SqlConnection theConnection)
        {
            //Checks if anything went wrong with decompiling the connection string from the sqlconnection
            if (connection_decompile(theConnection))
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
            //We pull this as its essential in determining what kind of connection is being asked for
            string windowsAuth = GetBetweenStringValue(sqlConnectionString.ToLower(), "integrated security=", ";");

            //Pulls the variable out
            DecompileVariables(sqlConnectionString.ToLower(), windowsAuth);

            //Checks the validity of the variables
            if (DecompileMinimumValidity(windowsAuth))
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