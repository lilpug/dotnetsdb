using System;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*          Compiling Select functions      */
        /*##########################################*/

        protected virtual void CompileSelect(Query current)
        {
            //This does not use the number as there can only be one main select for a query
            string distinct = (current.isDistinct) ? "DISTINCT " : "";
            compiledSql += $"Select {distinct} {string.Join(",", current.selectFields).TrimEnd(',')} FROM {current.selectTable}";
            current.selectTable = "";
            current.selectFields.Clear();
        }
    }
}