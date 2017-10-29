using System;
using System.Linq;

namespace DotNetSDB
{
    public partial class MySQLCore
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
            //Converts the query object to QueryExtension
            QueryExtension theQuery = (QueryExtension)GetQuery();
            
            theQuery.ExistRealTableValue = AddData(tableName + "%");
            compiledSql.Append($"select * from INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME LIKE {existDefinition}_1_0_0 AND TABLE_SCHEMA = '{db}'");
            
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