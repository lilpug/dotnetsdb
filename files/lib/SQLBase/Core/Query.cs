using System;
using System.Collections.Generic;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
		//This class is the query class which holds all the variables for all the various different sql parameters and functions
        [Serializable]
        protected class Query : IDisposable
        {
            //These variables store the fields for the functions *there is only one function created from these per query*
            public bool isDistinct = false;

            public string selectTable = "";
            public List<string> selectFields = new List<string>();
            public List<string> orderbyFields = new List<string>();
            public List<string> groupbyFields = new List<string>();

            //Variables used per query i.e. once an insert is run a new query object is created
            public string dropTableName = "";

            public string insertTableName = "";
            public List<string> insertFields = new List<string>();
            public List<string> insertValues = new List<string>();
            public List<string> createFields = new List<string>();
            public string createTable = "";

            public List<string> updateFields = new List<string>();
            public string updateTable = "";

            public string deleteTable = "";

            public List<string> joinFields = new List<string>();
            public List<string> whereStatements = new List<string>();
            public List<string> whereStatementsTypes = new List<string>();

            //These variables hold the real values
            public List<object[]> whereRealValues = new List<object[]>();

            public List<object[]> updateRealValues = new List<object[]>();
            public List<object[]> insertRealValues = new List<object[]>();

            public List<string> orderList = new List<string>();

            public List<string> pureSql = new List<string>();
            public string sqlStartWrapper = "";
            public string sqlEndWrapper = "";

            //This holds the real custom values
            public List<object[]> customRealValues = new List<object[]>();

            protected virtual void Dispose(bool disposing)
            {
                if (disposing)
                {
                    isDistinct = false;
                    selectTable = "";
                    selectFields.Clear();
                    orderbyFields.Clear();
                    groupbyFields.Clear();
                    dropTableName = "";
                    insertTableName = "";
                    insertFields.Clear();
                    insertValues.Clear();
                    createFields.Clear();
                    createTable = "";
                    updateFields.Clear();
                    updateTable = "";
                    deleteTable = "";
                    joinFields.Clear();
                    whereStatements.Clear();
                    whereStatementsTypes.Clear();
                    whereRealValues.Clear();
                    updateRealValues.Clear();
                    insertRealValues.Clear();
                    orderList.Clear();
                    pureSql.Clear();
                    sqlStartWrapper = "";
                    sqlEndWrapper = "";
                    customRealValues.Clear();
                }
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }		
		
        //This function is run every time a Top layer function is run to get the query object
        protected Query GetQuery()
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
            theQueries.Add(new Query());
        }
    }
}