using DotNetSDB.Connector;
using DotNetSDB.SqlServer.FileTable;

namespace DotNetSDB
{
    /// <summary>
    /// The SQL Server 2016 class
    /// </summary>
    public partial class SQLServer2016 : SqlServerCore
    {
        /*##########################################*/
        /*      Database connection functions       */
        /*##########################################*/

        /// <summary>
        /// Initialises the SQL Server connection with the supplied connection object
        /// </summary>
        /// <param name="connectionInformation">the SQL Server Connection Object</param>
        public SQLServer2016(SQLServerConnection connectionInformation)
            : base(connectionInformation)
        {
            Filetable = new SQLServerFileTableExtension(new DatabaseConnector(this), db);
        }
    }
}