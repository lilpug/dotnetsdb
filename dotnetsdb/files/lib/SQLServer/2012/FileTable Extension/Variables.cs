﻿using System;

namespace DotNetSDB
{
    public partial class SqlServer2012 : SqlServerCore
    {
        public partial class filetable_extension
        {
            private SqlServer2012 db;

            public filetable_extension(SqlServer2012 databaseObject)
            {
                db = databaseObject;
                if (!db.is_alive())
                {
                    throw new Exception("Database FileTable Initialisation: The database object is not connected.");
                }
            }
        }
    }
}