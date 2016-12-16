using System;

namespace DotNetSDB
{
    public partial class SqlServerCore
    {
        /*##########################################*/
        /*          Compiling Select functions      */
        /*##########################################*/

        //Overrides the default to accept Top for Sql Server
        protected override void CompileSelect(query current)
        {
            int index = theQueries.IndexOf(current);

            //This does not use the number as there can only be one main select for a query
            compiled_build += string.Format("Select {0} {1} {2} FROM {3}", ((current.is_dinstinct) ? "DISTINCT" : ""),
                ((theQueries2[index].select_top > 0) ? string.Format("Top {0}", theQueries2[index].select_top) : ""),
                String.Join(",", current.select_fields).TrimEnd(','), current.select_table);
            current.select_table = "";
            current.select_fields.Clear();
        }
    }
}