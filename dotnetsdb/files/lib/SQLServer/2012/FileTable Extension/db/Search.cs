using System;
using System.Data;

namespace DotNetSDB
{
    public partial class SqlServer2012 : SqlServerCore
    {
        public partial class filetable_extension
        {
            public DataTable search_all_file_context(string tableName, string searchContext)
            {
                try
                {
                    db.add_pure_sql(String.Format(@"
                    select *, CONVERT(varchar(max), file_stream) as text_data from {0}
                    where CONVERT(varchar(max), file_stream) like '%{1}%'
                    ", tableName, db.add_pure_sql_bind(searchContext)));
                    return db.run_return_datatable();
                }
                catch (Exception e)
                {
                    throw CustomErrorOutput(e);
                }
            }

            public DataTable search_all_file_context_in_directory(string tableName, string parentDirectoryID, string searchContext)
            {
                try
                {
                    string locator = get_path_locator(tableName, parentDirectoryID);
                    db.add_pure_sql(String.Format(@"
                    select *, CONVERT(varchar(max), file_stream) as text_data from {0}
                    where CONVERT(varchar(max), file_stream) like '%{1}%'
                    and parent_path_locator = '{2}'
                    ", tableName, db.add_pure_sql_bind(searchContext), db.add_pure_sql_bind(locator)));
                    return db.run_return_datatable();
                }
                catch (Exception e)
                {
                    throw CustomErrorOutput(e);
                }
            }

            public DataTable search_all_file_context_in_root(string tableName, string searchContext)
            {
                try
                {
                    db.add_pure_sql(String.Format(@"
                    select *, CONVERT(varchar(max), file_stream) as text_data from {0}
                    where CONVERT(varchar(max), file_stream) like '%{1}%'
                    and parent_path_locator is NULL
                    ", tableName, db.add_pure_sql_bind(searchContext)));
                    return db.run_return_datatable();
                }
                catch (Exception e)
                {
                    throw CustomErrorOutput(e);
                }
            }
        }
    }
}