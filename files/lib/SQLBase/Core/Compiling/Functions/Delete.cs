namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*          Compiling Delete functions      */
        /*##########################################*/

        protected virtual void CompileDelete(Query current)
        {
            compiledSql += $" DELETE FROM {current.deleteTable}";
            current.deleteTable = "";
        }
    }
}