namespace DotNetSDB
{
    public abstract partial class SqlServerCore
    {
        /*##########################################*/
        /*          Compiling Delete functions      */
        /*##########################################*/

        /// <summary>
        /// This function compiles the delete query and adds it to the query being built
        /// </summary>
        /// <param name="current"></param>
        protected override void CompileDelete(Query current)
        {
            //Converts the query object to QueryExtension
            QueryExtension theQuery = (QueryExtension)current;
            
            //Calculates if the first entry should be the alias if one is supplied
            string[] aliasTableSplit = theQuery.DeleteTable.Split(' ');
            string alias = (aliasTableSplit.Length == 2) ? aliasTableSplit[1] : theQuery.DeleteTable;

            string returnOutput = ((theQuery.DeleteReturned) ? "OUTPUT DELETED.*" : "");

            compiledSql.Append($" DELETE {alias} {returnOutput} FROM {theQuery.DeleteTable}");
        }
    }
}