namespace DotNetSDB
{
    public partial class SqlServerCore
    {
        /*##########################################*/
        /*           Main Front function            */
        /*##########################################*/

        /// <summary>
        /// This function starts the delete query
        /// </summary>
        /// <param name="tableName">The Table that you want to delete from</param>
        public virtual void add_delete_return(string tableName)
        {
            //Runs the original delete
            add_delete(tableName);            

            Query2 theQuery2 = GetQuery2();
            
            //Adds the extra feature to the query2
            theQuery2.deleteReturned = true;
        }
    }
}