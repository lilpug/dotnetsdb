namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*           Main Front function            */
        /*##########################################*/

        public void add_drop_table(string tableName)
        {
            query theQuery = get_query();

            theQuery.drop_table_name = tableName;

            theQuery.orderList.Add("drop");

            start_new_query();
        }
    }
}