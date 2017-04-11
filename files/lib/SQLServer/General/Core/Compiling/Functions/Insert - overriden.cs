using System;

namespace DotNetSDB
{
    public partial class SqlServerCore
    {
        /*##########################################*/
        /*          Compiling Insert functions      */
        /*##########################################*/

        protected override void CompileInsert(Query current)
        {
            string fields = "";
            string values = "";

            //Adds the data to be outputted if required
            string returnInsert = "";
            int index = theQueries.IndexOf(current);
            if (theQueries2[index].insertReturn)
            {
                returnInsert = " OUTPUT INSERTED.* ";
                theQueries2[index].insertReturn = false;
            }

            if (current.insertFields.Count > 0)
            {
                fields = $" ( {string.Join(",", current.insertFields)} ) ";
            }

            if (current.insertValues.Count > 0)
            {
                values = $" VALUES ({string.Join("),(", current.insertValues)}) ";
            }

            compiledSql += $" INSERT INTO  {current.insertTableName}{fields}{returnInsert}{values}";

            current.insertTableName = "";
            current.insertFields.Clear();
            current.insertValues.Clear();
        }
    }
}