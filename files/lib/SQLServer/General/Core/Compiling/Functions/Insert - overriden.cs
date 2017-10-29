using System;

namespace DotNetSDB
{
    public partial class SqlServerCore
    {
        /*##########################################*/
        /*          Compiling Insert functions      */
        /*##########################################*/

        /// <summary>
        /// This function compiles the insert query and adds it to the query being built
        /// </summary>
        /// <param name="current"></param>
        protected override void CompileInsert(Query current)
        {
            //Converts the query object to QueryExtension
            QueryExtension theQuery = (QueryExtension)current;

            string fields = "";
            string values = "";

            //Adds the data to be outputted if required
            string returnInsert = "";
            if (theQuery.InsertReturn)
            {
                returnInsert = " OUTPUT INSERTED.* ";
                theQuery.InsertReturn = false;
            }

            if (theQuery.InsertFields.Count > 0)
            {
                fields = $" ( {string.Join(",", theQuery.InsertFields)} ) ";
            }

            if (theQuery.InsertValues.Count > 0)
            {
                values = $" VALUES ({string.Join("),(", theQuery.InsertValues)}) ";
            }

            compiledSql.Append($" INSERT INTO  {theQuery.InsertTableName}{fields}{returnInsert}{values}");
        }
    }
}