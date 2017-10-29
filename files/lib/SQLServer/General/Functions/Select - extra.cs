namespace DotNetSDB
{
    public partial class SqlServerCore
    {
        /*##########################################*/
        /*           Main Front function            */
        /*##########################################*/

        //This function sets the top flag with the specified parameter

        /// <summary>
        /// This function adds a top to the select statement
        /// </summary>
        /// <param name="topValue"></param>
        public virtual void add_select_top(int topValue)
        {
            //Converts the query object to QueryExtension
            QueryExtension theQuery = (QueryExtension)GetQuery();

            theQuery.SelectTop = topValue;
        }
    }
}