namespace DotNetSDB
{
    public partial class SQLServer2012 : SqlServerCore
    {
        /*##########################################*/
        /*          Compiling Offset functions      */
        /*##########################################*/

        /// <summary>
        /// This function compiles the offset query and adds it to the query being built
        /// </summary>
        /// <param name="current"></param>
        protected virtual void CompileOffset(Query current)
        {
            //Converts the query object to QueryExtension
            QueryExtension2 theQuery = (QueryExtension2)current;

            compiledSql.Append($" {theQuery.Offset}");
        }
    }
}