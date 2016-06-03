using System;

namespace DotNetSDB
{
    public partial class MysqlCore
    {
        /*##########################################*/
        /*           Truncate SQL function          */
        /*##########################################*/

        //Note: This parameter can not be sanatized as its not in the where clause etc
        /// <summary>
        /// This function truncates the table
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool truncate_table(string tableName)
        {
            //Gives the sql string to the compiled_build
            compiled_build = "Truncate Table " + tableName;

            try
            {
                //Runs the query
                run();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}