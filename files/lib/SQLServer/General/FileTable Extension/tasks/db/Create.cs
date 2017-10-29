using System;

namespace DotNetSDB.SqlServer.FileTable
{
    public partial class SQLServerFileTableExtension
    {
		public partial class FileTableTasks
		{		
            /// <summary>
            /// This function creates a filetable
            /// </summary>
            /// <param name="tableName"></param>
            /// <param name="directoryName"></param>
			public void create_table(string tableName, string directoryName)
			{
				try
				{
					connector.DB.add_pure_sql($@"
						CREATE TABLE {tableName} AS FileTable
						WITH (
								FileTable_Directory = '{directoryName}',
								FileTable_Collate_Filename = database_default
								)");
					connector.DB.run();
				}
				catch (Exception e)
				{
					throw errorHandler.CustomErrorOutput(e);
				}
			}
		}
    }
    
}