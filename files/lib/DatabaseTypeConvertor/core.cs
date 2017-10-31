using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Linq;

namespace DotNetSDB
{
    /// <summary>
    /// Converts a base data type to a database data type
    /// </summary>
    public partial class DatabaseTypeConvertor
    {
        /// <summary>
        /// Converts a db type to .Net data type
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public Type ToNetType(DbType dbType)
        {
            DbTypeMapEntry entry = Find(dbType);
            return entry.Type;
        }

        /// <summary>
        /// Converts a SQL Server type to a .Net data type
        /// </summary>
        /// <param name="sqlDbType"></param>
        /// <returns></returns>
        public Type ToNetType(SqlDbType sqlDbType)
        {
            DbTypeMapEntry entry = Find(sqlDbType);
            return entry.Type;
        }

        /// <summary>
        /// Converts a MySQL type to a .Net data type
        /// </summary>
        /// <param name="mySqlDbType"></param>
        /// <returns></returns>
        public Type ToNetType(MySqlDbType mySqlDbType)
        {
            DbTypeMapEntry entry = Find(mySqlDbType);
            return entry.Type;
        }

        /// <summary>
        /// Converts a .Net type to a Db type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public DbType ToDbType(Type type)
        {
            DbTypeMapEntry entry = Find(type);
            return entry.DbType;
        }

        /// <summary>
        /// Converts a SQL Server data type to a DbType
        /// </summary>
        /// <param name="sqlDbType"></param>
        /// <returns></returns>
        public DbType ToDbType(SqlDbType sqlDbType)
        {
            DbTypeMapEntry entry = Find(sqlDbType);
            return entry.DbType;
        }

        /// <summary>
        /// Converts a MySQL data type to a DbType
        /// </summary>
        /// <param name="mySqlDbType"></param>
        /// <returns></returns>
        public DbType ToDbType(MySqlDbType mySqlDbType)
        {
            DbTypeMapEntry entry = Find(mySqlDbType);
            return entry.DbType;
        }

        /// <summary>
        /// Converts a .Net type to a SQL Server data type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public SqlDbType ToSqlDbType(Type type)
        {
            DbTypeMapEntry entry = Find(type);
            return entry.SQLserverDbType;
        }

        /// <summary>
        /// Converts a DbType to a SQL Server data type
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public SqlDbType ToSqlDbType(DbType dbType)
        {
            DbTypeMapEntry entry = Find(dbType);
            return entry.SQLserverDbType;
        }

        /// <summary>
        /// Converts a .Net type to a MySQL data type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public MySqlDbType ToMySqlDbType(Type type)
        {
            DbTypeMapEntry entry = Find(type);
            return entry.MySQLDbType;
        }

        /// <summary>
        /// Converts DbType to a MySQL data type
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public MySqlDbType ToMySqlDbType(DbType dbType)
        {
            DbTypeMapEntry entry = Find(dbType);
            return entry.MySQLDbType;
        }
        
        private DbTypeMapEntry Find(Type type)
        {
            //Checks if the type exists in the list
            var temp = from item in TypeList.AsEnumerable()
                       where item.Type == type && (item.DBVersion == null || item.DBVersion.Length == 0 || (hasDBVersion && item.DBVersion.Contains(dbVersion)))
                       select item;

            //Checks if we have atleast one entry
            if (temp != null && temp.FirstOrDefault() != null)
            {
                //Returns it if so
                return temp.First();
            }
            else
            {
                throw new ApplicationException("Database Type Converter: data was used which is an unsupported Type");
            }
        }

        private DbTypeMapEntry Find(DbType dbType)
        {
            //Checks if the type exists in the list
            var temp = from item in TypeList.AsEnumerable()
                       where item.DbType == dbType && (item.DBVersion == null || item.DBVersion.Length == 0 || (hasDBVersion && item.DBVersion.Contains(dbVersion)))
                       select item;

            //Checks if we have atleast one entry
            if (temp != null && temp.FirstOrDefault() != null)
            {
                //Returns it if so
                return temp.First();
            }
            else
            {
                throw new ApplicationException("Database Type Converter: data was used which is an unsupported DbType");
            }
        }

        private DbTypeMapEntry Find(SqlDbType sqlDbType)
        {
            //Checks if the type exists in the list
            var temp = from item in TypeList.AsEnumerable()
                       where item.SQLserverDbType == sqlDbType && (item.DBVersion == null || item.DBVersion.Length == 0 || (hasDBVersion && item.DBVersion.Contains(dbVersion)))
                       select item;

            //Checks if we have atleast one entry
            if(temp != null && temp.FirstOrDefault() != null)
            {
                //Returns it if so
                return temp.First();
            }
            else
            {
                throw new ApplicationException("Database Type Converter: data was used which is an unsupported SqlDbType");
            }
        }

        private DbTypeMapEntry Find(MySqlDbType mySqlDbType)
        {
            //Checks if the type exists in the list
            var temp = from item in TypeList.AsEnumerable()
                       where item.MySQLDbType == mySqlDbType && (item.DBVersion == null || item.DBVersion.Length == 0 || (hasDBVersion && item.DBVersion.Contains(dbVersion)))
                       select item;

            //Checks if we have atleast one entry
            if (temp != null && temp.FirstOrDefault() != null)
            {
                //Returns it if so
                return temp.First();
            }
            else
            {
                throw new ApplicationException("Database Type Converter: data was used which is an unsupported MySqlDbType");
            }
        }
    }
}