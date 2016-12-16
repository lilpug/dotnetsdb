using System;
using System.Linq;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*            Update functions              */
        /*##########################################*/

        //This function compiles the update fields
        protected void update_build_compiling(query theQuery, string definition, string table_name, string update_field, params object[] update_values)
        {
            if (string.IsNullOrWhiteSpace(table_name))
            {
                throw new Exception("Update Error: The table name supplied is empty.");
            }

            if (!string.IsNullOrWhiteSpace(update_field))
            {
                //Do not change the main table name if its already set
                if (string.IsNullOrWhiteSpace(theQuery.update_table) && !string.IsNullOrWhiteSpace(table_name))
                {
                    //Sets the table name
                    theQuery.update_table = table_name;
                }

                string temp_build = string.Format("{0}.{1} = {2}0", table_name, update_field, definition);

                //Builds the final statement
                theQuery.update_fields.Add(temp_build);
            }
            else
            {
                throw new Exception("Update Error: There is no fields supplied.");
            }
        }

        protected void update_build_compiling(query theQuery, string definition, string table_name, string[] update_fields, params object[] update_values)
        {
            if (string.IsNullOrWhiteSpace(table_name))
            {
                throw new Exception("Update Error: The table name supplied is empty.");
            }

            //Checks  to ensure the data is accurate as there should be one field for every data insert
            if (update_fields != null && update_fields.Count() > 0)
            {
                //Do not change the main table name if its already set
                if (string.IsNullOrWhiteSpace(theQuery.update_table) && !string.IsNullOrWhiteSpace(table_name))
                {
                    //Sets the table name
                    theQuery.update_table = table_name;
                }

                string temp_build = "";

                for (int i = 0; i < update_fields.Length; i++)//Compiles the query string
                {
                    //Determines if there should be a comma
                    string seperator = "";
                    if (i != 0)
                    {
                        seperator = ", ";
                    }

                    temp_build += string.Format("{0}{1}.{2} = {3}", seperator, table_name, update_fields[i], definition + i.ToString());
                }

                //Builds the final statement
                theQuery.update_fields.Add(temp_build);
            }
            else
            {
                throw new Exception("Update Error: There is no fields supplied.");
            }
        }

        /*##########################################*/
        /*       Update Validation functions        */
        /*##########################################*/

        protected void update_not_exist_validate(query theQuery)
        {
            if (!theQuery.orderList.Contains("update"))
            {
                throw new Exception("Update Error: you cannot add update fields without defining a main update statement first.");
            }
        }

        protected void update_exist_validate(query theQuery)
        {
            if (theQuery.orderList.Contains("update"))
            {
                throw new Exception("Update Error: a main update statement has already been defined, for additional fields use add_update_fields.");
            }
        }

        /*##########################################*/
        /*           Main Front function            */
        /*##########################################*/

        /// <summary>
        /// This function adds the update statement
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="updateField"></param>
        /// <param name="updateValue">Single value</param>
        public void add_update(string tableName, string updateField, object updateValue)
        {
            query theQuery = get_query();

            update_exist_validate(theQuery);

            string definition = update_definition + "_" + (theQueries.Count).ToString() + "_" + (theQuery.update_fields.Count).ToString() + "_";

            object[] holding = add_data(updateValue);
            if (holding.Count() != 1)
            {
                throw new Exception("Update Error: There is a different number of fields to the amount of values passed.");
            }

            update_build_compiling(theQuery, definition, tableName, updateField, holding);

            theQuery.update_real_values.Add(holding);

            theQuery.orderList.Add("update");
        }

        /// <summary>
        /// This function adds the update statement
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="updateFields"></param>
        /// <param name="updateValues">object[] only</param>
        public void add_update(string tableName, string[] updateFields, object updateValues)
        {
            query theQuery = get_query();

            update_exist_validate(theQuery);

            string definition = update_definition + "_" + (theQueries.Count).ToString() + "_" + (theQuery.update_fields.Count).ToString() + "_";

            object[] holding = add_data(updateValues);
            if (holding.Count() != updateFields.Length)
            {
                throw new Exception("Update Error: There is a different number of fields to the amount of values passed.");
            }

            update_build_compiling(theQuery, definition, tableName, updateFields, holding);

            theQuery.update_real_values.Add(holding);

            theQuery.orderList.Add("update");
        }

        /// <summary>
        /// This function adds additional update fields and values
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="updateField"></param>
        /// <param name="updateValue">Single value</param>
        public void add_update_fields(string tableName, string updateField, object updateValue)
        {
            query theQuery = get_query();

            update_not_exist_validate(theQuery);

            string definition = update_definition + "_" + (theQueries.Count).ToString() + "_" + (theQuery.update_fields.Count).ToString() + "_";

            object[] holding = add_data(updateValue);
            if (holding.Count() != 1)
            {
                throw new Exception("Update Error: There is a different number of fields to the amount of values passed.");
            }

            update_build_compiling(theQuery, definition, tableName, updateField, holding);

            theQuery.update_real_values.Add(holding);
        }

        /// <summary>
        /// This function adds additional update fields and values
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="updateFields"></param>
        /// <param name="updateValues">object[] only</param>
        public void add_update_fields(string tableName, string[] updateFields, object updateValues)
        {
            query theQuery = get_query();

            update_not_exist_validate(theQuery);

            string definition = update_definition + "_" + (theQueries.Count).ToString() + "_" + (theQuery.update_fields.Count).ToString() + "_";

            object[] holding = add_data(updateValues);
            if (holding.Count() != updateFields.Length)
            {
                throw new Exception("Update Error: There is a different number of fields to the amount of values passed.");
            }

            update_build_compiling(theQuery, definition, tableName, updateFields, holding);

            theQuery.update_real_values.Add(holding);
        }
    }
}