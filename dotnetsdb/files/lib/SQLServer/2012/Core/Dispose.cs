using System;
using System.Collections.Generic;

namespace DotNetSDB
{
    public partial class SqlServer2012
    {
        //This function is used to clear and dispose the query2 objects
        //Note: This function hooks into the disposeAll feature which is run on cleanup
        protected override void ExtraDispose()
        {
            //Runs the base first then executes the extras
            base.ExtraDispose();

            foreach (query3 q in theQueries3)
            {
                q.Dispose();
            }
            theQueries3.Clear();
        }
    }
}