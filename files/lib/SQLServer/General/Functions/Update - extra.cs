namespace DotNetSDB
{
    public partial class SqlServerCore
    {
        /*##########################################*/
        /*           Main Front function            */
        /*##########################################*/
        
        /// <summary>
        /// This function adds additional update fields and values
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        public virtual void add_update_return(string tableName, string field, object value)
        {
            //Runs the original update
            add_update(tableName, field, value);

            //Converts the query object to QueryExtension
            QueryExtension theQuery = (QueryExtension)GetQuery();

            //Adds the new return feature
            theQuery.UpdateReturned = true;
        }

        /// <summary>
        /// This function adds additional update fields and values
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        public virtual void add_update_return(string tableName, string[] fields, object values)
        {
            //Runs the original update
            add_update(tableName, fields, values);

            //Converts the query object to Query2
            //Note: it will always be Query2 if this function is being hit as GetQuery is override to create Query2 objects.
            QueryExtension theQuery2 = (QueryExtension)GetQuery();

            //Adds the new return feature
            theQuery2.UpdateReturned = true;
        }
    }
}