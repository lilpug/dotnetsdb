using System;
namespace DotNetSDB.Connector
{
    public partial class DatabaseConnector : IDisposable
    {
        //Returns the database object
        public dynamic db
        {
            get
            {
                if (sqlserver2012 != null)
                {
                    return sqlserver2012;
                }
                else if (sqlserver2008 != null)
                {
                    return sqlserver2008;
                }
                else if (mysql != null)
                {
                    return mysql;
                }
                else
                {
                    return null;
                }
            }
        }

        //Returns the database version being used
        public string dbVersion
        {
            get
            {
                if (sqlserver2012 != null)
                {
                    return "sqlserver2012";
                }
                else if (sqlserver2008 != null)
                {
                    return "sqlserver2008";
                }
                else if (mysql != null)
                {
                    return "mysql";
                }
                else
                {
                    return null;
                }
            }
        }

        //Returns if the database object is using mysql or not
        public bool isDbMysql
        {
            get
            {
                if (sqlserver2012 != null || sqlserver2008 != null)
                {
                    return false;
                }
                else //mysql
                {
                    return true;
                }
            }
        }

        //Returns if the database object is using sql server or not
        public bool isDbSqlServer
        {
            get
            {
                if (mysql != null)
                {
                    return false;
                }
                else //sqlserver
                {
                    return true;
                }
            }
        }

        public Type dbType
        {
            get
            {
                return db.GetType();
            }
        }
    }
}