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

            string returnOutput = ((current2.deleteReturned) ? "OUTPUT DELETED.*" : "");

            compiledSql.Append($" DELETE {alias} {returnOutput} FROM {current.deleteTable}");
            current.deleteTable = "";
            current2.deleteReturned = false;
        }
    }
}