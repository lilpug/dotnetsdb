using System;
namespace DotNetSDB.Connector
{
    public sealed partial class DatabaseConnector : IDisposable
    {
        //Database object initiator and checker functions 

        internal void SqlServer2016(SqlServer2016 sqlserver)
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

        internal void SqlServer2014(SqlServer2014 sqlserver)
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

        internal void SqlServer2012(SqlServer2012 sqlserver)
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

        internal void SqlServer2008(SqlServer2008 sqlserver)
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

        internal void mysqlCore(MySLQCore mysqlDb)
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