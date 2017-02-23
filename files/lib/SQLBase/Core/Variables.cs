using System;
using System.Collections.Generic;

namespace DotNetSDB
{
    public abstract partial class SQLBase : IDisposable
    {
        /*##########################################*/
        /*        Compiling Build Variables         */
        /*##########################################*/        

        //This variable holds all the query objects which will be processed
        protected List<Query> theQueries = new List<Query>();

        //This variable is used to compile the main sql query that will be executed
        protected string compiledSql = "";

        /*##########################################*/
        /*     Sanitisation Variable Definitions    */
        /*##########################################*/

        protected const string updateDefinition = "@update";
        protected const string insertDefinition = "@insert";
        protected const string whereDefinition = "@where";
        protected const string customDefinition = "@custom";        
    }
}