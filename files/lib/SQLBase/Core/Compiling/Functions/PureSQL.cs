namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*        Compiling Pure Sql functions      */
        /*##########################################*/

        protected virtual void CompilePureSQL(Query current)
        {
            compiledSql.Append($" {current.pureSql[0]}");
            current.pureSql.RemoveAt(0);
        }
    }
}