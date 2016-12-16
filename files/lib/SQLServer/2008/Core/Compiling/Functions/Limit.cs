namespace DotNetSDB
{
    public partial class SqlServer2008 : SqlServerCore
    {
        /*##########################################*/
        /*          Compiling Limit functions       */
        /*##########################################*/

        protected virtual void CompileLimit(query3 current)
        {
            compiled_build = " " + limit_create(current, current.orderby, compiled_build);
            current.limit_count_one = -1;
            current.limit_count_two = -1;
            current.orderby = "";
        }
    }
}