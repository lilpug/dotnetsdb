namespace DotNetSDB
{
    public partial class SQLServer2008 : SqlServerCore
    {
        /*##########################################*/
        /*          Compiling Limit functions       */
        /*##########################################*/

        protected virtual void CompileLimit(Query3 current)
        {
            string temp = $" {LimitCompile(current, current.orderby, compiledSql.ToString())}";
            compiledSql.Clear();
            compiledSql.Append(temp);
            current.limitCountOne = -1;
            current.limitCountTwo = -1;
            current.orderby = "";
        }
    }
}