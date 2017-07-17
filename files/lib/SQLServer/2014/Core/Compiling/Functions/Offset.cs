namespace DotNetSDB
{
    public partial class SQLServer2014 : SqlServerCore
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