namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*            Compiling Drop functions      */
        /*##########################################*/

        protected virtual void CompileDrop(Query current)
        {
            compiledSql.Append($" DROP TABLE {current.dropTableName}");
            current.dropTableName = "";
        }
    }
}