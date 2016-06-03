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
            query theQuery = get_query();
            query2 theQuery2 = get_query2();

            delete_exist_validation(theQuery);

            delete_single_validation(tableName);

            //Adds the delete table to the query
            theQuery.delete_table = tableName;

            //Adds the extra feature to the query2
            theQuery2.delete_returned = returnDeleted;

            //Adds the command
            theQuery.orderList.Add("delete");
        }
    }
}