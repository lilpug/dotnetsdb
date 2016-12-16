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
        private new void add_update(string tableName, string updateField, object updateValue)
        {
        }

        private new void add_update(string tableName, string[] updateFields, object updateValues)
        {
        }

        //Implements the new override methods

        /// <summary>
        /// This function adds additional update fields and values
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="updateField"></param>
        /// <param name="updateValue"></param>
        /// <param name="returnUpdated">returns all the updated rows and fields</param>
        public void add_update(string tableName, string updateField, object updateValue, bool returnUpdated = false)
        {
            query theQuery = get_query();
            query2 theQuery2 = get_query2();

            update_exist_validate(theQuery);

            //Sets the definition
            string definition = update_definition + "_" + (theQueries.Count).ToString() + "_" + (theQuery.update_fields.Count).ToString() + "_";

            //Processes the real object data
            object[] holding = add_data(updateValue);
            if (holding.Count() != 1)
            {
                throw new Exception("Update Error: There is a different number of fields to the amount of values passed.");
            }

            //Builds the field sql
            update_build_compiling(theQuery, definition, tableName, updateField, holding);

            //Adds the real values
            theQuery.update_real_values.Add(holding);

            //Updates the new feature in query 2
            theQuery2.update_returned = returnUpdated;

            //Adds the command to the order list
            theQuery.orderList.Add("update");
        }

        /// <summary>
        /// This function adds additional update fields and values
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="updateFields"></param>
        /// <param name="updateValues"></param>
        /// <param name="returnUpdated">returns all the updated rows and fields</param>
        public void add_update(string tableName, string[] updateFields, object updateValues, bool returnUpdated = false)
        {
            query theQuery = get_query();
            query2 theQuery2 = get_query2();

            update_exist_validate(theQuery);

            //Sets the definition
            string definition = update_definition + "_" + (theQueries.Count).ToString() + "_" + (theQuery.update_fields.Count).ToString() + "_";

            //Processes the real object data
            object[] holding = add_data(updateValues);
            if (holding.Count() != updateFields.Length)
            {
                throw new Exception("Update Error: There is a different number of fields to the amount of values passed.");
            }

            //Builds the field sql
            update_build_compiling(theQuery, definition, tableName, updateFields, holding);

            //Adds the real values
            theQuery.update_real_values.Add(holding);

            //Updates the new feature in query 2
            theQuery2.update_returned = returnUpdated;

            //Adds the command to the order list
            theQuery.orderList.Add("update");
        }
    }
}