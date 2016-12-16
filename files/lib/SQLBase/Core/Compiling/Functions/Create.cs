using System;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*          Compiling Create functions      */
        /*##########################################*/

        protected virtual void CompileCreate(query current)
        {
            compiled_build += string.Format(" CREATE TABLE {0} ({1})", current.create_table, String.Join(",", current.create_fields).TrimEnd(','));
            current.create_fields.Clear();
            current.create_table = "";
        }
    }
}