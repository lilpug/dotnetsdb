using System;
using System.Text;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*     Create Table Compiling functions     */
        /*##########################################*/

        /// <summary>
        /// This function deals with creating the create table SQL
        /// </summary>
        /// <param name="theQuery"></param>
        /// <param name="tableName"></param>
        /// <param name="field"></param>
        /// <param name="type"></param>
        protected void CreateCompile(Query theQuery, string tableName, string field, string type)
        {
            //Validates the table name is ok
            CreateTableNameValidation(tableName);

            if (!string.IsNullOrWhiteSpace(field) && !string.IsNullOrWhiteSpace(type))
            {
                //Do not change the main table name if its already set
                if (string.IsNullOrWhiteSpace(theQuery.CreateTable) && !string.IsNullOrWhiteSpace(tableName))
                {
                    //Sets the table name
                    theQuery.CreateTable = tableName;
                }

                //Builds the final statement
                theQuery.CreateFields.Add($"{field} {type}");
            }
            else
            {
                throw new Exception("Create Table Error: no field or type has been passed.");
            }
        }

        /// <summary>
        /// This function builds the create table SQL
        /// </summary>
        /// <param name="theQuery"></param>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
        /// <param name="types"></param>
        protected void CreateCompile(Query theQuery, string tableName, string[] fields, string[] types)
        {
            //Validates the table name is ok
            CreateTableNameValidation(tableName);

            if (fields != null && types != null)
            {
                //Checks if the sizes are not the same and if not returns a false
                if (fields.Length != types.Length)
                {
                    throw new Exception("Create Table Error: When using multiple fields there must be the same amount of field types passed.");
                }

                //Do not change the main table name if its already set
                if (string.IsNullOrWhiteSpace(theQuery.CreateTable) && !string.IsNullOrWhiteSpace(tableName))
                {
                    //Sets the table name
                    theQuery.CreateTable = tableName;
                }

                StringBuilder sb = new StringBuilder();
                
                for (int i = 0; i < fields.Length; i++)
                {
                    //Determines if there should be a comma
                    if (i != 0)
                    {
                        sb.Append(", ");
                    }

                    //Builds the string
                    sb.Append($"{fields[i]} {types[i]}");
                }

                //Builds the final statement
                theQuery.CreateFields.Add(sb.ToString());
            }
            else
            {
                throw new Exception("Create Table Error: no fields or types have been passed.");
            }
        }

        /*##########################################*/
        /*        Create Validation functions       */
        /*##########################################*/

        /// <summary>
        /// This function validates the create table name
        /// </summary>
        /// <param name="tableName"></param>
        protected void CreateTableNameValidation(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("Create Table Error: No table name has been supplied.");
            }
        }

        /// <summary>
        /// This function validates that the create table base statement has been added before trying to add additional fields
        /// </summary>
        /// <param name="theQuery"></param>
        protected void CreateNotExistValidation(Query theQuery)
        {
            if (!theQuery.OrderList.Contains("create"))
            {
                throw new Exception("Create Table Error: you cannot add additional fields and types without defining a main create statement first.");
            }
        }

        /// <summary>
        /// This function validates that the create table query has not already been run as a new one is about to be added
        /// </summary>
        /// <param name="theQuery"></param>
        protected void CreateExistValidation(Query theQuery)
        {
            if (theQuery.OrderList.Contains("create"))
            {
                throw new Exception("Create Table Error: a main create table statement has already been defined, for additional fields and types use add_create_fields.");
            }
        }

        /*##########################################*/
        /*           Main Front function            */
        /*##########################################*/

        /// <summary>
        /// This function creates the main create table statement
        /// </summary>
        /// <param name="newTableName"></param>
        /// <param name="newFieldsName"></param>
        /// <param name="newFieldType"></param>
        public void add_create_table(string newTableName, string newFieldsName, string newFieldType)
        {
            Query theQuery = GetQuery();

            CreateExistValidation(theQuery);

            CreateCompile(theQuery, newTableName, newFieldsName, newFieldType);

            //Adds the command
            theQuery.OrderList.Add("create");
        }

        /// <summary>
        /// This function creates the main create table statement
        /// </summary>
        /// <param name="newTableName"></param>
        /// <param name="newFieldsNames"></param>
        /// <param name="newFieldTypes"></param>
        public void add_create_table(string newTableName, string[] newFieldsNames, string[] newFieldTypes)
        {
            Query theQuery = GetQuery();

            CreateExistValidation(theQuery);

            CreateCompile(theQuery, newTableName, newFieldsNames, newFieldTypes);

            //Adds the command
            theQuery.OrderList.Add("create");
        }

        /// <summary>
        /// This function adds additional fields and types to the create table statement
        /// </summary>
        /// <param name="newFieldsName"></param>
        /// <param name="newFieldType"></param>
        public void add_create_fields(string newFieldsName, string newFieldType)
        {
            Query theQuery = GetQuery();

            CreateNotExistValidation(theQuery);

            CreateCompile(theQuery, null, newFieldsName, newFieldType);
        }

        /// <summary>
        /// This function adds additional fields and types to the create table statement
        /// </summary>
        /// <param name="newFieldsNames"></param>
        /// <param name="newFieldTypes"></param>
        public void add_create_fields(string[] newFieldsNames, string[] newFieldTypes)
        {
            Query theQuery = GetQuery();

            CreateNotExistValidation(theQuery);

            CreateCompile(theQuery, null, newFieldsNames, newFieldTypes);
        }
    }
}