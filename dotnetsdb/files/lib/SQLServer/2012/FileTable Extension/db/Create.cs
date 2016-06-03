using System;

namespace DotNetSDB
{
    public partial class SqlServer2012 : SqlServerCore
    {
        public partial class filetable_extension
        {
            public void create_table(string tableName, string directoryName)
            {
                try
                {
                    db.add_pure_sql(String.Format(@"
                        CREATE TABLE {0} AS FileTable
                        WITH (
                                FileTable_Directory = '{1}',
                                FileTable_Collate_Filename = database_default
                             )
                        ", tableName, directoryName));
                    db.run();
                }
                catch (Exception e)
                {
                    throw CustomErrorOutput(e);
                }
            }
        }
    }
}