using System;
using System.Linq;

namespace DotNetSDB
{
    public partial class MySLQCore
    {
        /*##########################################*/
        /*            Exist SQL function             */
        /*##########################################*/

        /// <summary>
        /// This function checks if a table exists
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public virtual bool table_exist(string tableName)
        {
            Query2 theQuery = GetQuery2();
            theQuery.exist_real_table_value = AddData(tableName + "%");
            compiledSql = string.Format("select * from INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME LIKE {0}_1_0_0 AND TABLE_SCHEMA = '{1}'", existDefinition, db);
            
            //Runs the query
            string[] results = run_return_string_array();

            //Checks if the return is correct or not
            if (results != null && results.Count() >= 1)
            {
                return true;
            }
                
            return false;
        }
    }
}