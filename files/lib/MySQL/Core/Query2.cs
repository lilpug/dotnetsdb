using System;

namespace DotNetSDB
{
    public partial class MySQLCore
    {
        /// <summary>
        /// This holds the extra query variables for this specific database version
        /// </summary>
        [Serializable]
        protected class QueryExtension : Query
        {
            /// <summary>
            /// Variable that stores the built limit
            /// </summary>
            public string Limit { get;set; }

            /// <summary>
            /// Variable that holds the exist table values
            /// </summary>
            public object[] ExistRealTableValue { get; set; }

            /// <summary>
            /// Variable that holds the get fields values
            /// </summary>
            public object[] GetFieldsRealTableValue { get; set; }

            /// <summary>
            /// This is the main constructor that loads up the extended query variables
            /// </summary>
            public QueryExtension() : base()
            {
            }
            
            /// <summary>
            /// This is the core disposal method for the Query Extension object
            /// </summary>
            public override void Dispose()
            {
                //Checks if the disposal method has already run
                if (!IsDisposed)
                {
                    Limit = null;

                    ExistRealTableValue = null;
                    GetFieldsRealTableValue = null;
                }

                //Calls the parents disposal method 
                base.Dispose();
            }
        }

        /// <summary>
        /// This function is used to override the default object creation creation to ensure its what we require
        /// </summary>
        public override void start_new_query()
        {
            theQueries.Add(new QueryExtension());
        }
    }
}