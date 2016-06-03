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
        public int connectionTime { get; set; }

        public OutputManagementVariable logger { get; set; }

        public MySQLUserConnection(string serverName, string username, string password, string databaseName, int connectionTimeout = 30, OutputManagementVariable errorLogger = null)
        {
            server = serverName;
            user = username;
            pwd = password;
            dbName = databaseName;
            connectionTime = connectionTimeout;
            logger = errorLogger;
        }
    }

    //sql_server_database class (the IDispossible allows the class to be used with using statements)
    public partial class MysqlCore
    {
        /*##########################################*/
        /*       Database Compiling functions       */
        /*##########################################*/

        //This function processes the connection string from a sqlconnection object and breaks it into its seperate entities
        protected bool connection_decompile(MySqlConnection theConnection)
        {
            //This will be used to find the correct positions which is why its all lower case
            //Note: we do not extract the final data from this as its all been made lower case
            string temp = theConnection.ConnectionString.ToLower();

            //Sets all the variables up
            DecompileVariables(temp);

            //Checks the validity of the variables
            if (DecompileMinimumValidity())
            {
                connection = "Server=" + server + "; Database=" + db + "; UId=" + user + "; Pwd=" + pwd + ";connection timeout=" + connectionTime.ToString() + ";AllowZeroDateTime=true;ConvertZeroDatetime=True";
                return true;
            }

            return false;
        }

        //This function pulls out all the variable from a connection string
        private void DecompileVariables(string connectionString)
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

            string tempConnectionTimeout = GetBetweenStringValue(connectionString, "connection timeout=", ";");
            connectionTime = (!string.IsNullOrWhiteSpace(tempConnectionTimeout)) ? Convert.ToInt32(tempConnectionTimeout) : 30;
        }

        //This function ensures the minimum required variables for a connection are valid
        private bool DecompileMinimumValidity()
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

        //This disposes of the connection
        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                //Runs the base class Dispose
                base.Dispose(disposing);

                //Adds the personal one
                if (disposing)
                {
                    MySqlConnection.ClearPool(myConnection);
                    myConnection.Dispose();
                }
            }
        }

        //These functions allow the user to change the connection string after the object has been built with whichever definition they want
        public bool switch_connection(MySQLUserConnection connectionInformation)
        {
            //Sets the connection string
            db = connectionInformation.dbName;
            user = connectionInformation.user;
            pwd = connectionInformation.pwd;
            server = connectionInformation.server;
            connectionTime = connectionInformation.connectionTime;
            loggerDetails = connectionInformation.logger;

            //Recreates the connection string
            connection = string.Format("Server={0};Database={1};UId={2};Pwd={3};Connection Timeout={4};AllowZeroDateTime=true;ConvertZeroDatetime=True;", server, db, user, pwd, connectionTime);

            //Initialises the connection
            connectionInit();

            return true;
        }

        public bool switch_connection(MySqlConnection theConnection)
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
            //Pulls the variable out
            DecompileVariables(sqlConnectionString.ToLower());

            //Checks the validity of the variables
            if (DecompileMinimumValidity())
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