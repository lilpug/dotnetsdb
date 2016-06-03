using System;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*          Compiling Select functions      */
        /*##########################################*/

        protected virtual void CompileSelect(query current)
        {
            //This does not use the number as there can only be one main select for a query
            compiled_build += string.Format("Select {0} {1} FROM {2}", (current.is_dinstinct) ? "DISTINCT " : "", String.Join(",", current.select_fields).TrimEnd(','), current.select_table);
            current.select_table = "";
            current.select_fields.Clear();
        }
    }
}