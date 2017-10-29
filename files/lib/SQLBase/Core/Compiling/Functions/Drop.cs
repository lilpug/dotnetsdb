namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*            Compiling Drop functions      */
        /*##########################################*/

        /// <summary>
        /// This function compiles the drop query and adds it to the query being built
        /// </summary>
        /// <param name="current"></param>
        protected virtual void CompileDrop(Query current)
        {
            compiledSql.Append($" DROP TABLE {current.DropTableName}");
        }
    }
}