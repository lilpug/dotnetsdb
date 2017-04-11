using System;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*           Compiling Group functions      */
        /*##########################################*/

        protected virtual void CompileGroup(Query current)
        {
            compiledSql += $" GROUP BY {string.Join(",", current.groupbyFields).TrimEnd(',')}";
            current.groupbyFields.Clear();
        }
    }
}