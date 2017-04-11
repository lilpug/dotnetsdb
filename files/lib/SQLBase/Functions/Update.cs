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
        protected void UpdateCompile(Query theQuery, string definition, string tableName, string updateField, params object[] updateValues)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("Update Error: The table name supplied is empty.");
            }

            if (!string.IsNullOrWhiteSpace(updateField))
            {
                //Do not change the main table name if its already set
                if (string.IsNullOrWhiteSpace(theQuery.updateTable) && !string.IsNullOrWhiteSpace(tableName))
                {
                    //Sets the table name
                    theQuery.updateTable = tableName;
                }

                string temp_build = $"{tableName}.{updateField} = {definition}0";

                //Builds the final statement
                theQuery.updateFields.Add(temp_build);
            }
            else
            {
                throw new Exception("Update Error: There is no fields supplied.");
            }
        }

        protected void UpdateCompile(Query theQuery, string definition, string tableName, string[] updateFields, params object[] updateValues)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("Update Error: The table name supplied is empty.");
            }

            //Checks  to ensure the data is accurate as there should be one field for every data insert
            if (updateFields != null && updateFields.Count() > 0)
            {
                //Do not change the main table name if its already set
                if (string.IsNullOrWhiteSpace(theQuery.updateTable) && !string.IsNullOrWhiteSpace(tableName))
                {
                    //Sets the table name
                    theQuery.updateTable = tableName;
                }

                string temp_build = "";

                for (int i = 0; i < updateFields.Length; i++)//Compiles the query string
                {
                    //Determines if there should be a comma
                    string seperator = "";
                    if (i != 0)
                    {
                        seperator = ", ";
                    }

                    temp_build += $"{seperator}{tableName}.{updateFields[i]} = {definition + i}";
                }

                //Builds the final statement
                theQuery.updateFields.Add(temp_build);
            }
            else
            {
                throw new Exception("Update Error: There is no fields supplied.");
            }
        }

        /*##########################################*/
        /*       Update Validation functions        */
        /*##########################################*/

        protected void UpdateNotExistValidation(Query theQuery)
        {
            if (!theQuery.orderList.Contains("update"))
            {
                throw new Exception("Update Error: you cannot add update fields without defining a main update statement first.");
            }
        }

        protected void UpdateExistValidation(Query theQuery)
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
        /// <param name="field"></param>
        /// <param name="value">Single value</param>
        public void add_update(string tableName, string field, object value)
        {
            Query theQuery = GetQuery();

            UpdateExistValidation(theQuery);

            string definition = $"{updateDefinition}_{theQueries.Count}_{theQuery.updateFields.Count}_";

            object[] holding = AddData(value);
            if (holding.Count() != 1)
            {
                throw new Exception("Update Error: There is a different number of fields to the amount of values passed.");
            }

            UpdateCompile(theQuery, definition, tableName, field, holding);

            theQuery.updateRealValues.Add(holding);

            theQuery.orderList.Add("update");
        }

        /// <summary>
        /// This function adds the update statement
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
        /// <param name="values">object[] only</param>
        public void add_update(string tableName, string[] fields, object values)
        {
            Query theQuery = GetQuery();

            UpdateExistValidation(theQuery);

            string definition = $"{updateDefinition}_{theQueries.Count}_{theQuery.updateFields.Count}_";

            object[] holding = AddData(values);
            if (holding.Count() != fields.Length)
            {
                throw new Exception("Update Error: There is a different number of fields to the amount of values passed.");
            }

            UpdateCompile(theQuery, definition, tableName, fields, holding);

            theQuery.updateRealValues.Add(holding);

            theQuery.orderList.Add("update");
        }

        /// <summary>
        /// This function adds additional update fields and values
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="field"></param>
        /// <param name="value">Single value</param>
        public void add_update_additions(string tableName, string field, object value)
        {
            Query theQuery = GetQuery();

            UpdateNotExistValidation(theQuery);

            string definition = $"{updateDefinition}_{theQueries.Count}_{theQuery.updateFields.Count}_";

            object[] holding = AddData(value);
            if (holding.Count() != 1)
            {
                throw new Exception("Update Error: There is a different number of fields to the amount of values passed.");
            }

            UpdateCompile(theQuery, definition, tableName, field, holding);

            theQuery.updateRealValues.Add(holding);
        }

        /// <summary>
        /// This function adds additional update fields and values
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
        /// <param name="values">object[] only</param>
        public void add_update_additions(string tableName, string[] fields, object values)
        {
            Query theQuery = GetQuery();

            UpdateNotExistValidation(theQuery);

            string definition = $"{updateDefinition}_{theQueries.Count}_{theQuery.updateFields.Count}_";

            object[] holding = AddData(values);
            if (holding.Count() != fields.Length)
            {
                throw new Exception("Update Error: There is a different number of fields to the amount of values passed.");
            }

            UpdateCompile(theQuery, definition, tableName, fields, holding);

            theQuery.updateRealValues.Add(holding);
        }
    }
}