using System;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*          Compiling Insert functions      */
        /*##########################################*/

        /// <summary>
        /// This function compiles the insert query and adds it to the query being built
        /// </summary>
        /// <param name="current"></param>
        protected virtual void CompileInsert(Query current)
        {
            string fields = "";
            string values = "";

            if (current.InsertFields.Count > 0)
            {
                fields = $" ( {string.Join(",", current.InsertFields)} ) ";
            }

            if (current.InsertValues.Count > 0)
            {
                values = $" VALUES ({string.Join("),(", current.InsertValues)}) ";
            }

            compiledSql.Append($" INSERT INTO  {current.InsertTableName}{fields}{values}");
        }
    }
}