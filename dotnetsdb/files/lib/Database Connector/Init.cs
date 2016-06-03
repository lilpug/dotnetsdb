using DotNetSDB;
using System;
namespace DotNetSDB.Connector
{
    public partial class DatabaseConnector : IDisposable
    {
        //Database object initiator and checker functions 

        private void SqlServer2012(SqlServer2012 sqlserver)
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

        private void SqlServer2008(SqlServer2008 sqlserver)
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

        private void mysqlCore(MysqlCore mysqlDb)
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