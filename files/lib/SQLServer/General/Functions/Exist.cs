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
            //Converts the query object to QueryExtension
            QueryExtension theQuery = (QueryExtension)GetQuery();

            theQuery.ExistRealTableValue = AddData(tableName);
            compiledSql.Append($"SELECT 1 FROM {db}.sys.tables WHERE name = {existDefinition}_1_0_0");
            
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