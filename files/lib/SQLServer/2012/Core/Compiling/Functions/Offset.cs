namespace DotNetSDB
{
    public partial class SQLServer2012 : SqlServerCore
    {
        /*##########################################*/
        /*          Compiling Offset functions      */
        /*##########################################*/

        protected virtual void CompileOffset(Query3 current)
        {
            compiledSql += $" {current.offset}";
            current.offset = "";
        }
    }
}