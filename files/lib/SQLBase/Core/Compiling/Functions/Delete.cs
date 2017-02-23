namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*          Compiling Delete functions      */
        /*##########################################*/

        protected virtual void CompileDelete(Query current)
        {
            compiledSql += string.Format(" DELETE FROM {0}", current.deleteTable);
            current.deleteTable = "";
        }
    }
}