using System;
using System.Linq;

namespace DotNetSDB
{
    public partial class SqlServerCore
    {
        /*##########################################*/
        /*            Exist SQL function             */
        /*##########################################*/

        /// <summary>
        /// This function returns whether a table exists
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public virtual bool table_exist(string tableName)
        {
            query2 theQuery = get_query2();
            theQuery.exist_real_table_value = add_data(tableName);
            compiled_build = "SELECT 1 FROM " + db + ".sys.tables WHERE name = " + exist_definition + "_1_0_0";
            
            //Runs the query
            string[] results = run_return_array();

            //Checks if the return is correct or not
            if (results != null && results.Count() >= 1)
            {
                return true;
            }
                
            return false;                            
        }
    }
}