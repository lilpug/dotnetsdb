namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*        Compiling Pure Sql functions      */
        /*##########################################*/

        /// <summary>
        /// This function compiles the pure SQL query and adds it to the query being built
        /// </summary>
        /// <param name="current"></param>
        protected virtual void CompilePureSQL(Query current)
        {
            //Compiles the first in the list and adds it to the query
            compiledSql.Append($" {current.PureSql[0]}");

            //Removes the current first so if there are multiple the next will be at 0 as well
            current.PureSql.RemoveAt(0);
        }
    }
}