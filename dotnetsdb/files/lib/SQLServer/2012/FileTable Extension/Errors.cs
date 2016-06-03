using System;

namespace DotNetSDB
{
    public partial class SqlServer2012 : SqlServerCore
    {
        public partial class filetable_extension
        {
            private Exception CustomErrorOutput(Exception e)
            {
                //Checks if the exception is caused by the filestream not being enabled
                if (e.Message.IndexOf("Default FILESTREAM filegroup is not available in database") != -1)
                {
                    return new Exception("Database FileTable Create: The filetable permissions are not enabled on this database, please use the 'database_enable_filetable_permissions' function and ensure you have enabled FileStream on the sql server instance in the configuration Manager.");
                }
                return e;
            }
        }
    }
}