using System;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*        Delete Validation functions       */
        /*##########################################*/

        protected void DeleteSingleValidation(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("Delete Error: No table name has been supplied.");
            }
        }

        protected void DeleteExistsValidation(Query theQuery)
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
            Query theQuery = GetQuery();

            DeleteExistsValidation(theQuery);

            DeleteSingleValidation(tableName);

            //Adds the delete table to the query
            theQuery.deleteTable = tableName;

            //Adds the command
            theQuery.orderList.Add("delete");
        }
    }
}