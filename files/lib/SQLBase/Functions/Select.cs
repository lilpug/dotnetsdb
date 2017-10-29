using System;
using System.Text;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*        Select Creation functions         */
        /*##########################################*/

        /// <summary>
        /// This function deals with creating the select SQL
        /// </summary>
        /// <param name="theQuery"></param>
        /// <param name="tableName"></param>
        /// <param name="selectField"></param>
        /// <param name="startField"></param>
        /// <param name="endField"></param>
        protected void SelectCompile(Query theQuery, string tableName, string selectField, string startField = null, string endField = null)
        {
            SelectValidation(tableName);

            //Checks if there is any select fields
            if (selectField != null)
            {
                //Checks if the sizes are not the same and if not returns a false
                if (
                    (startField != null && endField != null) &&
                    ((!string.IsNullOrWhiteSpace(startField) && string.IsNullOrWhiteSpace(endField)) || 
                     (string.IsNullOrWhiteSpace(startField) && !string.IsNullOrWhiteSpace(endField)))
                    )
                {
                    throw new Exception("Select Error: When using a start or end parameter you must supply both");
                }

                //Do not change the main table name if its already set
                if (string.IsNullOrWhiteSpace(theQuery.SelectTable) && !string.IsNullOrWhiteSpace(tableName))
                {
                    //Sets the table name
                    theQuery.SelectTable = tableName;
                }

                StringBuilder sb = new StringBuilder();

                //Builds the string
                if (startField != null && endField != null)
                {
                    sb.Append($"{startField} {tableName}.{selectField} {endField}");
                }
                else
                {
                    sb.Append($"{tableName}.{selectField}");
                }

                theQuery.SelectFields.Add(sb.ToString());
            }
            else
            {
                throw new Exception("Select Error: The fields parameter is empty.");
            }
        }

        /// <summary>
        /// This function deals with creating the select SQL
        /// </summary>
        /// <param name="theQuery"></param>
        /// <param name="tableName"></param>
        /// <param name="selectFields"></param>
        /// <param name="startFields"></param>
        /// <param name="endFields"></param>
        protected void SelectCompile(Query theQuery, string tableName, string[] selectFields, string[] startFields = null, string[] endFields = null)
        {
            SelectValidation(tableName);

            //Checks if there is any select fields
            if (selectFields != null)
            {
                //Checks if the sizes are not the same and if not returns a false
                if (
                    (startFields != null && endFields != null) &&
                    (selectFields.Length != startFields.Length || selectFields.Length != endFields.Length)
                    )
                {
                    throw new Exception("Select Error: When using multiple start or end parameters you must supply an equal amount of both");
                }

                //Do not change the main table name if its already set
                if (string.IsNullOrWhiteSpace(theQuery.SelectTable) && !string.IsNullOrWhiteSpace(tableName))
                {
                    //Sets the table name
                    theQuery.SelectTable = tableName;
                }
                StringBuilder sb = new StringBuilder();
                
                for (int i = 0; i < selectFields.Length; i++)
                {
                    //Determines if there should be a comma
                    if (i != 0)
                    {
                        sb.Append(", ");
                    }

                    //Builds the string
                    if (startFields != null && endFields != null)
                    {
                        sb.Append($"{startFields[i]} {tableName}.{selectFields[i]} {endFields[i]}");
                    }
                    else
                    {
                        sb.Append($"{tableName}.{selectFields[i]}");
                    }
                }
                theQuery.SelectFields.Add(sb.ToString());
            }
            else
            {
                throw new Exception("Select Error: The fields parameter is empty.");
            }
        }

        /*##########################################*/
        /*       Select Validation functions        */
        /*##########################################*/

        /// <summary>
        /// This function validates the select variables
        /// </summary>
        /// <param name="tableName"></param>
        protected void SelectValidation(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("Select Error: The table name supplied is empty.");
            }
        }

        /// <summary>
        /// This function validates that the select base statement has been added before trying to add additional fields
        /// </summary>
        /// <param name="theQuery"></param>
        protected void SelectNotExistValidation(Query theQuery)
        {
            if (!theQuery.OrderList.Contains("select"))
            {
                throw new Exception("Select Error: you cannot add select fields without defining a main select statement first.");
            }
        }

        /// <summary>
        /// This function validates that the select query has not already been run as a new one is about to be added
        /// </summary>
        /// <param name="theQuery"></param>
        protected void SelectExistValidation(Query theQuery)
        {
            if (theQuery.OrderList.Contains("select"))
            {
                throw new Exception("Select Error: a main select statement has already been defined, for additional fields use add_select_fields.");
            }
        }

        /*##########################################*/
        /*           Main Front function            */
        /*##########################################*/

        /// <summary>
        /// This function determines whether the select statement should be be distinct
        /// </summary>
        /// <param name="distinct"></param>
        public void is_distinct(bool distinct)
        {
            Query theQuery = GetQuery();
            theQuery.IsDistinct = distinct;
        }

        /// <summary>
        /// This functions adds an additional select field to a select statement
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="selectField"></param>
        /// <param name="startField"></param>
        /// <param name="endField"></param>
        public void add_select_fields(string tableName, string selectField, string startField = null, string endField = null)
        {
            //Obtains the current query object
            Query theQuery = GetQuery();

            SelectNotExistValidation(theQuery);

            //Builds the select_fields sql
            SelectCompile(theQuery, tableName, selectField, startField, endField);
        }

        /// <summary>
        /// This functions adds additional select fields to a select statement
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="selectFields"></param>
        /// <param name="startFields"></param>
        /// <param name="endFields"></param>
        public void add_select_fields(string tableName, string[] selectFields, string[] startFields = null, string[] endFields = null)
        {
            //Obtains the current query object
            Query theQuery = GetQuery();

            SelectNotExistValidation(theQuery);

            //Builds the select_fields sql
            SelectCompile(theQuery, tableName, selectFields, startFields, endFields);
        }

        /// <summary>
        /// This functions adds a select statement
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="selectField"></param>
        /// <param name="startField"></param>
        /// <param name="endField"></param>
        public void add_select(string tableName, string selectField, string startField = null, string endField = null)
        {
            Query theQuery = GetQuery();

            SelectExistValidation(theQuery);

            //Builds the select_fields sql
            SelectCompile(theQuery, tableName, selectField, startField, endField);

            //Adds the command
            theQuery.OrderList.Add("select");
        }

        /// <summary>
        /// This functions adds a select statement
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="selectFields"></param>
        /// <param name="startFields"></param>
        /// <param name="endFields"></param>
        public void add_select(string tableName, string[] selectFields, string[] startFields = null, string[] endFields = null)
        {
            Query theQuery = GetQuery();

            SelectExistValidation(theQuery);

            //Builds the select_fields sql
            SelectCompile(theQuery, tableName, selectFields, startFields, endFields);

            //Adds the command
            theQuery.OrderList.Add("select");
        }
    }
}