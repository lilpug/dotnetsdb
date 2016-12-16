namespace DotNetSDB
{
    public abstract partial class SqlServerCore
    {
        /*##########################################*/
        /*          Compiling Delete functions      */
        /*##########################################*/

        protected override void CompileDelete(query current)
        {
            int index = theQueries.IndexOf(current);
            query2 current2 = theQueries2[index];
            
            //Calculates if the first entry should be the alias if one is supplied
            string[] aliasTableSplit = current.delete_table.Split(' ');
            string alias = (aliasTableSplit.Length == 2) ? aliasTableSplit[1] : current.delete_table;

            compiled_build += string.Format(" DELETE {0} {1} FROM {2}", alias, ((current2.delete_returned) ? "OUTPUT DELETED.*" : ""), current.delete_table);
            current.delete_table = "";
            current2.delete_returned = false;
        }
    }
}