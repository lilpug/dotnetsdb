using System;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*          Compiling Create functions      */
        /*##########################################*/

        /// <summary>
        /// This function compiles the create query and adds it to the query being built
        /// </summary>
        /// <param name="current"></param>
        protected virtual void CompileCreate(Query current)
        {
            compiledSql.Append($" CREATE TABLE {current.CreateTable} ({string.Join(",", current.CreateFields).TrimEnd(',')})");
        }
    }
}