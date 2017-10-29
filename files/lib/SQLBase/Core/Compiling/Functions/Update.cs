using System;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*          Compiling Update functions      */
        /*##########################################*/

        /// <summary>
        /// This function compiles the update query and adds it to the query being built
        /// </summary>
        /// <param name="current"></param>
        protected virtual void CompileUpdate(Query current)
        {
            compiledSql.Append($" UPDATE {current.UpdateTable} SET {string.Join(", ", current.UpdateFields).TrimEnd(',')}");
        }
    }
}