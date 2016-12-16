using System;

namespace DotNetSDB
{
    public partial class SqlServer2016 : SqlServerCore
    {
        /*##########################################*/
        /*       Offset Compiling functions         */
        /*##########################################*/

        //This functions adds the variables ready for compiling the offset query        
        protected void offset_build_compiling(query3 theQuery, int offset, int fetch)
        {
            theQuery.offset = string.Format("OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", offset, fetch);
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
                throw new Exception(string.Format("The offset start value starts at 0 and above, yours is currently '{0}'.", offsetRows.ToString()));
            }
            if (numberOfRows < 0)
            {
                throw new Exception(string.Format("The fetch rows cannot be below zero, yours is currently '{0}'.", numberOfRows.ToString()));
            }
            query theMain = get_query();
            query3 theQuery = get_query3();
            offset_build_compiling(theQuery, offsetRows, numberOfRows);
            theMain.orderList.Add("offset");
        }
    }
}