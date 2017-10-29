using System;

namespace DotNetSDB
{
    public partial class SqlServerCore
    {
        /*##########################################*/
        /*          Compiling Select functions      */
        /*##########################################*/

        /// <summary>
        /// This function compiles the select query and adds it to the query being built
        /// </summary>
        /// <param name="current"></param>
        protected override void CompileSelect(Query current)
        {
            //Converts the query object to QueryExtension
            QueryExtension theQuery = (QueryExtension)current;

            //This does not use the number as there can only be one main select for a query
            string distinct = ((theQuery.IsDistinct) ? "DISTINCT" : "");
            string top = ((theQuery.SelectTop > 0) ? $"Top {theQuery.SelectTop}" : "");

            compiledSql.Append($"Select {distinct} {top} {string.Join(",", theQuery.SelectFields).TrimEnd(',')} FROM {theQuery.SelectTable}");
        }
    }
}