using System;
namespace DotNetSDB.Connector
{
    public sealed partial class DatabaseConnector : IDisposable
    {
        //Database object initiator and checker functions 

        internal void SqlServer2016(SQLServer2016 sqlserver)
        {
            if (sqlserver.is_alive())
            {
                sqlserver2016 = sqlserver;
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
                sqlserver2014 = sqlserver;
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
                sqlserver2012 = sqlserver;
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
                sqlserver2008 = sqlserver;
            }
            else
            {
                throw new Exception("Error: the sql server connection is not active");
            }
        }

        internal void mysqlCore(MySQLCore mysqlDb)
        {
            if (mysql.is_alive())
            {
                mysql = mysqlDb;
            }
            else
            {
                throw new Exception("Error: the mysql connection is not active");
            }
        }
    }
}