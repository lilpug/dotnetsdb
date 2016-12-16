namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*            Compiling Join functions      */
        /*##########################################*/

        protected virtual void CompileJoin(query current)
        {
            compiled_build += " " + current.join_fields[0];
            current.join_fields.RemoveAt(0);
        }
    }
}