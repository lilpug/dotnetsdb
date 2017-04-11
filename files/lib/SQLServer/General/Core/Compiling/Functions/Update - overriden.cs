using System;

namespace DotNetSDB
{
    public partial class SqlServerCore
    {
        /*##########################################*/
        /*          Compiling Select functions      */
        /*##########################################*/

        //Overrides the default to accept Top for Sql Server
        protected override void CompileUpdate(Query current)
        {
            int index = theQueries.IndexOf(current);
            Query2 current2 = theQueries2[index];

            //Calculates if the first entry should be the alias if one is supplied
            string[] aliasTableSplit = current.updateTable.Split(' ');
            string alias = (aliasTableSplit.Length == 2) ? aliasTableSplit[1] : current.updateTable;

            string returnOutput = ((current2.updateReturned) ? " OUTPUT INSERTED.* " : " ");
            compiledSql += $" UPDATE {alias} SET {string.Join(", ", current.updateFields).TrimEnd(',')} {returnOutput} FROM {current.updateTable}";

            current.updateFields.Clear();
            current.updateTable = "";
            current2.updateReturned = false;
        }
    }
}