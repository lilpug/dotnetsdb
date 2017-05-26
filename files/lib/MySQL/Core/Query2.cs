using System;
using System.Collections.Generic;

namespace DotNetSDB
{
    public partial class MySQLCore
    {
        //NOTE: even though we have another query object for the new features in this specific database version,
        //      we STILL use the normal query objects order list for the new commands!!

        //This holds the extra query variables for this specific database version
        [Serializable]
        protected class Query2
        {
            //Variables for the offset
            public string limit = "";

            public object[] exist_real_table_value = new object[0];
            public object[] get_fields_real_table_value = new object[0];            

            public void Dispose(bool disposing)
            {
                if (disposing)
                {
                    limit = "";

                    exist_real_table_value = null;
                    get_fields_real_table_value = null;
                }
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        //This variable holds all the query objects for this specific database version
        private List<Query2> theQueries2 = new List<Query2>();

        //This function is run every time a Top layer function is run to get the query object
        protected Query2 GetQuery2()
        {
            //This runs in case its the first initiation, if so it creates the new query object before getting it
            if (theQueries2.Count == 0)
            {
                start_new_query();
            }

            //Returns the latest query element
            return theQueries2[theQueries2.Count - 1];
        }

        //This function is used to create a new query object for running a new query
        public override void start_new_query()
        {
            //Runs the base first then executes the extras
            base.start_new_query();

            theQueries2.Add(new Query2());
        }
    }
}