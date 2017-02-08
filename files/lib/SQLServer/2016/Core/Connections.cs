using DotNetSDB.Connector;
using DotNetSDB.SqlServer.FileTable;
using System.Data.SqlClient; //Used for sqlCommand etc

namespace DotNetSDB
{
    public partial class SqlServer2016 : SqlServerCore
    {
        /*##########################################*/
        /*      Database connection functions       */
        /*##########################################*/

        //This is the constructor which initiases the connection to the database (overload function)
        public SqlServer2016(SQLServerUserConnection connectionInformation)
            : base(connectionInformation)
        {
            filetable = new SqlServerFileTableExtension(new DatabaseConnector(this), db);
        }

        //This is the constructor which initiases the connection to the database via windows authentication (overload function)
        public SqlServer2016(SQLServerWindowsConnection connectionInformation)
            : base(connectionInformation)
        {
            filetable = new SqlServerFileTableExtension(new DatabaseConnector(this), db);
        }
    }
}