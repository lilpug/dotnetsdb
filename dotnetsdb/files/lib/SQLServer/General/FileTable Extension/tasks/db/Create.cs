using System;

namespace DotNetSDB.SqlServer.FileTable
{
    public partial class SqlServerFileTableExtension
    {
		public partial class FileTableTasks
		{			
			public void create_table(string tableName, string directoryName)
			{
				try
				{
					connector.db.add_pure_sql(String.Format(@"
						CREATE TABLE {0} AS FileTable
						WITH (
								FileTable_Directory = '{1}',
								FileTable_Collate_Filename = database_default
								)
						", tableName, directoryName));
					connector.db.run();
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}
		}
    }
    
}