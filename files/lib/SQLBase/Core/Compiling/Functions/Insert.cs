using System;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*          Compiling Insert functions      */
        /*##########################################*/

        protected virtual void CompileInsert(Query current)
        {
            string fields = "";
            string values = "";

            if (current.insertFields.Count > 0)
            {
                fields = $" ( {string.Join(",", current.insertFields)} ) ";
            }

            if (current.insertValues.Count > 0)
            {
                values = $" VALUES ({string.Join("),(", current.insertValues)}) ";
            }

            compiledSql.Append($" INSERT INTO  {current.insertTableName}{fields}{values}");

            current.insertTableName = "";
            current.insertFields.Clear();
            current.insertValues.Clear();
        }
    }
}