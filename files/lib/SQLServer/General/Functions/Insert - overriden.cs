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

        private new void add_insert(string tableName, string field)
        {
        }

        private new void add_insert(string tableName, string[] fields)
        {
        }

        private new void add_insert(string tableName, object values)
        {
        }

        private new void add_insert(string tableName, string field, object value)
        {
        }

        private new void add_insert(string tableName, string[] fields, object values)
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
            Query theQuery = GetQuery();
            Query2 theQuery2 = GetQuery2();

            //Validation
            InsertExistValidation(theQuery);
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("Insert Error: The table name supplied is empty.");
            }

            //Holds the insert table
            theQuery.insertTableName = tableName;

            //Adds the new return feature
            theQuery2.insertReturn = returnInsert;

            //Adds the command
            theQuery.orderList.Add("insert");
        }

        /// <summary>
        /// This functions adds the insert statement with a field only
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="field"></param>
        /// <param name="returnInsert">returns all the inserted rows and fields</param>
        public virtual void add_insert(string tableName, string field, bool returnInsert = false)
        {
            Query theQuery = GetQuery();
            Query2 theQuery2 = GetQuery2();

            //Validation
            InsertExistValidation(theQuery);
            InsertsingleValidation(tableName, field);

            //Holds the insert table
            theQuery.insertTableName = tableName;

            //Builds the feidls query
            InsertFieldCompile(theQuery, field);

            //Adds the new return feature
            theQuery2.insertReturn = returnInsert;

            //Adds the command
            theQuery.orderList.Add("insert");
        }

        /// <summary>
        /// This functions adds the insert statement with fields only
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
        /// <param name="returnInsert">returns all the inserted rows and fields</param>
        public virtual void add_insert(string tableName, string[] fields, bool returnInsert = false)
        {
            Query theQuery = GetQuery();
            Query2 theQuery2 = GetQuery2();

            //Validation
            InsertExistValidation(theQuery);
            InsertMultipleValidation(tableName, fields);

            //Holds the insert table
            theQuery.insertTableName = tableName;

            //Builds the feidls query
            InsertFieldCompile(theQuery, fields);

            //Adds the new return feature
            theQuery2.insertReturn = returnInsert;

            //Adds the command
            theQuery.orderList.Add("insert");
        }

        /// <summary>
        /// This function adds the insert statement with only values
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="values">single value or object[] only</param>
        /// <param name="returnInsert">returns all the inserted rows and fields</param>
        public virtual void add_insert(string tableName, object values, bool returnInsert = false)
        {
            Query theQuery = GetQuery();
            Query2 theQuery2 = GetQuery2();
            string definition = string.Format("{0}_{1}_{2}_", insertDefinition , (theQueries.Count).ToString() , (theQuery.insertValues.Count).ToString());

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

            //Adds the new return feature
            theQuery2.insertReturn = returnInsert;

            //Adds the command
            theQuery.orderList.Add("insert");
        }

        /// <summary>
        /// This function adds the insert statement with a field and value
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="field"></param>
        /// <param name="value">single value only</param>
        /// <param name="returnInsert">returns all the inserted rows and fields</param>
        public virtual void add_insert(string tableName, string field, object value, bool returnInsert = false)
        {
            Query theQuery = GetQuery();
            Query2 theQuery2 = GetQuery2();
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

            //Adds the new return feature
            theQuery2.insertReturn = returnInsert;

            //Adds the command
            theQuery.orderList.Add("insert");
        }

        /// <summary>
        /// This function adds the insert statement with the fields and values
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
        /// <param name="values">object[] only</param>
        /// <param name="returnInsert">returns all the inserted rows and fields</param>
        public virtual void add_insert(string tableName, string[] fields, object values, bool returnInsert = false)
        {
            Query theQuery = GetQuery();
            Query2 theQuery2 = GetQuery2();
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

            //Adds the new return feature
            theQuery2.insertReturn = returnInsert;

            //Adds the command
            theQuery.orderList.Add("insert");
        }
    }
}