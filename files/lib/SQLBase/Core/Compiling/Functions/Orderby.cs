using System;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*         Compiling OrderBy functions      */
        /*##########################################*/

        protected virtual void CompileOrderBy(query current)
        {
            string orderby = string.Format(" ORDER BY {0}", String.Join(", ", current.orderby_fields).TrimEnd(','));
            //This does not use the number as there can only be one main select for a query
            compiled_build += orderby;
            current.orderby_fields.Clear();
        }
    }
}