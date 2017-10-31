using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DotNetSDB
{
    /// <summary>
    /// Converts a base data type to a database data type
    /// </summary>
    public partial class DatabaseTypeConvertor
    {
        /// <summary>
        /// A class that stores a map from a standard .NET reference type to database reference types
        /// </summary>
        public class DbTypeMapEntry
        {
            /// <summary>
            /// Stores the database versions which can use this type
            /// Note: null means all!
            /// </summary>
            public string[] DBVersion { get; set; }

            /// <summary>
            /// Stores the .NET reference type
            /// </summary>
            public Type Type { get; set; }

            /// <summary>
            /// Stores the standard database reference type
            /// </summary>
            public DbType DbType { get; set; }

            /// <summary>
            /// Stores the SQL Server database reference type
            /// </summary>
            public SqlDbType SQLserverDbType { get; set; }

            /// <summary>
            /// /// <summary>
            /// Stores the MySQL database reference type
            /// </summary>
            /// </summary>
            public MySqlDbType MySQLDbType { get; set; }

            /// <summary>
            /// Constructor that takes the reference values and stores
            /// </summary>
            /// <param name="type"></param>
            /// <param name="dbType"></param>
            /// <param name="sqlServerDbType"></param>
            /// <param name="mySqlDbType"></param>
            /// <param name="dbVersions"></param>
            public DbTypeMapEntry(Type type, DbType dbType, SqlDbType sqlServerDbType, MySqlDbType mySqlDbType, params string[] dbVersions)
            {
                Type = type;
                DbType = dbType;
                SQLserverDbType = sqlServerDbType;
                MySQLDbType = mySqlDbType;
                DBVersion = dbVersions;
            }
        };

        /// <summary>
        /// This variable stores a list of the type mapper class
        /// </summary>
        public List<DbTypeMapEntry> TypeList { get; set; }

        /// <summary>
        /// This variable stores the current supplied database version
        /// </summary>
        private string dbVersion;

        /// <summary>
        /// This variable stores if a dbversion variable has been passed
        /// </summary>
        private bool hasDBVersion;

        /// <summary>
        /// This is the constructor for the SQL Server type mapper
        /// </summary>
        public DatabaseTypeConvertor(string dbVersion = null)
        {
            TypeList = new List<DbTypeMapEntry>();
            
            this.dbVersion = dbVersion;

            //Stores if the db version has been supplied
            //Note: this is done to save processing time on linq checks
            if(!string.IsNullOrWhiteSpace(this.dbVersion))
            {
                hasDBVersion = true;
            }
            LoadTypes();
        }
    }
}