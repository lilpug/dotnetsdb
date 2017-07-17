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
            string distinct = ((current.isDistinct) ? "DISTINCT" : "");
            string top = ((theQueries2[index].selectTop > 0) ? $"Top {theQueries2[index].selectTop}" : "");

            compiledSql.Append($"Select {distinct} {top} {string.Join(",", current.selectFields).TrimEnd(',')} FROM {current.selectTable}");
            current.selectTable = "";
            current.selectFields.Clear();
        }
    }
}