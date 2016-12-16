namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*           Compiling Where functions      */
        /*##########################################*/

        protected virtual void CompileWhere(query current)
        {
            //This is done as the where and operator counters should be out by 1 as the first element will not have an operator defaulty added
            //this is because its the start 'WHERE'. So we process that first then make the counters the same so we can use them both there after.
            if (current.where_statements.Count != current.where_statement_types.Count)
            {
                compiled_build += string.Format(" WHERE {0} ", current.where_statements[0]);
                current.where_statements.RemoveAt(0);
            }
            else
            {
                compiled_build += string.Format(" {0} {1} ", current.where_statement_types[0], current.where_statements[0]);
                current.where_statements.RemoveAt(0);
                current.where_statement_types.RemoveAt(0);
            }
        }
    }
}