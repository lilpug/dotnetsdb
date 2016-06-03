using System;

namespace DotNetSDB.Connector
{
    public partial class DatabaseConnector : IDisposable
    {
        //Storage objects
        public SqlServer2012 sqlserver2012;
        public SqlServer2008 sqlserver2008;
        public MysqlCore mysql;

        //Constructor for determining which database connection has been passed
        public DatabaseConnector(object dbObject)
        {
            if (dbObject.GetType() == typeof(SqlServer2012))
            {
                SqlServer2012((SqlServer2012)dbObject);
            }
            else if (dbObject.GetType() == typeof(SqlServer2008))
            {
                SqlServer2008((SqlServer2008)dbObject);
            }
            else if (dbObject.GetType() == typeof(MysqlCore))
            {
                mysqlCore((MysqlCore)dbObject);
            }
            else
            {
                throw new Exception("The type of database object that has been passed is not valid.");
            }
        }

        //Deconstructor
        public void Dispose()
        {
            if (sqlserver2008 != null)
            {
                sqlserver2008.Dispose();
            }

            if (sqlserver2012 != null)
            {
                sqlserver2012.Dispose();
            }

            if (mysql != null)
            {
                mysql.Dispose();
            }
        }
    }
}