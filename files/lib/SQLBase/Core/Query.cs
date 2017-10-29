using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
		/// <summary>
        /// This is the base query class that both MySQL and SQL Server stem from
        /// </summary>
        [Serializable]
        protected class Query : IDisposable
        {
            //These variables store the fields for the functions *there is only one function created from these per query*

            /// <summary>
            /// Variable that holds the flag for if the query should be distinct
            /// </summary>
            public bool IsDistinct { get; set; }

            /// <summary>
            /// Variable that stores that table name to select from
            /// </summary>
            public string SelectTable { get; set; }

            /// <summary>
            /// Variable that holds a list of select fields
            /// </summary>
            public List<string> SelectFields { get; set; }

            /// <summary>
            /// Variable that holds a list of the orderby fields
            /// </summary>
            public List<string> OrderbyFields { get; set; }

            /// <summary>
            /// Variable that holds a list of groupby fields
            /// </summary>
            public List<string> GroupbyFields { get; set; }

            //Variables used per query i.e. once an insert is run a new query object is created

            /// <summary>
            /// Variable used to store drop table name
            /// </summary>
            public string DropTableName { get; set; }

            /// <summary>
            /// Variable used to store the insert table name
            /// </summary>
            public string InsertTableName { get; set; }

            /// <summary>
            /// Variable used to store a list of insert fields
            /// </summary>
            public List<string> InsertFields { get; set; }

            /// <summary>
            /// Variable used to store a list of insert values
            /// </summary>
            public List<string> InsertValues { get; set; }

            /// <summary>
            /// Variable used to store a list of create fields
            /// </summary>
            public List<string> CreateFields { get; set; }

            /// <summary>
            /// Variable used to store the create table name
            /// </summary>
            public string CreateTable { get; set; }

            /// <summary>
            /// Variable used to store a list of update fields
            /// </summary>
            public List<string> UpdateFields { get; set; }

            /// <summary>
            /// Variable used to store the update table name
            /// </summary>
            public string UpdateTable { get; set; }

            /// <summary>
            /// Variable used to store the delete table name
            /// </summary>
            public string DeleteTable { get; set; }

            /// <summary>
            /// Variable used to store a list of join fields
            /// </summary>
            public List<StringBuilder> JoinFields { get; set; }

            /// <summary>
            /// Variable used to store a list of where statements
            /// </summary>
            public List<string> WhereStatements { get; set; }

            /// <summary>
            /// Variable used to store a list of where statement types
            /// </summary>
            public List<string> WhereStatementsTypes { get; set; }

            /// <summary>
            /// Variable used to store a list of the real where values
            /// </summary>
            public List<object[]> WhereRealValues { get; set; }

            /// <summary>
            /// Variable used to store a list of the real update values
            /// </summary>
            public List<object[]> UpdateRealValues { get; set; }

            /// <summary>
            /// Variable used to store a list of the real insert values
            /// </summary>
            public List<object[]> InsertRealValues { get; set; }

            /// <summary>
            /// Variable used to store the order the query should be compiled in
            /// </summary>
            public List<string> OrderList { get; set; }

            /// <summary>
            /// Variable used to store a list of pure SQL statements
            /// </summary>
            public List<string> PureSql { get; set; }

            /// <summary>
            /// Variable used to store a SQL Server query start wrapper
            /// </summary>
            public string SqlStartWrapper { get; set; }

            /// <summary>
            /// Variable used to store a SQL Server query end wrapper
            /// </summary>
            public string SqlEndWrapper { get; set; }

            /// <summary>
            /// Variable used to store a list of custom real values
            /// </summary>
            public List<object[]> CustomRealValues { get; set; }

            /// <summary>
            /// This is the main constructor that loads up the base query variables
            /// </summary>
            public Query()
            {
                SelectFields = new List<string>();
                OrderbyFields = new List<string>();
                GroupbyFields = new List<string>();
                InsertFields = new List<string>();
                InsertValues = new List<string>();
                CreateFields = new List<string>();
                UpdateFields = new List<string>();
                JoinFields = new List<StringBuilder>();
                WhereStatements = new List<string>();
                WhereStatementsTypes = new List<string>();
                WhereRealValues = new List<object[]>();
                UpdateRealValues = new List<object[]>();
                InsertRealValues = new List<object[]>();
                OrderList = new List<string>();
                PureSql = new List<string>();
                CustomRealValues = new List<object[]>();
            }

            /// <summary>
            /// Core variable for determining if the object has already been disposed of
            /// </summary>
            protected bool IsDisposed { get; set; }
            
            /// <summary>
            /// This is the core dispose method for the query object
            /// </summary>
            public virtual void Dispose()
            {
                //Checks if the disposal method has already run
                if (!IsDisposed)
                {
                    IsDistinct = false;
                    SelectTable = null;
                    SelectFields.Clear();
                    OrderbyFields.Clear();
                    GroupbyFields.Clear();
                    DropTableName = null;
                    InsertTableName = null;
                    InsertFields.Clear();
                    InsertValues.Clear();
                    CreateFields.Clear();
                    CreateTable = null;
                    UpdateFields.Clear();
                    UpdateTable = null;
                    DeleteTable = null;
                    JoinFields.Clear();
                    WhereStatements.Clear();
                    WhereStatementsTypes.Clear();
                    WhereRealValues.Clear();
                    UpdateRealValues.Clear();
                    InsertRealValues.Clear();
                    OrderList.Clear();
                    PureSql.Clear();
                    SqlStartWrapper = null;
                    SqlEndWrapper = null;
                    CustomRealValues.Clear();

                    //Sets the disposal flag to true
                    IsDisposed = true;
                }

                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// This function is run every time a Top layer function is run to get the query object
        /// </summary>
        /// <returns></returns>
        protected virtual Query GetQuery()
        {
            //This runs in case its the first initiation, if so it creates the new query object before getting it
            if (theQueries.Count == 0)
            {
                start_new_query();
            }

            //Returns the latest query element
            return theQueries[theQueries.Count - 1];
        }

        /// <summary>
        /// This function is used to create a new query object for running a new query
        /// </summary>
        public virtual void start_new_query()
        {
            theQueries.Add(new Query());
        }
    }
}