using System;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*         Compiling OrderBy functions      */
        /*##########################################*/

        /// <summary>
        /// This function compiles the orderby query and adds it to the query being built
        /// </summary>
        /// <param name="current"></param>
        protected virtual void CompileOrderBy(Query current)
        {   
            compiledSql.Append($" ORDER BY {string.Join(", ", current.OrderbyFields).TrimEnd(',')}");            
        }
    }
}