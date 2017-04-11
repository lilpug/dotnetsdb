namespace DotNetSDB
{
    public partial class SqlServer2008 : SqlServerCore
    {
        /*##########################################*/
        /*          Compiling Limit functions       */
        /*##########################################*/

        protected virtual void CompileLimit(Query3 current)
        {
            compiledSql = $" {LimitCompile(current, current.orderby, compiledSql)}";
            current.limitCountOne = -1;
            current.limitCountTwo = -1;
            current.orderby = "";
        }
    }
}