namespace DotNetSDB
{
    public partial class MySQLCore
    {
        /*##########################################*/
        /*        Compiling Limit functions         */
        /*##########################################*/

        /// <summary>
        /// This function compiles the limit SQL
        /// </summary>
        /// <param name="current"></param>
        protected virtual void CompileLimit(Query current)
        {
            //Converts the query object to QueryExtension
            QueryExtension theQuery = (QueryExtension)current;

            compiledSql.Append($" {theQuery.Limit}");
            theQuery.Limit = null;
        }
    }
}