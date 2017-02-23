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
            compiledSql += string.Format(" UPDATE {0} SET {1}", current.updateTable, String.Join(", ", current.updateFields).TrimEnd(','));

            current.updateFields.Clear();
            current.updateTable = "";
        }
    }
}