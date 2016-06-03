using System;
using System.Collections.Generic;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
		//This class is the query class which holds all the variables for all the various different sql parameters and functions
        [SerializableAttribute]
        protected class query : IDisposable
        {
            //These variables store the fields for the functions *there is only one function created from these per query*
            public bool is_dinstinct = false;

            public string select_table = "";
            public List<string> select_fields = new List<string>();
            public List<string> orderby_fields = new List<string>();
            public List<string> groupby_fields = new List<string>();

            //Variables used per query i.e. once an insert is run a new query object is created
            public string drop_table_name = "";

            public string insert_table_name = "";
            public List<string> insert_fields = new List<string>();
            public List<string> insert_values = new List<string>();
            public List<string> create_fields = new List<string>();
            public string create_table = "";

            public List<string> update_fields = new List<string>();
            public string update_table = "";

            public string delete_table = "";

            public List<string> join_fields = new List<string>();
            public List<string> where_statements = new List<string>();
            public List<string> where_statement_types = new List<string>();

            //These variables hold the real values
            public List<object[]> where_real_values = new List<object[]>();

            public List<object[]> update_real_values = new List<object[]>();
            public List<object[]> insert_real_values = new List<object[]>();

            public List<string> orderList = new List<string>();

            public List<string> pure_sql = new List<string>();
            public string sql_start_wrapper = "";
            public string sql_end_wrapper = "";

            //This holds the real custom values
            public List<object[]> custom_real_values = new List<object[]>();

            protected virtual void Dispose(bool disposing)
            {
                if (disposing)
                {
                    is_dinstinct = false;
                    select_table = "";
                    select_fields.Clear();
                    orderby_fields.Clear();
                    groupby_fields.Clear();
                    drop_table_name = "";
                    insert_table_name = "";
                    insert_fields.Clear();
                    insert_values.Clear();
                    create_fields.Clear();
                    create_table = "";
                    update_fields.Clear();
                    update_table = "";
                    delete_table = "";
                    join_fields.Clear();
                    where_statements.Clear();
                    where_statement_types.Clear();
                    where_real_values.Clear();
                    update_real_values.Clear();
                    insert_real_values.Clear();
                    orderList.Clear();
                    pure_sql.Clear();
                    sql_start_wrapper = "";
                    sql_end_wrapper = "";
                    custom_real_values.Clear();
                }
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }		
		
        //This function is run every time a Top layer function is run to get the query object
        protected query get_query()
        {
            //This runs in case its the first initiation, if so it creates the new query object before getting it
            if (theQueries.Count == 0)
            {
                start_new_query();
            }

            //Returns the latest query element
            return theQueries[theQueries.Count - 1];
        }

        //This function is used to create a new query object for running a new query
        public virtual void start_new_query()
        {
            theQueries.Add(new query());
        }
    }
}