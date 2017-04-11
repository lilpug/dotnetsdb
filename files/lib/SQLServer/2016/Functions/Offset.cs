using System;

namespace DotNetSDB
{
    public partial class SqlServer2016 : SqlServerCore
    {
        /*##########################################*/
        /*       Offset Compiling functions         */
        /*##########################################*/

        //This functions adds the variables ready for compiling the offset query        
        protected void OffsetCompile(Query3 theQuery, int offset, int fetch)
        {
            theQuery.offset = $"OFFSET {offset} ROWS FETCH NEXT {fetch} ROWS ONLY";
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
            Query theMain = GetQuery();
            Query3 theQuery = GetQuery3();
            OffsetCompile(theQuery, offsetRows, numberOfRows);
            theMain.orderList.Add("offset");
        }
    }
}