using System;

namespace DotNetSDB
{
    public partial class SqlServerCore
    {
        /*##########################################*/
        /*          Compiling Select functions      */
        /*##########################################*/

        //Overrides the default to accept Top for Sql Server
        protected override void CompileUpdate(query current)
        {
            int index = theQueries.IndexOf(current);
            query2 current2 = theQueries2[index];

            //Calculates if the first entry should be the alias if one is supplied
            string[] aliasTableSplit = current.update_table.Split(' ');
            string alias = (aliasTableSplit.Length == 2) ? aliasTableSplit[1] : current.update_table;

            compiled_build += string.Format(" UPDATE {0} SET {1} {2} FROM {3}", alias, string.Join(", ", current.update_fields).TrimEnd(','), ((current2.update_returned) ? " OUTPUT INSERTED.* " : " "), current.update_table);

            current.update_fields.Clear();
            current.update_table = "";
            current2.update_returned = false;
        }
    }
}