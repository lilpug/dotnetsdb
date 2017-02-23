using System;
using System.Linq;

namespace DotNetSDB
{
    public partial class SqlServerCore
    {
        /*##########################################*/
        /*           Main Front function            */
        /*##########################################*/

        //Hides the original methods
        private new void add_update(string tableName, string field, object value)
        {
        }

        private new void add_update(string tableName, string[] fields, object values)
        {
        }

        //Implements the new override methods

        /// <summary>
        /// This function adds additional update fields and values
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="returnUpdated">returns all the updated rows and fields</param>
        public void add_update(string tableName, string field, object value, bool returnUpdated = false)
        {
            Query theQuery = GetQuery();
            Query2 theQuery2 = GetQuery2();

            UpdateExistValidation(theQuery);

            //Sets the definition
            string definition = string.Format("{0}_{1}_{2}_", updateDefinition ,(theQueries.Count).ToString() , (theQuery.updateFields.Count).ToString());

            //Processes the real object data
            object[] holding = AddData(value);
            if (holding.Count() != 1)
            {
                throw new Exception("Update Error: There is a different number of fields to the amount of values passed.");
            }

            //Builds the field sql
            UpdateCompile(theQuery, definition, tableName, field, holding);

            //Adds the real values
            theQuery.updateRealValues.Add(holding);

            //Updates the new feature in query 2
            theQuery2.updateReturned = returnUpdated;

            //Adds the command to the order list
            theQuery.orderList.Add("update");
        }

        /// <summary>
        /// This function adds additional update fields and values
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <param name="returnUpdated">returns all the updated rows and fields</param>
        public void add_update(string tableName, string[] fields, object values, bool returnUpdated = false)
        {
            Query theQuery = GetQuery();
            Query2 theQuery2 = GetQuery2();

            UpdateExistValidation(theQuery);

            //Sets the definition
            string definition = string.Format("{0}_{1}_{2}_", updateDefinition, (theQueries.Count).ToString(), (theQuery.updateFields.Count).ToString());

            //Processes the real object data
            object[] holding = AddData(values);
            if (holding.Count() != fields.Length)
            {
                throw new Exception("Update Error: There is a different number of fields to the amount of values passed.");
            }

            //Builds the field sql
            UpdateCompile(theQuery, definition, tableName, fields, holding);

            //Adds the real values
            theQuery.updateRealValues.Add(holding);

            //Updates the new feature in query 2
            theQuery2.updateReturned = returnUpdated;

            //Adds the command to the order list
            theQuery.orderList.Add("update");
        }
    }
}