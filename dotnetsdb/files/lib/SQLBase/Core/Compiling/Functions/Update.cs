using System;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*          Compiling Update functions      */
        /*##########################################*/

        protected virtual void CompileUpdate(query current)
        {
            compiled_build += string.Format(" UPDATE {0} SET {1}", current.update_table, String.Join(", ", current.update_fields).TrimEnd(','));

            current.update_fields.Clear();
            current.update_table = "";
        }
    }
}