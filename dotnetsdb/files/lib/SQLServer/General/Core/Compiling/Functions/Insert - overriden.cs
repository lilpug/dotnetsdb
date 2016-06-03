using System;

namespace DotNetSDB
{
    public partial class SqlServerCore
    {
        /*##########################################*/
        /*          Compiling Insert functions      */
        /*##########################################*/

        protected override void CompileInsert(query current)
        {
            string fields = "";
            string values = "";

            //Adds the data to be outputted if required
            string returnInsert = "";
            int index = theQueries.IndexOf(current);
            if (theQueries2[index].insert_return)
            {
                returnInsert = " OUTPUT INSERTED.* ";
                theQueries2[index].insert_return = false;
            }

            if (current.insert_fields.Count > 0)
            {
                fields = string.Format(" ( {0} ) ", String.Join(",", current.insert_fields));
            }

            if (current.insert_values.Count > 0)
            {
                values = string.Format(" VALUES ({0}) ", String.Join("),(", current.insert_values));
            }

            compiled_build += string.Format(" INSERT INTO  {0}{1}{2}{3}", current.insert_table_name, fields, returnInsert, values);

            current.insert_table_name = "";
            current.insert_fields.Clear();
            current.insert_values.Clear();
        }
    }
}