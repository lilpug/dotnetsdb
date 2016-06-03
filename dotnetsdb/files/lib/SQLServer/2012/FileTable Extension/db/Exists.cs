using System;

namespace DotNetSDB
{
    public partial class SqlServer2012 : SqlServerCore
    {
        public partial class filetable_extension
        {
            public bool database_id_exists(string tableName, string streamID)
            {
                try
                {
                    db.add_select(tableName, "stream_id");
                    db.add_where_normal(tableName, "stream_id", streamID);
                    string id = db.run_return_string();
                    if (!string.IsNullOrWhiteSpace(id))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    throw CustomErrorOutput(e);
                }
            }
        }
    }
}