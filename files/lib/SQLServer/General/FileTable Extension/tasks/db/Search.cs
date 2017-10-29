using System;
using System.Data;

namespace DotNetSDB.SqlServer.FileTable
{
    public partial class SQLServerFileTableExtension
    {			
		public partial class FileTableTasks
		{
			/// <summary>
            /// This function searchs all the file content in the filetable for a specific phrase
            /// </summary>
            /// <param name="tableName"></param>
            /// <param name="searchContext"></param>
            /// <returns></returns>
			public DataTable search_all_file_context(string tableName, string searchContext)
			{
				try
				{
					connector.DB.add_pure_sql($@"
					select *, CONVERT(varchar(max), file_stream) as text_data from {tableName}
					where CONVERT(varchar(max), file_stream) like '%{connector.DB.add_pure_sql_bind(searchContext)}%'");
					return connector.DB.run_return_datatable();
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}

            /// <summary>
            /// This function searchs all the file content from a folder in the filetable for a specific phrase
            /// </summary>
            /// <param name="tableName"></param>
            /// <param name="parentDirectoryID"></param>
            /// <param name="searchContext"></param>
            /// <returns></returns>
			public DataTable search_all_file_context_in_folder(string tableName, string parentDirectoryID, string searchContext)
			{
				try
				{
					string locator = get_path_locator(tableName, parentDirectoryID);
					connector.DB.add_pure_sql($@"
					select *, CONVERT(varchar(max), file_stream) as text_data from {tableName}
					where CONVERT(varchar(max), file_stream) like '%{connector.DB.add_pure_sql_bind(searchContext)}%'
					and parent_path_locator = '{connector.DB.add_pure_sql_bind(locator)}'");
					return connector.DB.run_return_datatable();
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}

            /// <summary>
            /// This function searchs all the file content in the root the filetable for a specific phrase
            /// </summary>
            /// <param name="tableName"></param>
            /// <param name="searchContext"></param>
            /// <returns></returns>
			public DataTable search_all_file_context_in_root(string tableName, string searchContext)
			{
				try
				{
					connector.DB.add_pure_sql($@"
					select *, CONVERT(varchar(max), file_stream) as text_data from {tableName}
					where CONVERT(varchar(max), file_stream) like '%{connector.DB.add_pure_sql_bind(searchContext)}%'
					and parent_path_locator is NULL");
					return connector.DB.run_return_datatable();
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}
		}
    }
    
}