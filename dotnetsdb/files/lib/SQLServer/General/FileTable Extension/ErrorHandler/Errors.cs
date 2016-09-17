using System;

namespace DotNetSDB.SqlServer.FileTable
{    
    public partial class SqlServerFileTableExtension
    {
		private partial class ErrorHandler
        { 			
			//This function is used to add our own information if specific errors occur
			public Exception CustomErrorOutput(Exception e)
			{
				//Checks if the exception is caused by the filestream not being enabled
				if (e.Message.IndexOf("Default FILESTREAM filegroup is not available in database") != -1)
				{
					return new Exception("Database FileTable Create: The filetable permissions are not enabled on this database, please use the '.permissions.enable_all' function and ensure you have enabled FileStream on the sql server instance in the configuration Manager.");
				}
				/*else if(e.Message.IndexOf("Default FILESTREAM filegroup is not available in database") != -1)
				{
					return new Exception("Database FileTable Create: The filetable permissions are not enabled on this database, please use the '.permissions.enable_all' function and ensure you have enabled FileStream on the sql server instance in the configuration Manager.");
				}*/
				return e;
			}
		}
    }
    
}