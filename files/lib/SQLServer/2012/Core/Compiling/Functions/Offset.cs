namespace DotNetSDB
{
    public partial class SqlServer2012 : SqlServerCore
    {
        /*##########################################*/
        /*          Compiling Offset functions      */
        /*##########################################*/

        protected virtual void CompileOffset(Query3 current)
        {
            compiledSql += string.Format(" {0}", current.offset);
            current.offset = "";
        }
    }
}