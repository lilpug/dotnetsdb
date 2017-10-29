using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetSDB
{
    public abstract partial class SQLBase : IDisposable
    {
        /*##########################################*/
        /*        Compiling Build Variables         */
        /*##########################################*/

        /// <summary>
        /// This variable holds all the query objects which will be processed
        /// </summary>
        protected List<Query> theQueries = new List<Query>();

        /// <summary>
        /// This variable is used to compile the main sql query that will be executed
        /// </summary>
        protected StringBuilder compiledSql = new StringBuilder();

        /*##########################################*/
        /*     Sanitisation Variable Definitions    */
        /*##########################################*/

        /// <summary>
        /// This variable stores the default update binding name to use
        /// </summary>
        protected const string updateDefinition = "@update";

        /// <summary>
        /// This variable stores the default insert binding name to use
        /// </summary>
        protected const string insertDefinition = "@insert";

        /// <summary>
        /// This variable stores the default where binding name to use
        /// </summary>
        protected const string whereDefinition = "@where";

        /// <summary>
        /// This variable stores the default custom binding name to use
        /// </summary>
        protected const string customDefinition = "@custom";        
    }
}