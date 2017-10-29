namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*           Main Front function            */
        /*##########################################*/
        /// <summary>
        /// This function adds a drop table statement
        /// </summary>
        /// <param name="tableName"></param>
        public virtual void add_drop_table(string tableName)
        {
            Query theQuery = GetQuery();

            theQuery.DropTableName = tableName;

            theQuery.OrderList.Add("drop");

            start_new_query();
        }
    }
}