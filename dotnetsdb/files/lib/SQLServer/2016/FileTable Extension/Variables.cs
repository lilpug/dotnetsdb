using System;

namespace DotNetSDB
{
    public partial class SqlServer2016 : SqlServerCore
    {
        public partial class filetable_extension
        {
            private SqlServer2016 db;

            public filetable_extension(SqlServer2016 databaseObject)
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