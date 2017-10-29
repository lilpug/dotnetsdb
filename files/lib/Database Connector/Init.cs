using System;
namespace DotNetSDB.Connector
{
    public sealed partial class DatabaseConnector : IDisposable
    {
        //These functions are the database object initiator and checker functions 

        internal void SqlServer2016(SQLServer2016 sqlserver)
        {
            if (sqlserver.is_alive())
            {
                Sqlserver2016 = sqlserver;
            }
            else
            {
                throw new Exception("Error: the sql server connection is not active");
            }
        }

        internal void SqlServer2014(SQLServer2014 sqlserver)
        {
            if (sqlserver.is_alive())
            {
                Sqlserver2014 = sqlserver;
            }
            else
            {
                throw new Exception("Error: the sql server connection is not active");
            }
        }

        internal void SqlServer2012(SQLServer2012 sqlserver)
        {
            if (sqlserver.is_alive())
            {
                Sqlserver2012 = sqlserver;
            }
            else
            {
                throw new Exception("Error: the sql server connection is not active");
            }
        }

        internal void SqlServer2008(SQLServer2008 sqlserver)
        {
            if (sqlserver.is_alive())
            {
                Sqlserver2008 = sqlserver;
            }
            else
            {
                throw new Exception("Error: the sql server connection is not active");
            }
        }

        internal void MysqlCore(MySQLCore mysqlDb)
        {
            if (MySQL.is_alive())
            {
                MySQL = mysqlDb;
            }
            else
            {
                throw new Exception("Error: the mysql connection is not active");
            }
        }
    }
}