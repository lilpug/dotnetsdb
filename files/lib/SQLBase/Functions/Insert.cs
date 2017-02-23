using System;
using System.Linq;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*      Insert Compiling functions          */
        /*##########################################*/

        //This function builds the insert sql with multiple field parameters for inserting values
        protected virtual void InsertCompile(Query theQuery, string definition, params object[] values)
        {
            //Checks if fields have been specified, if so there should be the same amount of parameters. otherwise does not matter
            if (theQuery.insertFields.Count() == 0 || theQuery.insertFields.Count() == values.Count())
            {
                string temp_build = "";
                for (int i = 0; i < values.Length; i++)//Compiles the query string
                {
                    //Determines if there should be a comma
                    string seperator = "";
                    if (i != 0)
                    {
                        seperator = ", ";
                    }

                    //Builds the string
                    temp_build += string.Format("{0}{1}", seperator, definition + i.ToString());
                }

                theQuery.insertValues.Add(temp_build);
            }
            else
            {
                throw new Exception("Insert Error: There is a different number of fields to the amount of values passed.");
            }
        }

        //This function builds the insert sql with multiple field parameters for inserting values
        protected virtual void InsertFieldCompile(Query theQuery, params string[] fields)
        {
            //Checks  to ensure the data is accurate as there should be one field for every data insert
            if (fields.Count() > 0)
            {
                if (fields.Count() > 0)
                {
                    //Merges the results
                    theQuery.insertFields = theQuery.insertFields.Concat(fields.ToList()).ToList();                    
                }
            }
            else
            {
                throw new Exception("Insert Error: There is no fields passed for the insert.");
            }
        }

        /*##########################################*/
        /*        Insert Validation functions       */
        /*##########################################*/

        protected void InsertsingleValidation(string tableName, string field)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("Insert Error: The table name supplied is empty.");
            }
            else if (string.IsNullOrWhiteSpace(field))
            {
                throw new Exception("Insert Error: The Insert Field supplied is empty.");
            }
        }

        protected void InsertMultipleValidation(string tableName, string[] fields)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("Insert Error: The table name supplied is empty.");
            }
            else if (fields != null && fields.Length <= 0)
            {
                throw new Exception("Insert Error: The Insert Fields that have been supplied are empty.");
            }
        }

        protected void InsertExistValidation(Query theQuery)
        {
            if (theQuery.orderList.Contains("insert"))
            {
                throw new Exception("Insert Error: a main insert statement has already been defined, for additional fields use add_select_fields and for additional values use add_insert_values.");
            }
        }

        protected void InsertNotExistValidation(Query theQuery)
        {
            if (!theQuery.orderList.Contains("insert"))
            {
                throw new Exception("Insert Error: you cannot add insert fields or values without defining a main insert statement first.");
            }
        }

        /*##########################################*/
        /*             Main Front function          */
        /*##########################################*/

        /// <summary>
        /// This function adds the insert statement without fields or values
        /// </summary>
        /// <param name="tableName"></param>
        public virtual void add_insert(string tableName)
        {
            Query theQuery = GetQuery();

            //Validation
            InsertExistValidation(theQuery);
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("Insert Error: The table name supplied is empty.");
            }

            //Holds the insert table
            theQuery.insertTableName = tableName;

            //Adds the command
            theQuery.orderList.Add("insert");
        }

        /// <summary>
        /// This functions adds the insert statement with a field only
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="field"></param>
        public virtual void add_insert(string tableName, string field)
        {
            Query theQuery = GetQuery();

            //Validation
            InsertExistValidation(theQuery);
            InsertsingleValidation(tableName, field);

            //Holds the insert table
            theQuery.insertTableName = tableName;

            //Builds the feidls query
            InsertFieldCompile(theQuery, field);

            //Adds the command
            theQuery.orderList.Add("insert");
        }

        /// <summary>
        /// This functions adds the insert statement with fields only
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
        public virtual void add_insert(string tableName, string[] fields)
        {
            Query theQuery = GetQuery();

            //Validation
            InsertExistValidation(theQuery);
            InsertMultipleValidation(tableName, fields);

            //Holds the insert table
            theQuery.insertTableName = tableName;

            //Builds the feidls query
            InsertFieldCompile(theQuery, fields);

            //Adds the command
            theQuery.orderList.Add("insert");
        }

        /// <summary>
        /// This function adds the insert statement with only values
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="values">single value or object[] only</param>
        public virtual void add_insert(string tableName, object values)
        {
            Query theQuery = GetQuery();
            
            string definition = string.Format("{0}_{1}_{2}_", insertDefinition, (theQueries.Count).ToString(), (theQuery.insertValues.Count).ToString());

            //Validation
            InsertExistValidation(theQuery);
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("Insert Error: The table name supplied is empty.");
            }

            //Gets the data object *even if the value is just null it should be length 1 i.e. new object[] {null}
            object[] holding = AddData(values);

            //Holds the insert table
            theQuery.insertTableName = tableName;

            //Builds the query
            InsertCompile(theQuery, definition, holding);

            //Adds the real values to a list for binding and sanitization later
            theQuery.insertRealValues.Add(holding);

            //Adds the command
            theQuery.orderList.Add("insert");
        }

        /// <summary>
        /// This function adds the insert statement with a field and value
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="field"></param>
        /// <param name="value">single value only</param>
        public virtual void add_insert(string tableName, string field, object value)
        {
            Query theQuery = GetQuery();
            string definition = string.Format("{0}_{1}_{2}_", insertDefinition, (theQueries.Count).ToString(), (theQuery.insertValues.Count).ToString());

            //Validation
            InsertExistValidation(theQuery);
            InsertsingleValidation(tableName, field);

            //Gets the data object *even if the value is just null it should be length 1 i.e. new object[] {null}
            object[] holding = AddData(value);

            //Holds the insert table
            theQuery.insertTableName = tableName;

            //Builds the query
            InsertFieldCompile(theQuery, field);
            InsertCompile(theQuery, definition, holding);

            //Adds the real values to a list for binding and sanitization later
            theQuery.insertRealValues.Add(holding);

            //Adds the command
            theQuery.orderList.Add("insert");
        }

        /// <summary>
        /// This function adds the insert statement with the fields and values
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
        /// <param name="values">object[] only</param>
        public virtual void add_insert(string tableName, string[] fields, object values)
        {
            Query theQuery = GetQuery();
            string definition = string.Format("{0}_{1}_{2}_", insertDefinition, (theQueries.Count).ToString(), (theQuery.insertValues.Count).ToString());

            //Validation
            InsertExistValidation(theQuery);
            InsertMultipleValidation(tableName, fields);

            //Gets the data object *even if the value is just null it should be length 1 i.e. new object[] {null}
            object[] holding = AddData(values);

            //Holds the insert table
            theQuery.insertTableName = tableName;

            //Builds the query
            InsertFieldCompile(theQuery, fields);
            InsertCompile(theQuery, definition, holding);

            //Adds the real values to a list for binding and sanitization later
            theQuery.insertRealValues.Add(holding);

            //Adds the command
            theQuery.orderList.Add("insert");
        }

        /// <summary>
        /// This function adds additional fields to an insert statement
        /// </summary>
        /// <param name="field"></param>
        public virtual void add_insert_fields(string field)
        {
            Query theQuery = GetQuery();

            //Validation
            InsertNotExistValidation(theQuery);
            InsertsingleValidation("exclude", field);

            //Builds the query
            InsertFieldCompile(theQuery, field);
        }

        /// <summary>
        /// This function adds additional fields to an insert statement
        /// </summary>
        /// <param name="fields"></param>
        public virtual void add_insert_fields(string[] fields)
        {
            Query theQuery = GetQuery();

            //Validation
            InsertNotExistValidation(theQuery);
            InsertMultipleValidation("exclude", fields);

            //Builds the query
            InsertFieldCompile(theQuery, fields);
        }

        /// <summary>
        /// <para>This function adds additional values to an insert statement</para>
        /// </summary>
        /// <param name="values">single values or object[] only</param>
        public virtual void add_insert_values(object values)
        {
            Query theQuery = GetQuery();
            string definition = string.Format("{0}_{1}_{2}_", insertDefinition, (theQueries.Count).ToString(), (theQuery.insertValues.Count).ToString());

            //Validation
            InsertNotExistValidation(theQuery);

            //Gets the data object *even if the value is just null it should be length 1 i.e. new object[] {null}
            object[] holding = AddData(values);

            //Builds the query
            InsertCompile(theQuery, definition, holding);

            //Adds the real values to a list for binding and sanitization later
            theQuery.insertRealValues.Add(holding);
        }
    }
}