namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*         Compiling Wrapper functions      */
        /*##########################################*/

        protected virtual void CompileStartWrapper(query current)
        {
            if (current.sql_start_wrapper != "")
            {
                compiled_build += " " + current.sql_start_wrapper;
                current.sql_start_wrapper = "";
            }
        }

        protected virtual void CompileEndWrapper(query current)
        {
            if (current.sql_end_wrapper != "")
            {
                compiled_build += " " + current.sql_end_wrapper;
                current.sql_end_wrapper = "";
            }
        }
    }
}