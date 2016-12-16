namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*          Compiling Delete functions      */
        /*##########################################*/

        protected virtual void CompileDelete(query current)
        {
            compiled_build += string.Format(" DELETE FROM {0}", current.delete_table);
            current.delete_table = "";
        }
    }
}