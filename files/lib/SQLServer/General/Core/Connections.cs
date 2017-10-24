using DotNetSDB.output;
using System;
using System.Data.SqlClient;
using System.Text;

namespace DotNetSDB
{
    /*##########################################*/
    /*       Database Connection Classes        */
    /*##########################################*/
    
    /// <summary>
    /// The SQL Server connection class
    /// </summary>
    public class SQLServerConnection : IDisposable
    {
        /// <summary>
        /// Holds the server location
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Holds the SQL username to use
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Holds the SQL user password to use
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Holds the database name to connect to
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// Holds whether or not to use windows authentication
        /// </summary>
        public string IntegratedSecurity { get; set; }

        /// <summary>
        /// Holds the port to connect to *-1 means default*
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Holds the connection timeout for the SQL connection
        /// </summary>
        public int ConnectionTimeout { get; set; }

        /// <summary>
        /// Holds any extra connection string parameters
        /// </summary>
        public string ConnectionStringExtra { get; set; }

        /// <summary>
        /// Holds the output management settings to use on failures
        /// </summary>
        public OutputManagementSettings LoggerSettings { get; set; }

        /// <summary>
        /// This function validates the connection information has the basic requirements
        /// </summary>
        private void Validation()
        {
            //Checks if a server has been supplied
            if (string.IsNullOrWhiteSpace(Server))
            {
                throw new Exception("SQL Server Connection Error: No server has been supplied.");
            }
            //Checks if a database has been supplied
            else if (string.IsNullOrWhiteSpace(Database))
            {
                throw new Exception("SQL Server Connection Error: No database has been supplied.");
            }
            //Checks if we have any form of credentials to use
            else if (string.IsNullOrWhiteSpace(User) && string.IsNullOrWhiteSpace(IntegratedSecurity))
            {
                throw new Exception("SQL Server Connection Error: No SQL user or Windows authentication has been supplied.");
            }
        }
        
        /// <summary>
        /// This function sets up the SQL Server connection based on a connection string
        /// </summary>
        /// <param name="connectionString">the SQL Server connection string to use</param>
        /// <param name="errorLogger">the error logger settings</param>
        public SQLServerConnection(string connectionString, OutputManagementSettings errorLogger = null)
        {
            //Checks if the supplied connection string is empty and throws an exception if so
            if(string.IsNullOrWhiteSpace(connectionString))
            {
                throw new Exception("SQL Server Connection Error: An empty connection string was supplied.");
            }

            //Splits the connection string up via the delimiter
            string[] connectionStringTypes = connectionString.Split(';');

            //Used to build the extra connection parameter string
            StringBuilder extraBuilder = new StringBuilder();

            //Loops through all the different types within the connection string
            foreach (string type in connectionStringTypes)
            {
                //Checks the type is not empty before breaking it up into its own category
                if (!string.IsNullOrWhiteSpace(type))
                {
                    //Stores the stripped out values
                    string name = "";
                    string value = "";

                    //Strips the value out
                    if (type.IndexOf('=') > 1)
                    {
                        //Obtains the type name
                        name = type.Substring(0, type.IndexOf('=')).Trim();

                        //Obtains the type value
                        value = type.Substring(type.IndexOf('=') + 1).Trim();
                        
                        //Checks the name is not empty
                        if (!string.IsNullOrWhiteSpace(name))
                        {
                            //This checks the type name and puts it into the correct category
                            switch (name.ToLower())
                            {
                                case "data source":
                                case "server":
                                case "address":
                                case "addr":
                                case "network address":
                                    //Checks if the server value has a port attached with it
                                    if (value.IndexOf(',') >= 1 && value.Length > 2)
                                    {
                                        //Extracts the port value
                                        string stringPort = value.Substring(value.IndexOf(',') + 1);

                                        //Checks it can convert it into an integer value otherwise throws an exception
                                        int tempPort = -1;
                                        if (!int.TryParse(stringPort, out tempPort))
                                        {
                                            throw new Exception("SQL Server Connection Error: Could not convert the port into an integer.");
                                        }
                                        else
                                        {
                                            Port = tempPort;
                                        }

                                        //Makes the value the server name without the port as we have now extracted it
                                        value = value.Substring(0, value.IndexOf(','));
                                    }

                                    Server = value;
                                    break;

                                case "initial catalog":
                                case "database":
                                    Database = value;
                                    break;

                                case "password":
                                case "pwd":
                                    Password = value;
                                    break;

                                case "user id":
                                case "uid":
                                    User = value;
                                    break;

                                case "connect timeout":
                                case "connection timeout":
                                case "timeout":

                                    //Checks it can convert it into an integer value otherwise throws an exception
                                    int tempTimeout = -1;
                                    if (!int.TryParse(value, out tempTimeout))
                                    {
                                        throw new Exception("SQL Server Connection Error: Could not convert the connection timeout into an integer.");
                                    }
                                    else
                                    {
                                        ConnectionTimeout = tempTimeout;
                                    }
                                    break;

                                case "integrated security":
                                case "trusted_connection":
                                    IntegratedSecurity = value;
                                    break;

                                default:
                                    //Only add it as an extra if the value is not empty
                                    if (!string.IsNullOrWhiteSpace(value))
                                    {
                                        extraBuilder.Append($"{name}={value};");
                                    }
                                    break;
                            }
                        }
                    }
                }
            }

            //Checks if a port has been supplied
            if(Port <= 0)
            {
                //This tells the connection builder not to add a port field
                Port = -1;
            }

            //Puts the built extra connection parameters into the string
            if (extraBuilder.Length > 0)
            {
                ConnectionStringExtra = extraBuilder.ToString();
            }

            //Checks if a default connection timeout has been specified if not generates one
            if (ConnectionTimeout <= 0)
            {
                ConnectionTimeout = 30;
            }

            //puts the logger settings into the logger variable
            LoggerSettings = errorLogger;

            //Runs the basic requirements validation
            Validation();
        }
        
        /// <summary>
        /// This function sets up the SQL Server connection based on an SQL user connection
        /// </summary>
        /// <param name="server">database server instance name</param>
        /// <param name="username">the SQL username to connect with</param>
        /// <param name="password">the SQL user password to connect with</param>
        /// <param name="database">database to connect to</param>
        /// <param name="connectionTimeout">connection timeout value in seconds</param>
        /// <param name="errorLogger">the error logger settings</param>
        /// <param name="additionalConnectionString">any additional extra parameters in connection string format</param>
        public SQLServerConnection(string server, string username, string password, string database, int connectionTimeout = 30, OutputManagementSettings errorLogger = null, string additionalConnectionString = null)
        {
            //Sets the server to connect to
            Server = server;

            //Sets the username to use
            User = username;

            //Sets the password to use
            Password = password;

            //Sets the database to connect to
            Database = database;
            
            //This tells the connection builder not to add a port field
            Port = -1;

            //Sets the connection timeout
            ConnectionTimeout = connectionTimeout;

            //Sets any extra connection string parameters
            ConnectionStringExtra = additionalConnectionString;

            //Tells the connection builder we don't want windows authentication
            IntegratedSecurity = null;

            //puts the logger settings into the logger variable
            LoggerSettings = errorLogger;

            //Runs the basic requirements validation
            Validation();
        }

        /// <summary>
        /// This function sets up the SQL Server connection based on an SQL user connection
        /// </summary>
        /// <param name="server">database server instance name</param>
        /// <param name="username">the SQL username to connect with</param>
        /// <param name="password">the SQL user password to connect with</param>
        /// <param name="database">database to connect to</param>   
        /// <param name="port">the port to connect to the SQL Server instance</param>
        /// <param name="connectionTimeout">connection timeout value in seconds</param>
        /// <param name="errorLogger">the error logger settings</param>
        /// <param name="additionalConnectionString">any additional extra parameters in connection string format</param>
        public SQLServerConnection(string server, string username, string password, string database, int port, int connectionTimeout = 30, OutputManagementSettings errorLogger = null, string additionalConnectionString = null)
        {
            //Sets the server to connect to
            Server = server;

            //Sets the username to use
            User = username;

            //Sets the password to use
            Password = password;

            //Sets the database to connect to
            Database = database;

            //Sets the database port to connect to
            Port = port;

            //Sets the connection timeout
            ConnectionTimeout = connectionTimeout;

            //Sets any extra connection string parameters
            ConnectionStringExtra = additionalConnectionString;

            //Tells the connection builder we don't want windows authentication
            IntegratedSecurity = null;

            //puts the logger settings into the logger variable
            LoggerSettings = errorLogger;

            //Runs the basic requirements validation
            Validation();
        }

        /// <summary>
        /// This function sets up the SQL Server connection based on a windows user connection
        /// </summary>
        /// <param name="server">database server instance name</param>
        /// <param name="database">database to connect to</param>        
        /// <param name="connectionTimeout">connection timeout value in seconds</param>
        /// <param name="errorLogger">the error logger settings</param>
        /// <param name="additionalConnectionString">any additional extra parameters in connection string format</param>
        public SQLServerConnection(string server, string database, int connectionTimeout = 30, OutputManagementSettings errorLogger = null, string additionalConnectionString = null)
        {
            Server = server;
            Database = database;
            Port = -1;//This tells the connection builder not to add a port field
            ConnectionTimeout = connectionTimeout;
            LoggerSettings = errorLogger;
            ConnectionStringExtra = additionalConnectionString;

            //Sets the server to connect to
            Server = server;
            
            //Sets the database to connect to
            Database = database;

            //This tells the connection builder not to add a port field
            Port = -1;

            //Sets the connection timeout
            ConnectionTimeout = connectionTimeout;

            //Sets any extra connection string parameters
            ConnectionStringExtra = additionalConnectionString;

            //Tells the connection builder we want windows authentication
            IntegratedSecurity = "SSPI";

            //puts the logger settings into the logger variable
            LoggerSettings = errorLogger;

            //Runs the basic requirements validation
            Validation();
        }

        /// <summary>
        /// This function sets up the SQL Server connection based on a windows user connection
        /// </summary>
        /// <param name="server">database server instance name</param>
        /// <param name="database">database to connect to</param>        
        /// <param name="port">the port to connect to the SQL Server instance</param>
        /// <param name="connectionTimeout">connection timeout value in seconds</param>
        /// <param name="errorLogger">the error logger settings</param>
        /// <param name="additionalConnectionString">any additional extra parameters in connection string format</param>
        public SQLServerConnection(string server, string database, int port, int connectionTimeout = 30, OutputManagementSettings errorLogger = null, string additionalConnectionString = null)
        {
            //Sets the server to connect to
            Server = server;
            
            //Sets the database to connect to
            Database = database;

            //Sets the database port to connect to
            Port = port;

            //Sets the connection timeout
            ConnectionTimeout = connectionTimeout;

            //Sets any extra connection string parameters
            ConnectionStringExtra = additionalConnectionString;

            //Tells the connection builder we want windows authentication
            IntegratedSecurity = "SSPI";

            //puts the logger settings into the logger variable
            LoggerSettings = errorLogger;

            //Runs the basic requirements validation
            Validation();
        }

        /// <summary>
        /// This is the core dispose function for the connection details
        /// </summary>
        public void Dispose()
        {
            Server = null;
            User = null;
            Password = null;
            Database = null;
            IntegratedSecurity = null;
            Port = 0;
            ConnectionTimeout = 0;
            ConnectionStringExtra = null;
            LoggerSettings = null;
        }
    }
    
    /// <summary>
    /// The main core class for the SQL Server Base
    /// </summary>
    public partial class SqlServerCore
    {
        /*######################################################*/
        /*    Database Connection String Compiling functions    */
        /*######################################################*/

        /// <summary>
        /// Builds the SQL Server windows connection string
        /// </summary>
        protected void WindowsAuthConnectionString()
        {
            string extra = (string.IsNullOrWhiteSpace(connectionStringExtra) ? "" : connectionStringExtra);
            if (port == -1)
            {
                connection = $"Server={server};Database={db};Integrated Security={integratedSecurity};connection timeout={connectionTime};{extra}";
            }
            else
            {
                connection = $"Server={server},{port};Database={db};Integrated Security={integratedSecurity};connection timeout={connectionTime};{extra}";
            }
        }

        /// <summary>
        /// Builds the SQL Server user connection string
        /// </summary>
        protected void SqlAuthConnectionString()
        {
            string extra = (string.IsNullOrWhiteSpace(connectionStringExtra) ? "" : connectionStringExtra);
            if (port == -1)
            {
                connection = $"Server={server};Database={db};User Id={user};Password={pwd};connection timeout={connectionTime};{extra}";
            }
            else
            {
                connection = $"Server={server},{port};Database={db};User Id={user};Password={pwd};connection timeout={connectionTime};{extra}";
            }
        }

        /*##########################################*/
        /*      Database connection functions       */
        /*##########################################*/

        /// <summary>
        /// This initialises the SQL Server connection
        /// </summary>
        /// <param name="connectionInformation">the SQL Server Connection Object</param>
        public SqlServerCore(SQLServerConnection connectionInformation)
        {
            if(connectionInformation != null)
            {
                //Checks if the connection details are using windows authentication
                if(
                    !string.IsNullOrWhiteSpace(connectionInformation.IntegratedSecurity) && 
                    (connectionInformation.IntegratedSecurity.ToLower() == "true" || connectionInformation.IntegratedSecurity.ToLower() == "yes" || connectionInformation.IntegratedSecurity.ToLower() == "sspi")
                  )
                {
                    //Sets the connection string variables
                    db = connectionInformation.Database;
                    server = connectionInformation.Server;
                    port = connectionInformation.Port;
                    connectionTime = connectionInformation.ConnectionTimeout;
                    loggerDetails = connectionInformation.LoggerSettings;
                    connectionStringExtra = connectionInformation.ConnectionStringExtra;
                    isWindowsAuth = true;
                    integratedSecurity = connectionInformation.IntegratedSecurity;

                    //Creates the connection string
                    WindowsAuthConnectionString();
                }
                else
                {
                    //Sets the connection string variables
                    db = connectionInformation.Database;
                    user = connectionInformation.User;
                    pwd = connectionInformation.Password;
                    server = connectionInformation.Server;
                    port = connectionInformation.Port;
                    connectionTime = connectionInformation.ConnectionTimeout;
                    loggerDetails = connectionInformation.LoggerSettings;
                    connectionStringExtra = connectionInformation.ConnectionStringExtra;
                    isWindowsAuth = false;

                    //Creates the connection string
                    SqlAuthConnectionString();
                }

                //Initialises the connection
                ConnectionInit();
            }
            else
            {
                throw new Exception("SQL Server Connection Error: A connection object has not been supplied.");
            }
        }

        /// <summary>
        /// This function checks to see if the connection information allows a connections or not
        /// </summary>
        /// <returns></returns>
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

        private void ConnectionInit()
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