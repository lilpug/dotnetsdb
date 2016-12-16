using System;
using System.Collections.Generic;

namespace DotNetSDB
{
    public partial class SqlServer2012
    {
        //NOTE: even though we have another query object for the new features in this specific database version,
        //      we STILL use the normal query objects order list for the new commands!!

        //This holds the extra query variables for this specific database version
        [SerializableAttribute]
        protected class query3
        {
            //Variables for the offset
            public string offset = "";

            public void Dispose(bool disposing)
            {
                if (disposing)
                {
                    offset = "";
                }
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        //This variable holds all the query objects for this specific database version
        private List<query3> theQueries3 = new List<query3>();

        //This function is run every time a Top layer function is run to get the query object
        protected query3 get_query3()
        {
            //This runs in case its the first initiation, if so it creates the new query object before getting it
            if (theQueries3.Count == 0)
            {
                start_new_query();
            }

            //Returns the latest query element
            return theQueries3[theQueries3.Count - 1];
        }

        //This function is used to create a new query object for running a new query
        public override void start_new_query()
        {
            //Runs the base first then executes the extras
            base.start_new_query();

            theQueries3.Add(new query3());
        }
    }
}