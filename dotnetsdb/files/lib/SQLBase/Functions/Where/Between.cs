using System;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*         Where Between function           */
        /*##########################################*/

        //This function deals with processing through the where between clause
        protected void where_between_compile(query theQuery, string definition, string table_name, string where_field, object where_values, string where_operator, string where_type = null, string start_wrapper = null, string end_wrapper = null)
        {
            //Array check preperation
            object[] check = new object[2]; //This is used for a single between (values)

            //Validation
            where_between_validation(theQuery, table_name, where_field, where_type, where_values);
            check = (object[])where_values;
            if (check == null || check.Length != 2)
            {
                throw new Exception("Where Between Error: There has to be 2 values within the object[] for a between clause.");
            }

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
            temp_build = string.Format("{0} {1}.{2} {3} BETWEEN {4} AND {5} {6}", startWrapper, table_name, where_field, whereOperator, definition + "0 ", definition + "1 ", endWrapper);
            theQuery.where_statements.Add(temp_build);
        }

        /*##########################################*/
        /*   Where Between Validation functions     */
        /*##########################################*/

        protected void where_between_validation(query theQuery, string tableName, string field, string type, object values)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("Where Between Error: The table name supplied is empty.");
            }
            else if (string.IsNullOrWhiteSpace(field))
            {
                throw new Exception("Where Between Error: The Field supplied is empty.");
            }
            else if (!string.IsNullOrWhiteSpace(type) && theQuery.where_statements.Count == 0)
            {
                throw new Exception("Where Between Error: The where type is supplied but this is the first where clause so there will be no type used.");
            }
            else if (values == null || values.GetType() != typeof(object[]))
            {
                throw new Exception("Where Between Error: The where values supplied are not within a object[].");
            }
        }

        /*##########################################*/
        /*             Main Front function          */
        /*##########################################*/

        /// <summary>
        /// This function defines a where between statement
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="whereField"></param>
        /// <param name="whereValues">object[] only, should only have 2 elements!</param>
        /// <param name="whereOperator">Optional, default is blank, could use NOT</param>
        /// <param name="whereType">Optional, default is 'AND'</param>
        /// <param name="startWrapper">Optional start wrapper</param>
        /// <param name="endWrapper">Optional end wrapper</param>
        public void add_where_between(string tableName, string whereField, object whereValues, string whereOperator = null, string whereType = null, string startWrapper = null, string endWrapper = null)
        {
            query theQuery = get_query();
            string definition = where_definition + "_" + (theQueries.Count).ToString() + "_" + (theQuery.where_statements.Count).ToString() + "_";

            //Builds the query
            where_between_compile(theQuery, definition, tableName, whereField, whereValues, whereOperator, whereType, startWrapper, endWrapper);

            //Adds the null value to a list for binding and sanitization later
            theQuery.where_real_values.Add(add_data(whereValues));

            //Adds the command
            theQuery.orderList.Add("where");
        }
    }
}