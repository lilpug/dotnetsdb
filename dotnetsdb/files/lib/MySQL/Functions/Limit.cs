using System;

namespace DotNetSDB
{
    public partial class MysqlCore
    {
        /*##########################################*/
        /*        Limit Compiling functions         */
        /*##########################################*/

        //This function is the mysql limit sql builder
        protected void limit_create(query2 theQuery, int start, int end)
        {
            theQuery.limit = "LIMIT " + (start - 1).ToString() + ", " + (end - start + 1).ToString();
        }

        /*##########################################*/
        /*           Main Front functions           */
        /*##########################################*/

        /// <summary>
        /// This function adds the limit statement
        /// </summary>
        /// <param name="startLocation"></param>
        /// <param name="endLocation"></param>
        public void add_limit(int startLocation, int endLocation)
        {
            if (startLocation <= 0)
            {
                throw new Exception(string.Format("The limit start value starts at 1 and above, yours is currently '{0}'.", startLocation.ToString()));
            }
            query theMain = get_query();
            query2 theQuery = get_query2();
            limit_create(theQuery, startLocation, endLocation);
            theMain.orderList.Add("limit");
        }
    }
}