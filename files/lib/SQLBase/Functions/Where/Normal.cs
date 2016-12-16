using System;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*         Normal Where function            */
        /*##########################################*/

        protected void where_normal_compiling(query theQuery, string definition, string table_name, string where_field, object where_value, string where_operator = null, string where_type = null, string start_wrapper = null, string end_wrapper = null)
        {
            //validation
            where_normal_validation(theQuery, table_name, where_field, where_type);

            //Extra optional variables
            string whereOperator = (string.IsNullOrWhiteSpace(where_operator) ? "=" : where_operator);
            string startWrapper = (!string.IsNullOrWhiteSpace(start_wrapper) ? start_wrapper : "");
            string endWrapper = (!string.IsNullOrWhiteSpace(end_wrapper) ? end_wrapper : "");

            string temp_build = "";

            //Adds the operator *default and*
            if (theQuery.where_statements.Count != 0)
            {
                theQuery.where_statement_types.Add((!string.IsNullOrWhiteSpace(where_type)) ? where_type : "AND");
            }

            //Builds the sql string
            temp_build = string.Format("{0} {1}.{2} {3} {4} {5}", startWrapper, table_name, where_field, whereOperator, definition + "0 ", endWrapper);
            theQuery.where_statements.Add(temp_build);
        }

        /*##########################################*/
        /*      Where Normal Validation functions   */
        /*##########################################*/

        protected void where_normal_validation(query theQuery, string tableName, string field, string type)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("Where Normal Error: The table name supplied is empty.");
            }
            else if (string.IsNullOrWhiteSpace(field))
            {
                throw new Exception("Where Normal Error: The Field supplied is empty.");
            }
            else if (!string.IsNullOrWhiteSpace(type) && theQuery.where_statements.Count == 0)
            {
                throw new Exception("Where Normal Error: The where type is supplied but this is the first where clause so there will be no type used.");
            }
        }

        /*##########################################*/
        /*             Main Front function          */
        /*##########################################*/

        /// <summary>
        /// This function defines a normal where statement
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="whereField"></param>
        /// <param name="whereValue">Single value</param>
        /// <param name="whereOperator">Optional, default is '='</param>
        /// <param name="whereType">Optional, default is 'AND'</param>
        /// <param name="startWrapper">Optional start wrapper</param>
        /// <param name="endWrapper">Optional end wrapper</param>
        public void add_where_normal(string tableName, string whereField, object whereValue, string whereOperator = null, string whereType = null, string startWrapper = null, string endWrapper = null)
        {
            query theQuery = get_query();
            string definition = where_definition + "_" + (theQueries.Count).ToString() + "_" + (theQuery.where_statements.Count).ToString() + "_";

            //Builds the query
            where_normal_compiling(theQuery, definition, tableName, whereField, whereValue, whereOperator, whereType, startWrapper, endWrapper);

            //Adds the real values to a list for binding and sanitization later
            theQuery.where_real_values.Add(add_data(whereValue));

            //Adds the command
            theQuery.orderList.Add("where");
        }
    }
}