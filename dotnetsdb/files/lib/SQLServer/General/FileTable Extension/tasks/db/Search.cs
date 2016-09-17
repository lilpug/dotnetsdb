﻿using System;
using System.Data;

namespace DotNetSDB.SqlServer.FileTable
{
    public partial class SqlServerFileTableExtension
    {			
		public partial class FileTableTasks
		{
			
			public DataTable search_all_file_context(string tableName, string searchContext)
			{
				try
				{
					connector.db.add_pure_sql(String.Format(@"
					select *, CONVERT(varchar(max), file_stream) as text_data from {0}
					where CONVERT(varchar(max), file_stream) like '%{1}%'
					", tableName, connector.db.add_pure_sql_bind(searchContext)));
					return connector.db.run_return_datatable();
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}

			public DataTable search_all_file_context_in_folder(string tableName, string parentDirectoryID, string searchContext)
			{
				try
				{
					string locator = get_path_locator(tableName, parentDirectoryID);
					connector.db.add_pure_sql(String.Format(@"
					select *, CONVERT(varchar(max), file_stream) as text_data from {0}
					where CONVERT(varchar(max), file_stream) like '%{1}%'
					and parent_path_locator = '{2}'
					", tableName, connector.db.add_pure_sql_bind(searchContext), connector.db.add_pure_sql_bind(locator)));
					return connector.db.run_return_datatable();
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}

			public DataTable search_all_file_context_in_root(string tableName, string searchContext)
			{
				try
				{
					connector.db.add_pure_sql(String.Format(@"
					select *, CONVERT(varchar(max), file_stream) as text_data from {0}
					where CONVERT(varchar(max), file_stream) like '%{1}%'
					and parent_path_locator is NULL
					", tableName, connector.db.add_pure_sql_bind(searchContext)));
					return connector.db.run_return_datatable();
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}
		}
    }
    
}