namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*        Compiling Pure Sql functions      */
        /*##########################################*/

        protected virtual void CompilePureSQL(query current)
        {
            compiled_build += " " + current.pure_sql[0];
            current.pure_sql.RemoveAt(0);
        }
    }
}