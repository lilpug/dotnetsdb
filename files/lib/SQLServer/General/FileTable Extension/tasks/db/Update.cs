using System;

namespace DotNetSDB.SqlServer.FileTable
{
    public partial class SqlServerFileTableExtension
    {
		public partial class FileTableTasks
		{			
			public void update_name(string tableName, string newName, string streamID)
			{
				try
				{
                    connector.db.add_update(tableName, "name", newName);
                    connector.db.add_where_normal(tableName, "stream_id", streamID);
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