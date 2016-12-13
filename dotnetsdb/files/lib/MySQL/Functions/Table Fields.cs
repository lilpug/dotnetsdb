using System;

namespace DotNetSDB
{
    public partial class MysqlCore
    {
        /*##########################################*/
        /*       Table Fields SQL functions         */
        /*##########################################*/

        /// <summary>
        /// This function returns all the column names of a table
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string[] table_get_fields(string tableName)
        {
            query2 theQuery = get_query2();

            theQuery.get_fields_real_table_value = add_data(tableName);

            //Gives the sql string to the compiled_build
            compiled_build = "SELECT column_name FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = " + fields_definition + "_1_0_0 ORDER BY ordinal_position;";
            
            return run_return_string_array();            
        }
    }
}