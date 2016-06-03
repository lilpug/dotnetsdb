namespace DotNetSDB
{
    public abstract partial class SqlServerCore
    {
        /*##########################################*/
        /*          Compiling Delete functions      */
        /*##########################################*/

        protected virtual void CompileDelete(query current)
        {
            int index = theQueries.IndexOf(current);
            query2 current2 = theQueries2[index];

            compiled_build += string.Format(" DELETE FROM {0} {1}", current.delete_table, ((current2.delete_returned) ? "OUTPUT DELETED.*" : ""));
            current.delete_table = "";
            current2.delete_returned = false;
        }
    }
}