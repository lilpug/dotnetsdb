using DotNetSDB.output;
using System.Data.SqlClient;

namespace DotNetSDB
{
    public partial class SqlServerCore : SQLBase
    {
        /*##########################################*/
        /*   OutputManagement Variable Definition   */
        /*##########################################*/

        /// <summary>
        /// This variable stores the output logger details
        /// </summary>
        protected OutputManagementSettings loggerDetails;

        /*##########################################*/
        /*     Sanitisation Variable Definitions    */
        /*##########################################*/

        /// <summary>
        /// This variable stores the default exist binding name to use
        /// </summary>
        protected const string existDefinition = "@exist_name";

        /// <summary>
        /// Stores the default field binding name to use
        /// </summary>
        protected const string fieldsDefinition = "@get_fields_name";

        /*##########################################*/
        /*      Database Connection Variables       */
        /*##########################################*/

        /// <summary>
        /// This variable stores the connection information variables
        /// </summary>
        protected string user, pwd, server, db, connection, connectionStringExtra, integratedSecurity;

        /// <summary>
        /// This variable stores the flag on if the connection is using windows auth
        /// </summary>
        protected bool isWindowsAuth;

        /// <summary>
        /// This variable stores the port to run the connection on
        /// </summary>
        protected int port;

        /// <summary>
        /// This variable stores the connection timeout value
        /// </summary>
        protected int connectionTime;

        /// <summary>
        /// This variable stores the MySQL connection object
        /// </summary>
        protected SqlConnection myConnection;

        /// <summary>
        /// This variable stores the max deadlock retry a query or stored procedure will do
        /// </summary>
        protected int maxDeadlockTry = 5;
        
    }
}