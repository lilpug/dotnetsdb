
namespace DotNetSDB
{
    public partial class SQLServer2008 : SqlServerCore
    {
        /*##########################################*/
        /*         Compiling OrderBy functions      */
        /*##########################################*/

        /// <summary>
        /// This function compiles the order by query and adds it to the query being built
        /// </summary>
        /// <param name="current"></param>
        protected override void CompileOrderBy(Query current)
        {
            //Converts the query object to QueryExtension
            QueryExtension2 theQuery = (QueryExtension2)current;

            string orderby = $" ORDER BY {string.Join(", ", theQuery.OrderbyFields).TrimEnd(',')} ";
            
            //This does not use the number as there can only be one main select for a query
            compiledSql.Append(orderby);
            
            //Stores the orderby for later on if they are using the limit wrapper in 2008
            theQuery.Orderby = orderby;
        }
    }
}