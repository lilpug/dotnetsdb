namespace DotNetSDB
{
    public partial class MySQLCore
    {
        /*##########################################*/
        /*          Compiling Insert functions      */
        /*##########################################*/

        protected virtual void CompileLimit(Query2 current)
        {
            compiledSql += $" {current.limit}";
            current.limit = "";
        }
    }
}