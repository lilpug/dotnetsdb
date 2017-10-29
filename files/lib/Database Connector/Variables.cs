using System;

namespace DotNetSDB.Connector
{
    /// <summary>
    /// This is the DatabaseConnector class which is used as a bridge to store all connections via dynamic objects
    /// </summary>
    public sealed partial class DatabaseConnector : IDisposable
    {
        /// <summary>
        /// Can stores the SQL Server 2016 database object
        /// </summary>
        public SQLServer2016 Sqlserver2016 { get; set; }

        /// <summary>
        /// Can stores the SQL Server 2014 database object
        /// </summary>
        public SQLServer2014 Sqlserver2014 { get; set; }

        /// <summary>
        /// Can stores the SQL Server 2012 database object
        /// </summary>
        public SQLServer2012 Sqlserver2012 { get; set; }
        
        /// <summary>
        /// Can stores the SQL Server 2008 database object
        /// </summary>
        public SQLServer2008 Sqlserver2008 { get; set; }

        /// <summary>
        /// Can stores the MySQL database object
        /// </summary>
        public MySQLCore MySQL { get; set; }

        /// <summary>
        /// Constructor for determining which database connection has been passed
        /// </summary>
        /// <param name="dbObject"></param>
        public DatabaseConnector(object dbObject)
        {
            if (dbObject.GetType() == typeof(SQLServer2016))
            {
                SqlServer2016((SQLServer2016)dbObject);
            }
            else if (dbObject.GetType() == typeof(SQLServer2014))
            {
                SqlServer2014((SQLServer2014)dbObject);
            }
            else if (dbObject.GetType() == typeof(SQLServer2012))
            {
                SqlServer2012((SQLServer2012)dbObject);
            }
            else if (dbObject.GetType() == typeof(SQLServer2008))
            {
                SqlServer2008((SQLServer2008)dbObject);
            }
            else if (dbObject.GetType() == typeof(MySQLCore))
            {
                MysqlCore((MySQLCore)dbObject);
            }
            else
            {
                throw new Exception("The type of database object that has been passed is not valid.");
            }
        }

        /// <summary>
        /// Core variable for determining if the object has already been disposed of
        /// </summary>
        private bool isDisposed = false;

        /// <summary>
        /// This is the core dispose method for the database connector object
        /// </summary>
        public void Dispose()
        {
            if (!isDisposed)
            {
                if (Sqlserver2016 != null)
                {
                    Sqlserver2016.Dispose();
                }

                if (Sqlserver2014 != null)
                {
                    Sqlserver2014.Dispose();
                }

                if (Sqlserver2012 != null)
                {
                    Sqlserver2012.Dispose();
                }

                if (Sqlserver2008 != null)
                {
                    Sqlserver2008.Dispose();
                }

                if (MySQL != null)
                {
                    MySQL.Dispose();
                }

                isDisposed = true;
            }
        }
    }
}