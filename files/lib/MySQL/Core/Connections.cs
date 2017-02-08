﻿using DotNetSDB.output;
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
                connection = string.Format("Server={0};Database={1};UId={2};Pwd={3};Connection Timeout={4};AllowZeroDateTime=true;ConvertZeroDatetime=True", server, db, user, pwd, connectionTime.ToString());
            }
            else
            {
                connection = string.Format("Server={0},{1};Database={2};UId={3};Pwd={4};Connection Timeout={5};AllowZeroDateTime=true;ConvertZeroDatetime=True", server, port, db, user, pwd, connectionTime.ToString());
            }
        }
       
        /*##########################################*/
        /*      Database connection functions       */
        /*##########################################*/

        public MysqlCore(MySQLUserConnection connectionInformation)
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