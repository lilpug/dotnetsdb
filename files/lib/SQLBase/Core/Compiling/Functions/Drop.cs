namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*            Compiling Drop functions      */
        /*##########################################*/

        protected virtual void CompileDrop(Query current)
        {
            compiledSql += string.Format(" DROP TABLE {0}", current.dropTableName);
            current.dropTableName = "";
        }
    }
}