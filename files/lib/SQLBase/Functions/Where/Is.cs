using System;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*              Where is function           */
        /*##########################################*/

        //This function deals with processing through the where like clause
        protected void where_is_compiling(query theQuery, string table_name, string where_field, string where_operator, string where_type = null, string start_wrapper = null, string end_wrapper = null)
        {
            //validation
            where_is_validation(theQuery, table_name, where_field, where_type);

            //Extra optional variables
            string whereOperator = (string.IsNullOrWhiteSpace(where_operator) ? "" : where_operator);
            string startWrapper = (!string.IsNullOrWhiteSpace(start_wrapper) ? start_wrapper : "");
            string endWrapper = (!string.IsNullOrWhiteSpace(end_wrapper) ? end_wrapper : "");

            string temp_build = "";

            //Adds the operator *default and*
            if (theQuery.where_statements.Count != 0)
            {
                theQuery.where_statement_types.Add((!string.IsNullOrWhiteSpace(where_type)) ? where_type : "AND");
            }

            //Builds the sql string
            temp_build = string.Format("{0} {1}.{2} IS {3} NULL {4}", startWrapper, table_name, where_field, whereOperator, endWrapper);
            theQuery.where_statements.Add(temp_build);
        }

        /*##########################################*/
        /*      Where Is Validation functions       */
        /*##########################################*/

        protected void where_is_validation(query theQuery, string tableName, string field, string type)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("Where Is Error: The table name supplied is empty.");
            }
            else if (string.IsNullOrWhiteSpace(field))
            {
                throw new Exception("Where Is Error: The Field supplied is empty.");
            }
            else if (!string.IsNullOrWhiteSpace(type) && theQuery.where_statements.Count == 0)
            {
                throw new Exception("Where Is Error: The where type is supplied but this is the first where clause so there will be no type used.");
            }
        }

        /*##########################################*/
        /*             Main Front function          */
        /*##########################################*/

        /// <summary>
        /// This function defines a where is statement
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="whereField"></param>
        /// <param name="whereOperator">Optional, default is blank, could use NOT</param>
        /// <param name="whereType">Optional, default is 'AND'</param>
        /// <param name="startWrapper">Optional start wrapper</param>
        /// <param name="endWrapper">Optional end wrapper</param>
        public void add_where_is(string tableName, string whereField, string whereOperator = null, string whereType = null, string startWrapper = null, string endWrapper = null)
        {
            query theQuery = get_query();

            //Builds the query
            where_is_compiling(theQuery, tableName, whereField, whereOperator, whereType, startWrapper, endWrapper);

            //Adds the null value to a list for binding and sanitization later
            theQuery.where_real_values.Add(add_data(null));

            //Adds the command
            theQuery.orderList.Add("where");
        }
    }
}