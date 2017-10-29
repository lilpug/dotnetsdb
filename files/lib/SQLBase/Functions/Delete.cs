using System;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*        Delete Validation functions       */
        /*##########################################*/

        /// <summary>
        /// This function validates the delete query table name
        /// </summary>
        /// <param name="tableName"></param>
        protected void DeleteSingleValidation(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("Delete Error: No table name has been supplied.");
            }
        }

        /// <summary>
        /// This function validates that the delete query has not already been run as a new one is about to be added
        /// </summary>
        /// <param name="theQuery"></param>
        protected void DeleteExistsValidation(Query theQuery)
        {
            if (theQuery.OrderList.Contains("delete"))
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
            Query theQuery = GetQuery();

            DeleteExistsValidation(theQuery);

            DeleteSingleValidation(tableName);

            //Adds the delete table to the query
            theQuery.DeleteTable = tableName;

            //Adds the command
            theQuery.OrderList.Add("delete");
        }
    }
}