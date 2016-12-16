using System;

namespace DotNetSDB
{
    public partial class SqlServerCore
    {
        /*##########################################*/
        /*             Main Front function          */
        /*##########################################*/

        //Hides the original methods
        private new void add_insert(string tableName)
        {
        }

        private new void add_insert(string tableName, string insertField)
        {
        }

        private new void add_insert(string tableName, string[] insertFields)
        {
        }

        private new void add_insert(string tableName, object insertValues)
        {
        }

        private new void add_insert(string tableName, string insertField, object insertValue)
        {
        }

        private new void add_insert(string tableName, string[] insertFields, object insertValues)
        {
        }

        //Implements the new override methods

        /// <summary>
        /// This function adds the insert statement without fields or values
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="returnInsert">returns all the inserted rows and fields</param>
        public virtual void add_insert(string tableName, bool returnInsert = false)
        {
            query theQuery = get_query();
            query2 theQuery2 = get_query2();

            //Validation
            insert_exist_validation(theQuery);
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("Insert Error: The table name supplied is empty.");
            }

            //Holds the insert table
            theQuery.insert_table_name = tableName;

            //Adds the new return feature
            theQuery2.insert_return = returnInsert;

            //Adds the command
            theQuery.orderList.Add("insert");
        }

        /// <summary>
        /// This functions adds the insert statement with a field only
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="insertField"></param>
        /// <param name="returnInsert">returns all the inserted rows and fields</param>
        public virtual void add_insert(string tableName, string insertField, bool returnInsert = false)
        {
            query theQuery = get_query();
            query2 theQuery2 = get_query2();

            //Validation
            insert_exist_validation(theQuery);
            insert_single_validation(tableName, insertField);

            //Holds the insert table
            theQuery.insert_table_name = tableName;

            //Builds the feidls query
            insert_build_fields_compiling(theQuery, insertField);

            //Adds the new return feature
            theQuery2.insert_return = returnInsert;

            //Adds the command
            theQuery.orderList.Add("insert");
        }

        /// <summary>
        /// This functions adds the insert statement with fields only
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="insertFields"></param>
        /// <param name="returnInsert">returns all the inserted rows and fields</param>
        public virtual void add_insert(string tableName, string[] insertFields, bool returnInsert = false)
        {
            query theQuery = get_query();
            query2 theQuery2 = get_query2();

            //Validation
            insert_exist_validation(theQuery);
            insert_multiple_validation(tableName, insertFields);

            //Holds the insert table
            theQuery.insert_table_name = tableName;

            //Builds the feidls query
            insert_build_fields_compiling(theQuery, insertFields);

            //Adds the new return feature
            theQuery2.insert_return = returnInsert;

            //Adds the command
            theQuery.orderList.Add("insert");
        }

        /// <summary>
        /// This function adds the insert statement with only values
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="insertValues">single value or object[] only</param>
        /// <param name="returnInsert">returns all the inserted rows and fields</param>
        public virtual void add_insert(string tableName, object insertValues, bool returnInsert = false)
        {
            query theQuery = get_query();
            query2 theQuery2 = get_query2();
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

            //Adds the new return feature
            theQuery2.insert_return = returnInsert;

            //Adds the command
            theQuery.orderList.Add("insert");
        }

        /// <summary>
        /// This function adds the insert statement with a field and value
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="insertField"></param>
        /// <param name="insertValue">single value only</param>
        /// <param name="returnInsert">returns all the inserted rows and fields</param>
        public virtual void add_insert(string tableName, string insertField, object insertValue, bool returnInsert = false)
        {
            query theQuery = get_query();
            query2 theQuery2 = get_query2();
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

            //Adds the new return feature
            theQuery2.insert_return = returnInsert;

            //Adds the command
            theQuery.orderList.Add("insert");
        }

        /// <summary>
        /// This function adds the insert statement with the fields and values
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="insertFields"></param>
        /// <param name="insertValues">object[] only</param>
        /// <param name="returnInsert">returns all the inserted rows and fields</param>
        public virtual void add_insert(string tableName, string[] insertFields, object insertValues, bool returnInsert = false)
        {
            query theQuery = get_query();
            query2 theQuery2 = get_query2();
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

            //Adds the new return feature
            theQuery2.insert_return = returnInsert;

            //Adds the command
            theQuery.orderList.Add("insert");
        }
    }
}