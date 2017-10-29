using DotNetSDB.output;
using MySql.Data.MySqlClient;

namespace DotNetSDB
{
    public partial class MySQLCore : SQLBase
    {
        /*##########################################*/
        /*   OutputManagement Variable Definition   */
        /*##########################################*/

        /// <summary>
        /// Stores the output logger details
        /// </summary>
        protected OutputManagementSettings loggerDetails;

        /*##########################################*/
        /*     Sanitisation Variable Definitions    */
        /*##########################################*/

        /// <summary>
        /// Stores the default exist binding name to use
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
        /// Stores the connection information variables
        /// </summary>
        protected string user, pwd, server, db, connection, connectionStringExtra, integratedSecurity;

        /// <summary>
        /// Stores the flag on if the connection is using windows auth
        /// </summary>
        protected bool isWindowsAuth;

        /// <summary>
        /// Stores the port to run the connection on
        /// </summary>
        protected int port;

        /// <summary>
        /// Stores the connection timeout value
        /// </summary>
        protected int connectionTime;

        /// <summary>
        /// Stores the MySQL connection object
        /// </summary>
        protected MySqlConnection myConnection;

        /// <summary>
        /// Stores the max deadlock retry a query or stored procedure will do
        /// </summary>
        protected int maxDeadlockTry = 5;
    }
}