using System;

namespace DotNetSDB
{
    public partial class MySQLCore
    {
        /*##########################################*/
        /*        Limit Compiling functions         */
        /*##########################################*/

        /// <summary>
        /// This function is the MySQL limit SQL builder
        /// </summary>
        /// <param name="theQuery"></param>
        /// <param name="start"></param>
        protected void LimitCompile(QueryExtension theQuery, int start)
        {
            theQuery.Limit = $"LIMIT {start}";
        }

        /// <summary>
        /// This function is the MySQL limit SQL builder
        /// </summary>
        /// <param name="theQuery"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        protected void LimitCompile(QueryExtension theQuery, int start, int end)
        {
            theQuery.Limit = $"LIMIT {start}, {end}";
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

            //Converts the query object to QueryExtension
            QueryExtension theQuery = (QueryExtension)GetQuery();

            LimitCompile(theQuery, maxRows);
            theQuery.OrderList.Add("limit");
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

            //Converts the query object to QueryExtension
            QueryExtension theQuery = (QueryExtension)GetQuery();

            LimitCompile(theQuery, startLocation, numberOfRows);
            theQuery.OrderList.Add("limit");
        }
    }
}