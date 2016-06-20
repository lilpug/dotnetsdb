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
        }

        //This is the constructor which initiases the connection to the database via windows authentication (overload function)
        public SqlServer2016(SQLServerWindowsConnection connectionInformation)
            : base(connectionInformation)
        {
        }

        //This is the constructor which allows the user to pass in a sqlconnection object for connecting
        public SqlServer2016(SqlConnection theConnection)
            : base(theConnection)
        {
        }

        //This is the constructor which allows the user to pass in a connection string for connecting
        public SqlServer2016(string sqlConnectionString)
            : base(sqlConnectionString)
        {
        }
    }
}