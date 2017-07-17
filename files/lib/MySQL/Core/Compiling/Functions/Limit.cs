namespace DotNetSDB
{
    public partial class MySQLCore
    {
        /*##########################################*/
        /*          Compiling Insert functions      */
        /*##########################################*/

        protected virtual void CompileLimit(Query2 current)
        {
            compiledSql.Append($" {current.limit}");
            current.limit = "";
        }
    }
}