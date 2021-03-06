﻿using System;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*         Where Between function           */
        /*##########################################*/

        /// <summary>
        /// This function deals with creating the where between SQL
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
            
            //Adds the operator *default and*
            if (theQuery.WhereStatements.Count != 0)
            {
                theQuery.WhereStatementsTypes.Add((!string.IsNullOrWhiteSpace(type)) ? type : "AND");
            }

            //Builds the sql string
            theQuery.WhereStatements.Add($"{startWrapper} {tableName}.{field} {whereOperator} BETWEEN {definition}0 AND {definition}1 {endWrapper}");
        }

        /*##########################################*/
        /*   Where Between Validation functions     */
        /*##########################################*/

        /// <summary>
        /// This function validates the where between variables
        /// </summary>
        /// <param name="theQuery"></param>
        /// <param name="tableName"></param>
        /// <param name="field"></param>
        /// <param name="type"></param>
        /// <param name="values"></param>
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
            else if (!string.IsNullOrWhiteSpace(type) && theQuery.WhereStatements.Count == 0)
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
            string definition = $"{whereDefinition}_{theQueries.Count}_{theQuery.WhereStatements.Count}_";

            //Builds the query
            WhereBetweenCompile(theQuery, definition, tableName, field, values, theOperator, type, startWrapper, endWrapper);

            //Adds the null value to a list for binding and sanitization later
            theQuery.WhereRealValues.Add(AddData(values));

            //Adds the command
            theQuery.OrderList.Add("where");
        }
    }
}