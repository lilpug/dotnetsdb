namespace DotNetSDB
{
    public partial class MySLQCore
    {
        /*##########################################*/
        /*          Compiling Insert functions      */
        /*##########################################*/

        protected virtual void CompileLimit(Query2 current)
        {
            compiledSql += string.Format(" {0}", current.limit);
            current.limit = "";
        }
    }
}