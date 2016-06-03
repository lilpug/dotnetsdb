using System;
using System.Linq;

namespace DotNetSDB
{
    public partial class MysqlCore
    {
        /*##########################################*/
        /*            Exist SQL function             */
        /*##########################################*/

        /// <summary>
        /// This function checks if a table exists
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool table_exist(string tableName)
        {
            query2 theQuery = get_query2();

            theQuery.exist_real_table_value = tableName + "%";
            compiled_build = "select * from INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME LIKE " + exist_definition + "_1_0_0 AND TABLE_SCHEMA = '" + db + "'";

            try
            {
                //Runs the query
                string[] results = run_return_array();

                //Checks if the return is correct or not
                if (results.Count() >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                //Clears the queries ready for the next
                disposeAll();
                compiled_build = "";
                return false;
            }
        }
    }
}