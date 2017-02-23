namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*           Main Front function            */
        /*##########################################*/

        public void add_drop_table(string tableName)
        {
            Query theQuery = GetQuery();

            theQuery.dropTableName = tableName;

            theQuery.orderList.Add("drop");

            start_new_query();
        }
    }
}