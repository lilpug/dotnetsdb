namespace DotNetSDB
{
    /// <summary>
    /// The SQL Server 2008 class
    /// </summary>
    public partial class SQLServer2008 : SqlServerCore
    {
        /*##########################################*/
        /*      Database connection functions       */
        /*##########################################*/

        /// <summary>
        /// Initialises the SQL Server connection with the supplied connection object
        /// </summary>
        /// <param name="connectionInformation">the SQL Server Connection Object</param>
        public SQLServer2008(SQLServerConnection connectionInformation)
            : base(connectionInformation)
        {
        }
    }
}