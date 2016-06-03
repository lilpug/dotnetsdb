using System;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*     Create Table Compiling functions     */
        /*##########################################*/

        //This function builds the create table sql
        protected void create_table_compiling(query theQuery, string table_name, string create_field, string create_type)
        {
            if (!string.IsNullOrWhiteSpace(create_field) && !string.IsNullOrWhiteSpace(create_type))
            {
                //Do not change the main table name if its already set
                if (string.IsNullOrWhiteSpace(theQuery.create_table) && !string.IsNullOrWhiteSpace(table_name))
                {
                    //Sets the table name
                    theQuery.create_table = table_name;
                }

                string temp_build = "";

                //Builds the string
                temp_build += string.Format("{0} {1}", create_field, create_type);

                //Builds the final statement
                theQuery.create_fields.Add(temp_build);
            }
            else
            {
                throw new Exception("Create Table Error: no field or type has been passed.");
            }
        }

        protected void create_table_compiling(query theQuery, string table_name, string[] create_fields, string[] create_types)
        {
            if (create_fields != null && create_types != null)
            {
                //Checks if the sizes are not the same and if not returns a false
                if (create_fields.Length != create_types.Length)
                {
                    throw new Exception("Create Table Error: When using multiple fields there must be the same amount of field types passed.");
                }

                //Do not change the main table name if its already set
                if (string.IsNullOrWhiteSpace(theQuery.create_table) && !string.IsNullOrWhiteSpace(table_name))
                {
                    //Sets the table name
                    theQuery.create_table = table_name;
                }

                string temp_build = "";
                for (int i = 0; i < create_fields.Length; i++)
                {
                    //Determines if there should be a comma
                    string seperator = "";
                    if (i != 0)
                    {
                        seperator = ", ";
                    }

                    //Builds the string
                    temp_build += string.Format("{0}{1} {2}", seperator, create_fields[i], create_types[i]);
                }

                //Builds the final statement
                theQuery.create_fields.Add(temp_build);
            }
            else
            {
                throw new Exception("Create Table Error: no fields or types have been passed.");
            }
        }

        /*##########################################*/
        /*        Create Validation functions       */
        /*##########################################*/

        protected void create_single_validation(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("Delete Error: No table name has been supplied.");
            }
        }

        protected void create_not_exist_validation(query theQuery)
        {
            if (!theQuery.orderList.Contains("create"))
            {
                throw new Exception("Create Table Error: you cannot add additional fields and types without defining a main create statement first.");
            }
        }

        protected void create_exist_validation(query theQuery)
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
        /// <param name="newFieldsNames"></param>
        /// <param name="newFieldTypes"></param>
        public void add_create_table(string newTableName, string newFieldsName, string newFieldType)
        {
            query theQuery = get_query();

            create_exist_validation(theQuery);

            create_table_compiling(theQuery, newTableName, newFieldsName, newFieldType);

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
            query theQuery = get_query();

            create_exist_validation(theQuery);

            create_table_compiling(theQuery, newTableName, newFieldsNames, newFieldTypes);

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
            query theQuery = get_query();

            create_not_exist_validation(theQuery);

            create_table_compiling(theQuery, null, newFieldsName, newFieldType);
        }

        /// <summary>
        /// This function adds additional fields and types to the create table statement
        /// </summary>
        /// <param name="newFieldsNames"></param>
        /// <param name="newFieldTypes"></param>
        public void add_create_fields(string[] newFieldsNames, string[] newFieldTypes)
        {
            query theQuery = get_query();

            create_not_exist_validation(theQuery);

            create_table_compiling(theQuery, null, newFieldsNames, newFieldTypes);
        }
    }
}