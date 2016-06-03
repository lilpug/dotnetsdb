using System;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*        Delete Validation functions       */
        /*##########################################*/

        protected void delete_single_validation(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("Delete Error: No table name has been supplied.");
            }
        }

        protected void delete_exist_validation(query theQuery)
        {
            if (theQuery.orderList.Contains("delete"))
            {
                throw new Exception("Delete Error: a main delete statement has already been defined.");
            }
        }

        /*##########################################*/
        /*           Main Front function            */
        /*##########################################*/

        /// <summary>
        /// This function adds the delete statement
        /// </summary>
        /// <param name="tableName"></param>
        public virtual void add_delete(string tableName)
        {
            query theQuery = get_query();

            delete_exist_validation(theQuery);

            delete_single_validation(tableName);

            //Adds the delete table to the query
            theQuery.delete_table = tableName;

            //Adds the command
            theQuery.orderList.Add("delete");
        }
    }
}