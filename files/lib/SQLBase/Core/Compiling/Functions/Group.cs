using System;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*           Compiling Group functions      */
        /*##########################################*/

        /// <summary>
        /// This function compiles the group query and adds it to the query being built
        /// </summary>
        /// <param name="current"></param>
        protected virtual void CompileGroup(Query current)
        {
            compiledSql.Append($" GROUP BY {string.Join(",", current.GroupbyFields).TrimEnd(',')}");
        }
    }
}