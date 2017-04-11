
namespace DotNetSDB
{
    public partial class SqlServer2008 : SqlServerCore
    {
        /*##########################################*/
        /*         Compiling OrderBy functions      */
        /*##########################################*/

        //Overrides the order by base function to add the orderby statement into the query3 object for the limit capability later
        protected override void CompileOrderBy(Query current)
        {
            string orderby = $" ORDER BY {string.Join(", ", current.orderbyFields).TrimEnd(',')} ";
            //This does not use the number as there can only be one main select for a query
            compiledSql += orderby;

            /* Extra Code Start */
            //Gets the index of the current query we are on
            int index = theQueries.IndexOf(current);
            theQueries3[index].orderby = orderby;
            /* Extra Code End */

            current.orderbyFields.Clear();
        }
    }
}