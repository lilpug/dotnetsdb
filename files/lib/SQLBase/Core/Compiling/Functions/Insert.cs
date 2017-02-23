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
                fields = string.Format(" ( {0} ) ", string.Join(",", current.insertFields));
            }

            if (current.insertValues.Count > 0)
            {
                values = string.Format(" VALUES ({0}) ", string.Join("),(", current.insertValues));
            }

            compiledSql += string.Format(" INSERT INTO  {0}{1}{2}", current.insertTableName, fields, values);

            current.insertTableName = "";
            current.insertFields.Clear();
            current.insertValues.Clear();
        }
    }
}