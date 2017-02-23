using System;

namespace DotNetSDB
{
    public partial class SqlServerCore
    {
        /*##########################################*/
        /*          Compiling Select functions      */
        /*##########################################*/

        //Overrides the default to accept Top for Sql Server
        protected override void CompileSelect(Query current)
        {
            int index = theQueries.IndexOf(current);

            //This does not use the number as there can only be one main select for a query
            compiledSql += string.Format("Select {0} {1} {2} FROM {3}", ((current.isDistinct) ? "DISTINCT" : ""),
                ((theQueries2[index].selectTop > 0) ? string.Format("Top {0}", theQueries2[index].selectTop) : ""),
                string.Join(",", current.selectFields).TrimEnd(','), current.selectTable);
            current.selectTable = "";
            current.selectFields.Clear();
        }
    }
}