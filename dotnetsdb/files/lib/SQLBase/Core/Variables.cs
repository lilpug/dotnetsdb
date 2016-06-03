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
        protected List<query> theQueries = new List<query>();

        //This variable is used to compile the main sql query that will be executed
        protected string compiled_build = "";

        /*##########################################*/
        /*     Sanitisation Variable Definitions    */
        /*##########################################*/

        protected const string update_definition = "@update";
        protected const string insert_definition = "@insert";
        protected const string where_definition = "@where";
        protected const string custom_definition = "@custom";        
    }
}