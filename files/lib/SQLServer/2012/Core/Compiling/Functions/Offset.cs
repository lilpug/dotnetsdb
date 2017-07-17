namespace DotNetSDB
{
    public partial class SQLServer2012 : SqlServerCore
    {
        /*##########################################*/
        /*          Compiling Offset functions      */
        /*##########################################*/

        protected virtual void CompileOffset(Query3 current)
        {
            compiledSql.Append($" {current.offset}");
            current.offset = "";
        }
    }
}