namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*          Compiling Delete functions      */
        /*##########################################*/

        /// <summary>
        /// This function compiles the delete query and adds it to the query being built
        /// </summary>
        /// <param name="current"></param>
        protected virtual void CompileDelete(Query current)
        {
            compiledSql.Append($" DELETE FROM {current.DeleteTable}");
        }
    }
}