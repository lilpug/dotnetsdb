using System;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*          Compiling Select functions      */
        /*##########################################*/

        /// <summary>
        /// This function compiles the select query and adds it to the query being built
        /// </summary>
        /// <param name="current"></param>
        protected virtual void CompileSelect(Query current)
        {   
            string distinct = (current.IsDistinct) ? "DISTINCT " : "";
            compiledSql.Append($"Select {distinct} {string.Join(",", current.SelectFields).TrimEnd(',')} FROM {current.SelectTable}");
        }
    }
}