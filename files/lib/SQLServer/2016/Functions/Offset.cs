using System;

namespace DotNetSDB
{
    public partial class SQLServer2016 : SqlServerCore
    {
        /*##########################################*/
        /*       Offset Compiling functions         */
        /*##########################################*/

        /// <summary>
        /// This function deals with creating the Offset SQL
        /// </summary>
        /// <param name="theQuery"></param>
        /// <param name="offset"></param>
        /// <param name="fetch"></param>
        protected void OffsetCompile(QueryExtension2 theQuery, int offset, int fetch)
        {
            theQuery.Offset = $"OFFSET {offset} ROWS FETCH NEXT {fetch} ROWS ONLY";
        }

        /*##########################################*/
        /*           Main Front function            */
        /*##########################################*/

        /// <summary>
        /// Note: This function requires an orderby before using it
        /// </summary>
        /// <param name="offsetRows"></param>
        /// <param name="numberOfRows"></param>
        public void add_offset(int offsetRows, int numberOfRows)
        {
            if (offsetRows < 0)
            {
                throw new Exception($"The offset start value starts at 0 and above, yours is currently '{offsetRows}'.");
            }
            if (numberOfRows < 0)
            {
                throw new Exception($"The fetch rows cannot be below zero, yours is currently '{numberOfRows}'.");
            }

            //Converts the query object to QueryExtension
            QueryExtension2 theQuery = (QueryExtension2)GetQuery();

            OffsetCompile(theQuery, offsetRows, numberOfRows);
            theQuery.OrderList.Add("offset");
        }
    }
}