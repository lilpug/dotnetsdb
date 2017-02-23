using System;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*         Compiling OrderBy functions      */
        /*##########################################*/

        protected virtual void CompileOrderBy(Query current)
        {
            string orderby = string.Format(" ORDER BY {0}", string.Join(", ", current.orderbyFields).TrimEnd(','));
            //This does not use the number as there can only be one main select for a query
            compiledSql += orderby;
            current.orderbyFields.Clear();
        }
    }
}