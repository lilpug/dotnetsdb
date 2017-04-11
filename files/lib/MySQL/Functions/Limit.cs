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
            theQuery.limit = $"LIMIT {start}";
        }

        //This function is the mysql limit sql builder
        protected void LimitCompile(Query2 theQuery, int start, int end)
        {
            theQuery.limit = $"LIMIT {start}, {end}";
        }        

        /*##########################################*/
        /*           Main Front functions           */
        /*##########################################*/

        /// <summary>
        /// This function adds the limit statement
        /// </summary>
        /// <param name="maxRows"></param>        
        public virtual void add_limit(int maxRows)
        {
            if (maxRows <= 0)
            {
                throw new Exception($"The limit start value starts at 1 and above, yours is currently '{maxRows.ToString()}'.");
            }            
            Query theMain = GetQuery();
            Query2 theQuery = GetQuery2();
            LimitCompile(theQuery, maxRows);
            theMain.orderList.Add("limit");
        }

        /// <summary>
        /// This function adds the limit statement
        /// </summary>
        /// <param name="startLocation"></param>
        /// <param name="numberOfRows"></param>
        public virtual void add_limit(int startLocation, int numberOfRows)
        {
            if (startLocation < 0)
            {
                throw new Exception($"The limit start value starts at 0 and above, yours is currently '{startLocation}'.");
            }
            if (numberOfRows < startLocation)
            {
                throw new Exception($"The limit end value starts before the start value, start value: '{startLocation}', end value: '{numberOfRows}'.");
            }
            Query theMain = GetQuery();
            Query2 theQuery = GetQuery2();
            LimitCompile(theQuery, startLocation, numberOfRows);
            theMain.orderList.Add("limit");
        }
    }
}