using System;
using System.Collections.Generic;

namespace DotNetSDB
{
    public partial class MysqlCore
    {
        //NOTE: even though we have another query object for the new features in this specific database version,
        //      we STILL use the normal query objects order list for the new commands!!

        //This holds the extra query variables for this specific database version
        [SerializableAttribute]
        protected class query2
        {
            //Variables for the offset
            public string limit = "";

            public string exist_real_table_value = "";
            public string get_fields_real_table_value = "";

            public void Dispose(bool disposing)
            {
                if (disposing)
                {
                    limit = "";

                    exist_real_table_value = "";
                    get_fields_real_table_value = "";
                }
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        //This variable holds all the query objects for this specific database version
        private List<query2> theQueries2 = new List<query2>();

        //This function is run every time a Top layer function is run to get the query object
        protected query2 get_query2()
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

            theQueries2.Add(new query2());
        }
    }
}