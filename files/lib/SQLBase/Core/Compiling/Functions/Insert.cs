using System;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*          Compiling Insert functions      */
        /*##########################################*/

        protected virtual void CompileInsert(query current)
        {
            string fields = "";
            string values = "";

            if (current.insert_fields.Count > 0)
            {
                fields = string.Format(" ( {0} ) ", String.Join(",", current.insert_fields));
            }

            if (current.insert_values.Count > 0)
            {
                values = string.Format(" VALUES ({0}) ", String.Join("),(", current.insert_values));
            }

            compiled_build += string.Format(" INSERT INTO  {0}{1}{2}", current.insert_table_name, fields, values);

            current.insert_table_name = "";
            current.insert_fields.Clear();
            current.insert_values.Clear();
        }
    }
}