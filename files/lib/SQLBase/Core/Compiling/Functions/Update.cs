using System;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*          Compiling Update functions      */
        /*##########################################*/

        protected virtual void CompileUpdate(Query current)
        {
            compiledSql += $" UPDATE {current.updateTable} SET {string.Join(", ", current.updateFields).TrimEnd(',')}";

            current.updateFields.Clear();
            current.updateTable = "";
        }
    }
}