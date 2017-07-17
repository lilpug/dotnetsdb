namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*          Compiling Delete functions      */
        /*##########################################*/

        protected virtual void CompileDelete(Query current)
        {
            compiledSql.Append($" DELETE FROM {current.deleteTable}");
            current.deleteTable = "";
        }
    }
}