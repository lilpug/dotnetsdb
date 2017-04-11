namespace DotNetSDB
{
    public partial class SqlServer2014 : SqlServerCore
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