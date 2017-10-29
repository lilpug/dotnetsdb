using System;
using System.Linq;
using System.Text;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*            Update functions              */
        /*##########################################*/

        /// <summary>
        /// This function deals with creating the update SQL
        /// </summary>
        /// <param name="theQuery"></param>
        /// <param name="definition"></param>
        /// <param name="tableName"></param>
        /// <param name="updateField"></param>
        /// <param name="updateValues"></param>
        protected void UpdateCompile(Query theQuery, string definition, string tableName, string updateField, params object[] updateValues)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("Update Error: The table name supplied is empty.");
            }

            if (!string.IsNullOrWhiteSpace(updateField))
            {
                //Do not change the main table name if its already set
                if (string.IsNullOrWhiteSpace(theQuery.UpdateTable) && !string.IsNullOrWhiteSpace(tableName))
                {
                    //Sets the table name
                    theQuery.UpdateTable = tableName;
                }
                
                //Builds the final statement
                theQuery.UpdateFields.Add($"{tableName}.{updateField} = {definition}0");
            }
            else
            {
                throw new Exception("Update Error: There is no fields supplied.");
            }
        }

        /// <summary>
        /// This function deals with creating the update SQL
        /// </summary>
        /// <param name="theQuery"></param>
        /// <param name="definition"></param>
        /// <param name="tableName"></param>
        /// <param name="updateFields"></param>
        /// <param name="updateValues"></param>
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
                if (string.IsNullOrWhiteSpace(theQuery.UpdateTable) && !string.IsNullOrWhiteSpace(tableName))
                {
                    //Sets the table name
                    theQuery.UpdateTable = tableName;
                }

                StringBuilder sb = new StringBuilder();
                
                for (int i = 0; i < updateFields.Length; i++)//Compiles the query string
                {
                    //Determines if there should be a comma
                    if (i != 0)
                    {
                        sb.Append(", ");
                    }

                    sb.Append($"{tableName}.{updateFields[i]} = {definition + i}");
                }

                //Builds the final statement
                theQuery.UpdateFields.Add(sb.ToString());
            }
            else
            {
                throw new Exception("Update Error: There is no fields supplied.");
            }
        }

        /*##########################################*/
        /*       Update Validation functions        */
        /*##########################################*/

        /// <summary>
        /// This function validates that the update base statement has been added before trying to add additional fields
        /// </summary>
        /// <param name="theQuery"></param>
        protected void UpdateNotExistValidation(Query theQuery)
        {
            if (!theQuery.OrderList.Contains("update"))
            {
                throw new Exception("Update Error: you cannot add update fields without defining a main update statement first.");
            }
        }

        /// <summary>
        /// This function validates that the update query has not already been run as a new one is about to be added
        /// </summary>
        /// <param name="theQuery"></param>
        protected void UpdateExistValidation(Query theQuery)
        {
            if (theQuery.OrderList.Contains("update"))
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

            string definition = $"{updateDefinition}_{theQueries.Count}_{theQuery.UpdateFields.Count}_";

            object[] holding = AddData(value);
            if (holding.Count() != 1)
            {
                throw new Exception("Update Error: There is a different number of fields to the amount of values passed.");
            }

            UpdateCompile(theQuery, definition, tableName, field, holding);

            theQuery.UpdateRealValues.Add(holding);

            theQuery.OrderList.Add("update");
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

            string definition = $"{updateDefinition}_{theQueries.Count}_{theQuery.UpdateFields.Count}_";

            object[] holding = AddData(values);
            if (holding.Count() != fields.Length)
            {
                throw new Exception("Update Error: There is a different number of fields to the amount of values passed.");
            }

            UpdateCompile(theQuery, definition, tableName, fields, holding);

            theQuery.UpdateRealValues.Add(holding);

            theQuery.OrderList.Add("update");
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

            string definition = $"{updateDefinition}_{theQueries.Count}_{theQuery.UpdateFields.Count}_";

            object[] holding = AddData(value);
            if (holding.Count() != 1)
            {
                throw new Exception("Update Error: There is a different number of fields to the amount of values passed.");
            }

            UpdateCompile(theQuery, definition, tableName, field, holding);

            theQuery.UpdateRealValues.Add(holding);
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

            string definition = $"{updateDefinition}_{theQueries.Count}_{theQuery.UpdateFields.Count}_";

            object[] holding = AddData(values);
            if (holding.Count() != fields.Length)
            {
                throw new Exception("Update Error: There is a different number of fields to the amount of values passed.");
            }

            UpdateCompile(theQuery, definition, tableName, fields, holding);

            theQuery.UpdateRealValues.Add(holding);
        }
    }
}