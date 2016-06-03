namespace DotNetSDB
{
    public partial class MysqlCore
    {
        /*##########################################*/
        /*          Compiling Insert functions      */
        /*##########################################*/

        protected virtual void CompileLimit(query2 current)
        {
            compiled_build += " " + current.limit;
            current.limit = "";
        }
    }
}