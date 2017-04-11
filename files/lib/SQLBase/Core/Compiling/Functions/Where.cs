namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*           Compiling Where functions      */
        /*##########################################*/

        protected virtual void CompileWhere(Query current)
        {
            //This is done as the where and operator counters should be out by 1 as the first element will not have an operator defaulty added
            //this is because its the start 'WHERE'. So we process that first then make the counters the same so we can use them both there after.
            if (current.whereStatements.Count != current.whereStatementsTypes.Count)
            {
                compiledSql += $" WHERE {current.whereStatements[0]} ";
                current.whereStatements.RemoveAt(0);
            }
            else
            {
                compiledSql += $" {current.whereStatementsTypes[0]} {current.whereStatements[0]} ";
                current.whereStatements.RemoveAt(0);
                current.whereStatementsTypes.RemoveAt(0);
            }
        }
    }
}