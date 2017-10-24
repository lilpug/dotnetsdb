using System;

namespace DotNetSDB.SqlServer.FileTable
{
    public partial class SQLServerFileTableExtension
    {
		public partial class FileTableTasks
		{			
			public void create_table(string tableName, string directoryName)
			{
				try
				{
					connector.db.add_pure_sql($@"
						CREATE TABLE {tableName} AS FileTable
						WITH (
								FileTable_Directory = '{directoryName}',
								FileTable_Collate_Filename = database_default
								)");
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