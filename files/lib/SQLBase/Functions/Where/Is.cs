using System;

namespace DotNetSDB
{
    /// <summary>
    /// This is the SQLBase class which is used as a base throughout the MySQL and SQL Server classes
    /// </summary>
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*              Where is function           */
        /*##########################################*/

        /// <summary>
        /// This function deals with creating the where is SQL
        /// </summary>
        /// <param name="theQuery"></param>
        /// <param name="tableName"></param>
        /// <param name="field"></param>
        /// <param name="theOperator"></param>
        /// <param name="type"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        protected void WhereIsCompile(Query theQuery, string tableName, string field, string theOperator, string type = null, string start = null, string end = null)
        {
            //validation
            WhereIsValidation(theQuery, tableName, field, type);

            //Extra optional variables
            string whereOperator = (string.IsNullOrWhiteSpace(theOperator) ? "" : theOperator);
            string startWrapper = (!string.IsNullOrWhiteSpace(start) ? start : "");
            string endWrapper = (!string.IsNullOrWhiteSpace(end) ? end : "");
            
            //Adds the operator *default and*
            if (theQuery.WhereStatements.Count != 0)
            {
                theQuery.WhereStatementsTypes.Add((!string.IsNullOrWhiteSpace(type)) ? type : "AND");
            }

            //Builds the sql string
            theQuery.WhereStatements.Add($"{startWrapper} {tableName}.{field} IS {whereOperator} NULL {endWrapper}");
        }

        /*##########################################*/
        /*      Where Is Validation functions       */
        /*##########################################*/

        /// <summary>
        /// This function validates the where is variables
        /// </summary>
        /// <param name="theQuery"></param>
        /// <param name="tableName"></param>
        /// <param name="field"></param>
        /// <param name="type"></param>
        protected void WhereIsValidation(Query theQuery, string tableName, string field, string type)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("Where Is Error: The table name supplied is empty.");
            }
            else if (string.IsNullOrWhiteSpace(field))
            {
                throw new Exception("Where Is Error: The Field supplied is empty.");
            }
            else if (!string.IsNullOrWhiteSpace(type) && theQuery.WhereStatements.Count == 0)
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
        /// <param name="field"></param>
        /// <param name="theOperator">Optional, default is blank, could use NOT</param>
        /// <param name="type">Optional, default is 'AND'</param>
        /// <param name="startWrapper">Optional start wrapper</param>
        /// <param name="endWrapper">Optional end wrapper</param>
        public void add_where_is(string tableName, string field, string theOperator = null, string type = null, string startWrapper = null, string endWrapper = null)
        {
            Query theQuery = GetQuery();

            //Builds the query
            WhereIsCompile(theQuery, tableName, field, theOperator, type, startWrapper, endWrapper);

            //Adds the null value to a list for binding and sanitization later
            theQuery.WhereRealValues.Add(AddData(null));

            //Adds the command
            theQuery.OrderList.Add("where");
        }
    }
}