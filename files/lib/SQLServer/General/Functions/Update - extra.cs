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

            Query2 theQuery2 = GetQuery2();

            //Adds the new return feature
            theQuery2.updateReturned = true;
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

            Query2 theQuery2 = GetQuery2();

            //Adds the new return feature
            theQuery2.updateReturned = true;
        }
    }
}