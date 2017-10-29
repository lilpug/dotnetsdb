namespace DotNetSDB
{
    public partial class SQLServer2008 : SqlServerCore
    {
        /*##########################################*/
        /*          Compiling Limit functions       */
        /*##########################################*/

        /// <summary>
        /// This function compiles the limit query wrapper and adds it to the query being built
        /// </summary>
        /// <param name="current"></param>
        protected virtual void CompileLimit(Query current)
        {
            //Converts the query object to QueryExtension
            QueryExtension2 theQuery = (QueryExtension2)current;
            
            string temp = $" {LimitCompile(theQuery, theQuery.Orderby, compiledSql.ToString())}";

            //Wipes the query and then adds it back as this is the wrapper for the entire query
            compiledSql.Clear();
            compiledSql.Append(temp);
        }
    }
}