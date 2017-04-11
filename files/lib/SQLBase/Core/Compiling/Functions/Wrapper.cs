namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*         Compiling Wrapper functions      */
        /*##########################################*/

        protected virtual void CompileStartWrapper(Query current)
        {
            if (current.sqlStartWrapper != "")
            {
                compiledSql += $" {current.sqlStartWrapper}";
                current.sqlStartWrapper = "";
            }
        }

        protected virtual void CompileEndWrapper(Query current)
        {
            if (current.sqlEndWrapper != "")
            {
                compiledSql += $" {current.sqlEndWrapper}";
                current.sqlEndWrapper = "";
            }
        }
    }
}