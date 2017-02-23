using System;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*         Where Between function           */
        /*##########################################*/

        //This function deals with processing through the where between clause
        protected void WhereBetweenCompile(Query theQuery, string definition, string tableName, string field, object values, string theOperator, string type = null, string start = null, string end = null)
        {
            //Array check preperation
            object[] check = new object[2]; //This is used for a single between (values)

            //Validation
            WhereBetweenValidation(theQuery, tableName, field, type, values);
            check = (object[])values;
            if (check == null || check.Length != 2)
            {
                throw new Exception("Where Between Error: There has to be 2 values within the object[] for a between clause.");
            }

            //Extra optional variables
            string whereOperator = (string.IsNullOrWhiteSpace(theOperator) ? "" : theOperator);
            string startWrapper = (!string.IsNullOrWhiteSpace(start) ? start : "");
            string endWrapper = (!string.IsNullOrWhiteSpace(end) ? end : "");

            string temp_build = "";

            //Adds the operator *default and*
            if (theQuery.whereStatements.Count != 0)
            {
                theQuery.whereStatementsTypes.Add((!string.IsNullOrWhiteSpace(type)) ? type : "AND");
            }

            //Builds the sql string
            temp_build = string.Format("{0} {1}.{2} {3} BETWEEN {4}0 AND {4}1 {5}", startWrapper, tableName, field, whereOperator, definition, endWrapper);
            theQuery.whereStatements.Add(temp_build);
        }

        /*##########################################*/
        /*   Where Between Validation functions     */
        /*##########################################*/

        protected void WhereBetweenValidation(Query theQuery, string tableName, string field, string type, object values)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("Where Between Error: The table name supplied is empty.");
            }
            else if (string.IsNullOrWhiteSpace(field))
            {
                throw new Exception("Where Between Error: The Field supplied is empty.");
            }
            else if (!string.IsNullOrWhiteSpace(type) && theQuery.whereStatements.Count == 0)
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
        /// <param name="field"></param>
        /// <param name="values">object[] only, should only have 2 elements!</param>
        /// <param name="theOperator">Optional, default is blank, could use NOT</param>
        /// <param name="type">Optional, default is 'AND'</param>
        /// <param name="startWrapper">Optional start wrapper</param>
        /// <param name="endWrapper">Optional end wrapper</param>
        public void add_where_between(string tableName, string field, object values, string theOperator = null, string type = null, string startWrapper = null, string endWrapper = null)
        {
            Query theQuery = GetQuery();
            string definition = string.Format("{0}_{1}_{2}_", whereDefinition, (theQueries.Count).ToString(), (theQuery.whereStatements.Count).ToString());

            //Builds the query
            WhereBetweenCompile(theQuery, definition, tableName, field, values, theOperator, type, startWrapper, endWrapper);

            //Adds the null value to a list for binding and sanitization later
            theQuery.whereRealValues.Add(AddData(values));

            //Adds the command
            theQuery.orderList.Add("where");
        }
    }
}