using System;
using System.Collections.Generic;

namespace DotNetSDB
{
    public partial class MysqlCore
    {
        //This is the dispose method for disposing of the connection
        //Note: called at the end of a using statement
        public override void Dispose()
        {
            base.Dispose();

            //Clears all the connection information
            myConnection.Dispose();
            user = "";
            pwd = "";
            server = "";
            db = "";
            connection = "";
            loggerDetails = null;
            maxDeadlockTry = -1;
            connectionTime = -1;
        }

        //This function is used to clear and dispose the query2 objects
        //Note: This function hooks into the disposeAll feature which is run on cleanup
        protected override void ExtraDispose()
        {
            //Runs the base first then executes the extras
            base.ExtraDispose();

            foreach (query2 q in theQueries2)
            {
                q.Dispose();
            }
            theQueries2.Clear();            
        }
    }
}