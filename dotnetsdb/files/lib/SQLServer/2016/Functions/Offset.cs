using System;

namespace DotNetSDB
{
    public partial class SqlServer2016 : SqlServerCore
    {
        /*##########################################*/
        /*       Offset Compiling functions         */
        /*##########################################*/

        //This functions adds the variables ready for compiling the offset query
        //Note: first = f -1, last = last - f + 1
        protected void offset_build_compiling(query3 theQuery, int first, int last)
        {
            theQuery.offset = string.Format("OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", (first - 1), (last - first));
        }

        /*##########################################*/
        /*           Main Front function            */
        /*##########################################*/

        /// <summary>
        /// Note: This function requires an orderby before using it
        /// </summary>
        /// <param name="startLocation"></param>
        /// <param name="endLocation"></param>
        public void add_offset(int startLocation, int endLocation)
        {
            if (startLocation <= 0)
            {
                throw new Exception(string.Format("The offset start value starts at 1 and above, yours is currently '{0}'.", startLocation.ToString()));
            }
            query theMain = get_query();
            query3 theQuery = get_query3();
            offset_build_compiling(theQuery, startLocation, endLocation);
            theMain.orderList.Add("offset");
        }
    }
}