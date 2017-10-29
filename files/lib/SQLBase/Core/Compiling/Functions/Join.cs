namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*            Compiling Join functions      */
        /*##########################################*/

        /// <summary>
        /// This function compiles the join query and adds it to the query being built
        /// </summary>
        /// <param name="current"></param>
        protected virtual void CompileJoin(Query current)
        {
            //Compiles the first join in the list and adds it to the query
            compiledSql.Append($" {current.JoinFields[0]}");

            //Removes the current first join so if there are multiple the next will be at 0 as well
            current.JoinFields.RemoveAt(0);
        }
    }
}