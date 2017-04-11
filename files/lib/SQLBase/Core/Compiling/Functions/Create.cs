using System;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*          Compiling Create functions      */
        /*##########################################*/

        protected virtual void CompileCreate(Query current)
        {
            compiledSql += $" CREATE TABLE {current.createTable} ({string.Join(",", current.createFields).TrimEnd(',')})";
            current.createFields.Clear();
            current.createTable = "";
        }
    }
}