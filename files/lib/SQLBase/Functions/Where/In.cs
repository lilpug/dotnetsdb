using System;
using System.Text;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*            Where in function             */
        /*##########################################*/

        /// <summary>
        /// This function deals with creating the where in SQL
        /// </summary>
        /// <param name="theQuery"></param>
        /// <param name="definition"></param>
        /// <param name="tableName"></param>
        /// <param name="field"></param>
        /// <param name="values"></param>
        /// <param name="theOperator"></param>
        /// <param name="type"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        protected void WhereInCompile(Query theQuery, string definition, string tableName, string field, object values, string theOperator, string type = null, string start = null, string end = null)
        {
            //Array check preperation
            object[] check = new object[2]; //This is used for a single between (values)

            //Validation
            WhereInValidation(theQuery, tableName, field, type, values);
            check = AddData(values);
            if (check == null && check.Length <= 0)
            {
                throw new Exception("Where In Error: The where values supplied are empty.");
            }

            //Builds the values for the single definitions
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < check.Length; i++)
            {
                sb.Append($"{definition}{i}");

                //Checks to make sure we only add a comma on everything but the last loop
                if (i != (check.Length - 1))
                {
                    sb.Append(",");
                }
            }
            
            //Adds the operator *default and*
            if (theQuery.WhereStatements.Count != 0)
            {
                theQuery.WhereStatementsTypes.Add((!string.IsNullOrWhiteSpace(type)) ? type : "AND");
            }

            //Extra optional variables
            string whereOperator = (string.IsNullOrWhiteSpace(theOperator) ? "" : theOperator);
            string startWrapper = (!string.IsNullOrWhiteSpace(start) ? start : "");
            string endWrapper = (!string.IsNullOrWhiteSpace(end) ? end : "");
            
            //Builds the sql string
            theQuery.WhereStatements.Add($"{startWrapper} {tableName}.{field} {whereOperator} IN ({sb.ToString()}) {endWrapper}");
        }

        /*##########################################*/
        /*       Where In Validation functions      */
        /*##########################################*/
        
        /// <summary>
        /// This function validates the where in variables
        /// </summary>
        /// <param name="theQuery"></param>
        /// <param name="tableName"></param>
        /// <param name="field"></param>
        /// <param name="type"></param>
        /// <param name="values"></param>
        protected void WhereInValidation(Query theQuery, string tableName, string field, string type, object values)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("Where In Error: The table name supplied is empty.");
            }
            else if (string.IsNullOrWhiteSpace(field))
            {
                throw new Exception("Where In Error: The Field supplied is empty.");
            }
            else if (!string.IsNullOrWhiteSpace(type) && theQuery.WhereStatements.Count == 0)
            {
                throw new Exception("Where In Error: The where type is supplied but this is the first where clause so there will be no type used.");
            }
            else if (values == null || values.GetType() != typeof(object[]))
            {
                throw new Exception("Where In Error: The where values supplied are not within a object[].");
            }
        }

        /*##########################################*/
        /*             Main Front function          */
        /*##########################################*/

        /// <summary>
        /// This function defines a where in statement
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="field"></param>
        /// <param name="values">object[] only</param>
        /// <param name="theOperator">Optional, default is blank, could use NOT</param>
        /// <param name="type">Optional, default is 'AND'</param>
        /// <param name="startWrapper">Optional start wrapper</param>
        /// <param name="endWrapper">Optional end wrapper</param>
        public void add_where_in(string tableName, string field, object values, string theOperator = null, string type = null, string startWrapper = null, string endWrapper = null)
        {
            Query theQuery = GetQuery();
            string definition = $"{whereDefinition}_{theQueries.Count}_{theQuery.WhereStatements.Count}_";

            //Builds the query
            WhereInCompile(theQuery, definition, tableName, field, values, theOperator, type, startWrapper, endWrapper);

            //Adds the real value to a list for binding and sanitization later
            theQuery.WhereRealValues.Add(AddData(values));

            //Adds the command
            theQuery.OrderList.Add("where");
        }
    }
}