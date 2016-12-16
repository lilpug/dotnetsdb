using System;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*           Compiling Group functions      */
        /*##########################################*/

        protected virtual void CompileGroup(query current)
        {
            compiled_build += string.Format(" GROUP BY {0}", String.Join(",", current.groupby_fields).TrimEnd(','));
            current.groupby_fields.Clear();
        }
    }
}