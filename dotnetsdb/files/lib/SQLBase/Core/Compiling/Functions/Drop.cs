namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*            Compiling Drop functions      */
        /*##########################################*/

        protected virtual void CompileDrop(query current)
        {
            compiled_build += string.Format(" DROP TABLE {0}", current.drop_table_name);
            current.drop_table_name = "";
        }
    }
}