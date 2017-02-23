using System;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*         Normal Where function            */
        /*##########################################*/

        protected void WhereNormalCompile(Query theQuery, string definition, string tableName, string field, object value, string theOperator = null, string type = null, string start = null, string end = null)
        {
            //validation
            WhereNormalValidation(theQuery, tableName, field, type);

            //Extra optional variables
            string whereOperator = (string.IsNullOrWhiteSpace(theOperator) ? "=" : theOperator);
            string startWrapper = (!string.IsNullOrWhiteSpace(start) ? start : "");
            string endWrapper = (!string.IsNullOrWhiteSpace(end) ? end : "");

            string temp_build = "";

            //Adds the operator *default and*
            if (theQuery.whereStatements.Count != 0)
            {
                theQuery.whereStatementsTypes.Add((!string.IsNullOrWhiteSpace(type)) ? type : "AND");
            }

            //Builds the sql string
            temp_build = string.Format("{0} {1}.{2} {3} {4} {5}", startWrapper, tableName, field, whereOperator, definition + "0 ", endWrapper);
            theQuery.whereStatements.Add(temp_build);
        }

        /*##########################################*/
        /*      Where Normal Validation functions   */
        /*##########################################*/

        protected void WhereNormalValidation(Query theQuery, string tableName, string field, string type)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("Where Normal Error: The table name supplied is empty.");
            }
            else if (string.IsNullOrWhiteSpace(field))
            {
                throw new Exception("Where Normal Error: The Field supplied is empty.");
            }
            else if (!string.IsNullOrWhiteSpace(type) && theQuery.whereStatements.Count == 0)
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
        /// <param name="field"></param>
        /// <param name="value">Single value</param>
        /// <param name="theOperator">Optional, default is '='</param>
        /// <param name="type">Optional, default is 'AND'</param>
        /// <param name="startWrapper">Optional start wrapper</param>
        /// <param name="endWrapper">Optional end wrapper</param>
        public void add_where_normal(string tableName, string field, object value, string theOperator = null, string type = null, string startWrapper = null, string endWrapper = null)
        {
            Query theQuery = GetQuery();
            
            string definition = string.Format("{0}_{1}_{2}_", whereDefinition, (theQueries.Count).ToString(), (theQuery.whereStatements.Count).ToString());

            //Builds the query
            WhereNormalCompile(theQuery, definition, tableName, field, value, theOperator, type, startWrapper, endWrapper);

            //Adds the real values to a list for binding and sanitization later
            theQuery.whereRealValues.Add(AddData(value));

            //Adds the command
            theQuery.orderList.Add("where");
        }
    }
}