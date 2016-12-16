namespace DotNetSDB
{
    public partial class SqlServerCore
    {
        /*##########################################*/
        /*           Main Front function            */
        /*##########################################*/

        //This function sets the top flag with the specified parameter
        //Note: THIS FUNCTION IS A SQL SERVER ONLY!

        /// <summary>
        /// This function adds a top to the select statement
        /// </summary>
        /// <param name="topValue"></param>
        public void add_select_top(int topValue)
        {
            //Obtains the current query object
            query2 theQuery = get_query2();
            theQuery.select_top = topValue;
        }
    }
}