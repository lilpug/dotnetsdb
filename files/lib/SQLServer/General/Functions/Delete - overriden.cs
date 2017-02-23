namespace DotNetSDB
{
    public partial class SqlServerCore
    {
        /*##########################################*/
        /*           Main Front function            */
        /*##########################################*/

        private new void add_delete(string tableName)
        {
        }

        /// <summary>
        /// This function starts the delete query
        /// </summary>
        /// <param name="tableName">The Table that you want to delete from</param>
        /// <param name="returnDeleted">Returns all the deleted rows and fields</param>
        public void add_delete(string tableName, bool returnDeleted = false)
        {
            Query theQuery = GetQuery();
            Query2 theQuery2 = GetQuery2();

            DeleteExistsValidation(theQuery);

            DeleteSingleValidation(tableName);

            //Adds the delete table to the query
            theQuery.deleteTable = tableName;

            //Adds the extra feature to the query2
            theQuery2.deleteReturned = returnDeleted;

            //Adds the command
            theQuery.orderList.Add("delete");
        }
    }
}