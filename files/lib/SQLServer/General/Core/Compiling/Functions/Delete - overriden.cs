namespace DotNetSDB
{
    public abstract partial class SqlServerCore
    {
        /*##########################################*/
        /*          Compiling Delete functions      */
        /*##########################################*/

        protected override void CompileDelete(Query current)
        {
            int index = theQueries.IndexOf(current);
            Query2 current2 = theQueries2[index];
            
            //Calculates if the first entry should be the alias if one is supplied
            string[] aliasTableSplit = current.deleteTable.Split(' ');
            string alias = (aliasTableSplit.Length == 2) ? aliasTableSplit[1] : current.deleteTable;

            compiledSql += string.Format(" DELETE {0} {1} FROM {2}", alias, ((current2.deleteReturned) ? "OUTPUT DELETED.*" : ""), current.deleteTable);
            current.deleteTable = "";
            current2.deleteReturned = false;
        }
    }
}