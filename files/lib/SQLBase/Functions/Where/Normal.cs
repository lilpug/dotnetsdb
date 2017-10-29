using System;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*         Normal Where function            */
        /*##########################################*/

        /// <summary>
        /// This function deals with creating the normal where SQL
        /// </summary>
        /// <param name="theQuery"></param>
        /// <param name="definition"></param>
        /// <param name="tableName"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="theOperator"></param>
        /// <param name="type"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        protected void WhereNormalCompile(Query theQuery, string definition, string tableName, string field, object value, string theOperator = null, string type = null, string start = null, string end = null)
        {
            //validation
            WhereNormalValidation(theQuery, tableName, field, type);

            //Extra optional variables
            string whereOperator = (string.IsNullOrWhiteSpace(theOperator) ? "=" : theOperator);
            string startWrapper = (!string.IsNullOrWhiteSpace(start) ? start : "");
            string endWrapper = (!string.IsNullOrWhiteSpace(end) ? end : "");
            
            //Adds the operator *default and*
            if (theQuery.WhereStatements.Count != 0)
            {
                theQuery.WhereStatementsTypes.Add((!string.IsNullOrWhiteSpace(type)) ? type : "AND");
            }

            //Builds the sql string
            theQuery.WhereStatements.Add($"{startWrapper} {tableName}.{field} {whereOperator} {definition}0 {endWrapper}");
        }

        /*##########################################*/
        /*      Where Normal Validation functions   */
        /*##########################################*/

        /// <summary>
        /// This function validates the normal where variables
        /// </summary>
        /// <param name="theQuery"></param>
        /// <param name="tableName"></param>
        /// <param name="field"></param>
        /// <param name="type"></param>
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
            else if (!string.IsNullOrWhiteSpace(type) && theQuery.WhereStatements.Count == 0)
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
            
            string definition = $"{whereDefinition}_{theQueries.Count}_{theQuery.WhereStatements.Count}_";

            //Builds the query
            WhereNormalCompile(theQuery, definition, tableName, field, value, theOperator, type, startWrapper, endWrapper);

            //Adds the real values to a list for binding and sanitization later
            theQuery.WhereRealValues.Add(AddData(value));

            //Adds the command
            theQuery.OrderList.Add("where");
        }
    }
}