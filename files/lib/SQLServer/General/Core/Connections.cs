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
            port = -1;//This tells the connection builder not to add a port field
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
            port = -1;//This tells the connection builder not to add a port field
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
                connection = string.Format("Server={0};Database={1};User Id={2};Password={3};connection timeout={4}", server, db, user, pwd, connectionTime);
            }
            else
            {
                connection = string.Format("Server={0},{1};Database={2};User Id={3};Password={4};connection timeout={5}", server, port, db, user, pwd, connectionTime);
            }
        }
        
        /*##########################################*/
        /*      Database connection functions       */
        /*##########################################*/

        public SqlServerCore(SQLServerUserConnection connectionInformation)
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
        }

        public SqlServerCore(SQLServerWindowsConnection connectionInformation)
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