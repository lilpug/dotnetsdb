using System;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*              Where is function           */
        /*##########################################*/

        //This function deals with processing through the where like clause
        protected void WhereIsCompile(Query theQuery, string tableName, string field, string theOperator, string type = null, string start = null, string end = null)
        {
            //validation
            WhereIsValidation(theQuery, tableName, field, type);

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
            temp_build = $"{startWrapper} {tableName}.{field} IS {whereOperator} NULL {endWrapper}";
            theQuery.whereStatements.Add(temp_build);
        }

        /*##########################################*/
        /*      Where Is Validation functions       */
        /*##########################################*/

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
            else if (!string.IsNullOrWhiteSpace(type) && theQuery.whereStatements.Count == 0)
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
            theQuery.whereRealValues.Add(AddData(null));

            //Adds the command
            theQuery.orderList.Add("where");
        }
    }
}