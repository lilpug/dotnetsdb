using System;
namespace DotNetSDB.Connector
{
    public sealed partial class DatabaseConnector : IDisposable
    {
        /// <summary>
        /// Returns a dynamic database object
        /// </summary>
        public dynamic DB
        {
            get
            {
                if (Sqlserver2016 != null)
                {
                    return Sqlserver2016;
                }
                else if (Sqlserver2014 != null)
                {
                    return Sqlserver2014;
                }    
                else if (Sqlserver2012 != null)
                {
                    return Sqlserver2012;
                }
                else if (Sqlserver2008 != null)
                {
                    return Sqlserver2008;
                }
                else if (MySQL != null)
                {
                    return MySQL;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Returns the database version being used
        /// </summary>
        public string DBVersion
        {
            get
            {
                if (Sqlserver2016 != null)
                {
                    return "sqlserver2016";
                }
                else if (Sqlserver2014 != null)
                {
                    return "sqlserver2014";
                }
                else if (Sqlserver2012 != null)
                {
                    return "sqlserver2012";
                }
                else if (Sqlserver2008 != null)
                {
                    return "sqlserver2008";
                }
                else if (MySQL != null)
                {
                    return "mysql";
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Returns if the database object is using MySQL or not
        /// </summary>
        public bool IsDbMysql
        {
            get
            {
                if (Sqlserver2016 != null || Sqlserver2014 != null || Sqlserver2012 != null || Sqlserver2008 != null)
                {
                    return false;
                }
                else //mysql
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Returns if the database object is using SQL Server or not
        /// </summary>
        public bool IsDbSqlServer
        {
            get
            {
                if (MySQL != null)
                {
                    return false;
                }
                else //sqlserver
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Returns the current database class type
        /// </summary>
        public Type DbType
        {
            get
            {
                return DB.GetType();
            }
        }
    }
}