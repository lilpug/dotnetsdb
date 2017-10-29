namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*         Compiling Wrapper functions      */
        /*##########################################*/

        /// <summary>
        /// This function compiles the query start wrapper and adds it to the query being built
        /// </summary>
        /// <param name="current"></param>
        protected virtual void CompileStartWrapper(Query current)
        {
            if (!string.IsNullOrWhiteSpace(current.SqlStartWrapper))
            {
                compiledSql.Append($" {current.SqlStartWrapper}");
            }
        }

        /// <summary>
        /// This function compiles the query end wrapper and adds it to the query being built
        /// </summary>
        protected virtual void CompileEndWrapper(Query current)
        {
            if (!string.IsNullOrWhiteSpace(current.SqlEndWrapper))
            {
                compiledSql.Append($" {current.SqlEndWrapper}");
            }
        }
    }
}