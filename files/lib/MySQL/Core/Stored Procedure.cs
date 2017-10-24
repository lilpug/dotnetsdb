using System.Collections.Generic;

namespace DotNetSDB
{
    public partial class MySQLCore
    {
        /// <summary>
        /// This object stores the internal stored procedure data
        /// </summary>
        protected class StoredProcedure
        {
            /// <summary>
            /// Holds the stored procedures name
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Holds the stored procedures parameters that are going to be supplied
            /// </summary>
            public Dictionary<string, object> Parameters { get;set; }
        }

        /// <summary>
        /// This variable stores the information for the stored procedure that is to be run on an execution method
        /// </summary>
        protected StoredProcedure Procedure { get; set; }
    }
}