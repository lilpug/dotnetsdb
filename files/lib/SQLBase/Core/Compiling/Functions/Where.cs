namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*           Compiling Where functions      */
        /*##########################################*/

        /// <summary>
        /// This function compiles the where query and adds it to the query being built
        /// </summary>
        /// <param name="current"></param>
        protected virtual void CompileWhere(Query current)
        {
            //This is done as the where and operator counters should be out by 1 as the first element will not have an operator defaulty added
            //this is because its the start 'WHERE'. So we process that first then make the counters the same so we can use them both there after.
            if (current.WhereStatements.Count != current.WhereStatementsTypes.Count)
            {
                compiledSql.Append($" WHERE {current.WhereStatements[0]} ");
                current.WhereStatements.RemoveAt(0);
            }
            else
            {
                compiledSql.Append($" {current.WhereStatementsTypes[0]} {current.WhereStatements[0]} ");
                current.WhereStatements.RemoveAt(0);
                current.WhereStatementsTypes.RemoveAt(0);
            }
        }
    }
}