using System;

namespace DotNetSDB
{
    public partial class SQLServer2008
    {
        /// <summary>
        /// This holds the extra query variables for this specific database version
        /// </summary>
        [Serializable]
        protected class QueryExtension2 : QueryExtension
        {
            /// <summary>
            /// Variable that holds the limit min value
            /// </summary>
            public int LimitCountOne { get; set; }

            /// <summary>
            /// Variable that holds the limit max value
            /// </summary>
            public int LimitCountTwo { get; set; }

            /// <summary>
            /// Variable that holds the generated orderby for the limit wrapper
            /// </summary>
            public string Orderby { get; set; }

            /// <summary>
            /// This is the main constructor that loads up the extended query variables
            /// </summary>
            public QueryExtension2() : base()
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
                    LimitCountOne = -1;
                    LimitCountTwo = -1;
                    Orderby = null;
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
            theQueries.Add(new QueryExtension2());
        }
    }
}