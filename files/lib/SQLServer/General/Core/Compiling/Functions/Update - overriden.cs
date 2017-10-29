using System;

namespace DotNetSDB
{
    public partial class SqlServerCore
    {
        /*##########################################*/
        /*          Compiling Select functions      */
        /*##########################################*/

        /// <summary>
        /// This function compiles the update query and adds it to the query being built
        /// </summary>
        /// <param name="current"></param>
        protected override void CompileUpdate(Query current)
        {
            //Converts the query object to QueryExtension
            QueryExtension theQuery = (QueryExtension)current;

            //Calculates if the first entry should be the alias if one is supplied
            string[] aliasTableSplit = theQuery.UpdateTable.Split(' ');
            string alias = (aliasTableSplit.Length == 2) ? aliasTableSplit[1] : theQuery.UpdateTable;

            string returnOutput = ((theQuery.UpdateReturned) ? " OUTPUT INSERTED.* " : " ");
            compiledSql.Append($" UPDATE {alias} SET {string.Join(", ", theQuery.UpdateFields).TrimEnd(',')} {returnOutput} FROM {theQuery.UpdateTable}");

            theQuery.UpdateFields.Clear();
            theQuery.UpdateTable = "";
            theQuery.UpdateReturned = false;
        }
    }
}