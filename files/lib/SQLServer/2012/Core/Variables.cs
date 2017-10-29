﻿using DotNetSDB.SqlServer.FileTable;

namespace DotNetSDB
{
    public partial class SQLServer2012
    {
        /*##########################################*/
        /*       FileTable Extension Variable       */
        /*##########################################*/

        /// <summary>
        /// This variable stores the SQL Server filetable extension object
        /// </summary>
        public SQLServerFileTableExtension Filetable { get; set; }
    }
}