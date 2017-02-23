using System;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*     Create Table Compiling functions     */
        /*##########################################*/

        //This function builds the create table sql
        protected void CreateCompile(Query theQuery, string tableName, string field, string type)
        {
            if (!string.IsNullOrWhiteSpace(field) && !string.IsNullOrWhiteSpace(type))
            {
                //Do not change the main table name if its already set
                if (string.IsNullOrWhiteSpace(theQuery.createTable) && !string.IsNullOrWhiteSpace(tableName))
                {
                    //Sets the table name
                    theQuery.createTable = tableName;
                }

                string temp_build = "";

                //Builds the string
                temp_build += string.Format("{0} {1}", field, type);

                //Builds the final statement
                theQuery.createFields.Add(temp_build);
            }
            else
            {
                throw new Exception("Create Table Error: no field or type has been passed.");
            }
        }

        protected void CreateCompile(Query theQuery, string tablelName, string[] fields, string[] types)
        {
            if (fields != null && types != null)
            {
                //Checks if the sizes are not the same and if not returns a false
                if (fields.Length != types.Length)
                {
                    throw new Exception("Create Table Error: When using multiple fields there must be the same amount of field types passed.");
                }

                //Do not change the main table name if its already set
                if (string.IsNullOrWhiteSpace(theQuery.createTable) && !string.IsNullOrWhiteSpace(tablelName))
                {
                    //Sets the table name
                    theQuery.createTable = tablelName;
                }

                string temp_build = "";
                for (int i = 0; i < fields.Length; i++)
                {
                    //Determines if there should be a comma
                    string seperator = "";
                    if (i != 0)
                    {
                        seperator = ", ";
                    }

                    //Builds the string
                    temp_build += string.Format("{0}{1} {2}", seperator, fields[i], types[i]);
                }

                //Builds the final statement
                theQuery.createFields.Add(temp_build);
            }
            else
            {
                throw new Exception("Create Table Error: no fields or types have been passed.");
            }
        }

        /*##########################################*/
        /*        Create Validation functions       */
        /*##########################################*/

        protected void CreateSingleValidation(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("Delete Error: No table name has been supplied.");
            }
        }

        protected void CreateNotExistValidation(Query theQuery)
        {
            if (!theQuery.orderList.Contains("create"))
            {
                throw new Exception("Create Table Error: you cannot add additional fields and types without defining a main create statement first.");
            }
        }

        protected void CreateExistValidation(Query theQuery)
        {
            if (theQuery.orderList.Contains("create"))
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
            theQuery.orderList.Add("create");
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
            theQuery.orderList.Add("create");
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