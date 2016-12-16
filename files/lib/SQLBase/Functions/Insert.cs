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
        protected virtual void insert_build_compiling(query theQuery, string definition, params object[] insert_values)
        {
            //Checks if fields have been specified, if so there should be the same amount of parameters. otherwise does not matter
            if (theQuery.insert_fields.Count() == 0 || theQuery.insert_fields.Count() == insert_values.Count())
            {
                string temp_build = "";
                for (int i = 0; i < insert_values.Length; i++)//Compiles the query string
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

                theQuery.insert_values.Add(temp_build);
            }
            else
            {
                throw new Exception("Insert Error: There is a different number of fields to the amount of values passed.");
            }
        }

        //This function builds the insert sql with multiple field parameters for inserting values
        protected virtual void insert_build_fields_compiling(query theQuery, params string[] insert_fields)
        {
            //Checks  to ensure the data is accurate as there should be one field for every data insert
            if (insert_fields.Count() > 0)
            {
                if (insert_fields.Count() > 0)
                {
                    //Merges the results
                    theQuery.insert_fields = theQuery.insert_fields.Concat(insert_fields.ToList()).ToList();

                    //theQuery.insert_fields = insert_fields.ToList();
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

        protected void insert_single_validation(string tableName, string field)
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

        protected void insert_multiple_validation(string tableName, string[] insertFields)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("Insert Error: The table name supplied is empty.");
            }
            else if (insertFields != null && insertFields.Length <= 0)
            {
                throw new Exception("Insert Error: The Insert Fields that have been supplied are empty.");
            }
        }

        protected void insert_exist_validation(query theQuery)
        {
            if (theQuery.orderList.Contains("insert"))
            {
                throw new Exception("Insert Error: a main insert statement has already been defined, for additional fields use add_select_fields and for additional values use add_insert_values.");
            }
        }

        protected void insert_not_exist_validation(query theQuery)
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
            query theQuery = get_query();

            //Validation
            insert_exist_validation(theQuery);
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("Insert Error: The table name supplied is empty.");
            }

            //Holds the insert table
            theQuery.insert_table_name = tableName;

            //Adds the command
            theQuery.orderList.Add("insert");
        }

        /// <summary>
        /// This functions adds the insert statement with a field only
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="insertField"></param>
        public virtual void add_insert(string tableName, string insertField)
        {
            query theQuery = get_query();

            //Validation
            insert_exist_validation(theQuery);
            insert_single_validation(tableName, insertField);

            //Holds the insert table
            theQuery.insert_table_name = tableName;

            //Builds the feidls query
            insert_build_fields_compiling(theQuery, insertField);

            //Adds the command
            theQuery.orderList.Add("insert");
        }

        /// <summary>
        /// This functions adds the insert statement with fields only
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="insertFields"></param>
        public virtual void add_insert(string tableName, string[] insertFields)
        {
            query theQuery = get_query();

            //Validation
            insert_exist_validation(theQuery);
            insert_multiple_validation(tableName, insertFields);

            //Holds the insert table
            theQuery.insert_table_name = tableName;

            //Builds the feidls query
            insert_build_fields_compiling(theQuery, insertFields);

            //Adds the command
            theQuery.orderList.Add("insert");
        }

        /// <summary>
        /// This function adds the insert statement with only values
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="insertValues">single value or object[] only</param>
        public virtual void add_insert(string tableName, object insertValues)
        {
            query theQuery = get_query();
            string definition = insert_definition + "_" + (theQueries.Count).ToString() + "_" + (theQuery.insert_values.Count).ToString() + "_";

            //Validation
            insert_exist_validation(theQuery);
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("Insert Error: The table name supplied is empty.");
            }

            //Gets the data object *even if the value is just null it should be length 1 i.e. new object[] {null}
            object[] holding = add_data(insertValues);

            //Holds the insert table
            theQuery.insert_table_name = tableName;

            //Builds the query
            insert_build_compiling(theQuery, definition, holding);

            //Adds the real values to a list for binding and sanitization later
            theQuery.insert_real_values.Add(holding);

            //Adds the command
            theQuery.orderList.Add("insert");
        }

        /// <summary>
        /// This function adds the insert statement with a field and value
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="insertField"></param>
        /// <param name="insertValue">single value only</param>
        public virtual void add_insert(string tableName, string insertField, object insertValue)
        {
            query theQuery = get_query();
            string definition = insert_definition + "_" + (theQueries.Count).ToString() + "_" + (theQuery.insert_values.Count).ToString() + "_";

            //Validation
            insert_exist_validation(theQuery);
            insert_single_validation(tableName, insertField);

            //Gets the data object *even if the value is just null it should be length 1 i.e. new object[] {null}
            object[] holding = add_data(insertValue);

            //Holds the insert table
            theQuery.insert_table_name = tableName;

            //Builds the query
            insert_build_fields_compiling(theQuery, insertField);
            insert_build_compiling(theQuery, definition, holding);

            //Adds the real values to a list for binding and sanitization later
            theQuery.insert_real_values.Add(holding);

            //Adds the command
            theQuery.orderList.Add("insert");
        }

        /// <summary>
        /// This function adds the insert statement with the fields and values
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="insertFields"></param>
        /// <param name="insertValues">object[] only</param>
        public virtual void add_insert(string tableName, string[] insertFields, object insertValues)
        {
            query theQuery = get_query();
            string definition = insert_definition + "_" + (theQueries.Count).ToString() + "_" + (theQuery.insert_values.Count).ToString() + "_";

            //Validation
            insert_exist_validation(theQuery);
            insert_multiple_validation(tableName, insertFields);

            //Gets the data object *even if the value is just null it should be length 1 i.e. new object[] {null}
            object[] holding = add_data(insertValues);

            //Holds the insert table
            theQuery.insert_table_name = tableName;

            //Builds the query
            insert_build_fields_compiling(theQuery, insertFields);
            insert_build_compiling(theQuery, definition, holding);

            //Adds the real values to a list for binding and sanitization later
            theQuery.insert_real_values.Add(holding);

            //Adds the command
            theQuery.orderList.Add("insert");
        }

        /// <summary>
        /// This function adds additional fields to an insert statement
        /// </summary>
        /// <param name="insertField"></param>
        public virtual void add_insert_fields(string insertField)
        {
            query theQuery = get_query();

            //Validation
            insert_not_exist_validation(theQuery);
            insert_single_validation("exclude", insertField);

            //Builds the query
            insert_build_fields_compiling(theQuery, insertField);
        }

        /// <summary>
        /// This function adds additional fields to an insert statement
        /// </summary>
        /// <param name="insertFields"></param>
        public virtual void add_insert_fields(string[] insertFields)
        {
            query theQuery = get_query();

            //Validation
            insert_not_exist_validation(theQuery);
            insert_multiple_validation("exclude", insertFields);

            //Builds the query
            insert_build_fields_compiling(theQuery, insertFields);
        }

        /// <summary>
        /// <para>This function adds additional values to an insert statement</para>
        /// </summary>
        /// <param name="insertValues">single values or object[] only</param>
        public virtual void add_insert_values(object insertValues)
        {
            query theQuery = get_query();
            string definition = insert_definition + "_" + (theQueries.Count).ToString() + "_" + (theQuery.insert_values.Count).ToString() + "_";

            //Validation
            insert_not_exist_validation(theQuery);

            //Gets the data object *even if the value is just null it should be length 1 i.e. new object[] {null}
            object[] holding = add_data(insertValues);

            //Builds the query
            insert_build_compiling(theQuery, definition, holding);

            //Adds the real values to a list for binding and sanitization later
            theQuery.insert_real_values.Add(holding);
        }
    }
}