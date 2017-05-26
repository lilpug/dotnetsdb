﻿using System.Data.SqlClient; //Used for sqlCommand etc

namespace DotNetSDB
{
    public partial class SQLServer2008 : SqlServerCore
    {
        /*##########################################*/
        /*      Database connection functions       */
        /*##########################################*/

        //This is the constructor which initiases the connection to the database (overload function)
        public SQLServer2008(SQLServerUserConnection connectionInformation)
            : base(connectionInformation)
        {
        }

        //This is the constructor which initiases the connection to the database via windows authentication (overload function)
        public SQLServer2008(SQLServerWindowsConnection connectionInformation)
            : base(connectionInformation)
        {
        }
    }
}