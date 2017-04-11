
namespace DotNetSDB
{
    public partial class SqlServerCore
    {
        /*##########################################*/
        /*       Table Fields SQL functions         */
        /*##########################################*/

        /// <summary>
        /// This function returns all the column names of a table
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public virtual string[] table_fields(string tableName)
        {
            Query2 theQuery = GetQuery2();

            theQuery.getFieldsRealTableValue = AddData(tableName);

            //Gives the sql string to the compiled_build
            compiledSql = $"SELECT column_name FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = {fieldsDefinition}_1_0_0 ORDER BY ordinal_position;";
            
            return run_return_string_array();
        }
    }
}