using System;

namespace DotNetSDB
{
    public partial class MySLQCore
    {
        /*##########################################*/
        /*        Limit Compiling functions         */
        /*##########################################*/

        //This function is the mysql limit sql builder
        protected void LimitCompile(Query2 theQuery, int start)
        {
            theQuery.limit = string.Format("LIMIT {0}", start);
        }

        //This function is the mysql limit sql builder
        protected void LimitCompile(Query2 theQuery, int start, int end)
        {
            theQuery.limit = string.Format("LIMIT {0}, {1}", start, end);
        }        

        /*##########################################*/
        /*           Main Front functions           */
        /*##########################################*/

        /// <summary>
        /// This function adds the limit statement
        /// </summary>
        /// <param name="startLocation"></param>        
        public void add_limit(int startLocation)
        {
            if (startLocation <= 0)
            {
                throw new Exception(string.Format("The limit start value starts at 1 and above, yours is currently '{0}'.", startLocation.ToString()));
            }            
            Query theMain = GetQuery();
            Query2 theQuery = GetQuery2();
            LimitCompile(theQuery, startLocation);
            theMain.orderList.Add("limit");
        }

        /// <summary>
        /// This function adds the limit statement
        /// </summary>
        /// <param name="startLocation"></param>
        /// <param name="numberOfRows"></param>
        public void add_limit(int startLocation, int numberOfRows)
        {
            if (startLocation < 0)
            {
                throw new Exception(string.Format("The limit start value starts at 0 and above, yours is currently '{0}'.", startLocation.ToString()));
            }
            if (numberOfRows < startLocation)
            {
                throw new Exception(string.Format("The limit end value starts before the start value, start value: '{0}', end value: '{1}'.", startLocation.ToString(), numberOfRows.ToString()));
            }
            Query theMain = GetQuery();
            Query2 theQuery = GetQuery2();
            LimitCompile(theQuery, startLocation, numberOfRows);
            theMain.orderList.Add("limit");
        }
    }
}