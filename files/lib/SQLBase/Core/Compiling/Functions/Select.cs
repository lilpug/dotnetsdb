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
            compiledSql += string.Format("Select {0} {1} FROM {2}", (current.isDistinct) ? "DISTINCT " : "", string.Join(",", current.selectFields).TrimEnd(','), current.selectTable);
            current.selectTable = "";
            current.selectFields.Clear();
        }
    }
}