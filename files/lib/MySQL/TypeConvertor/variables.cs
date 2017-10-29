using MySql.Data.MySqlClient;

//Convert .Net Type to SqlDbType or DbType and vise versa
//This class can be useful when you make conversion between types .The class supports conversion between .Net Type , SqlDbType and DbType .
//the original idea was from: //https://gist.github.com/abrahamjp/858392
//although i have modified type selectors to be more up to date i.e. datetime2
using System;
using System.Collections;
using System.Data;

namespace DotNetSDB
{
    /// <summary>
    /// Convert a base data type to another base data type
    /// </summary>
    public partial class MySqlTypeConvertor
    {
        /// <summary>
        /// A class that stores a map from a standard .NET reference type to database reference types
        /// </summary>
        public class DbTypeMapEntry
        {
            /// <summary>
            /// Stores the .NET type reference
            /// </summary>
            public Type Type { get; set; }

            /// <summary>
            /// Stores the standard database type reference
            /// </summary>
            public DbType DbType { get; set; }

            /// <summary>
            /// Stores the MySQL database type reference
            /// </summary>
            public MySqlDbType SqlDbType { get; set; }

            /// <summary>
            /// Constructor that takes the reference values and stores
            /// </summary>
            /// <param name="type"></param>
            /// <param name="dbType"></param>
            /// <param name="sqlDbType"></param>
            public DbTypeMapEntry(Type type, DbType dbType, MySqlDbType sqlDbType)
            {
                Type = type;
                DbType = dbType;
                SqlDbType = sqlDbType;
            }
        }

        /// <summary>
        /// Variable that stores an array list of the type mapper class
        /// </summary>
        public ArrayList TypeList { get; set; }
        
        /// <summary>
        /// This is the constructor for the MySQL type mapper
        /// </summary>
        public MySqlTypeConvertor()
        {
            TypeList = new ArrayList();
            LoadTypes();
        }
    }
}