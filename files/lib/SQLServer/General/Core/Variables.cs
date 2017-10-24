using DotNetSDB.output;
using System.Data.SqlClient;

namespace DotNetSDB
{
    public partial class SqlServerCore : SQLBase
    {
        /*##########################################*/
        /*   OutputManagement Variable Definition   */
        /*##########################################*/

        protected OutputManagementSettings loggerDetails;

        /*##########################################*/
        /*     Sanitisation Variable Definitions    */
        /*##########################################*/

        protected const string existDefinition = "@exist_name";
        protected const string fieldsDefinition = "@get_fields_name";

        /*##########################################*/
        /*      Database Connection Variables       */
        /*##########################################*/

        //Class variables for the database connections
        protected string user, pwd, server, db, connection, connectionStringExtra, integratedSecurity;

        protected bool isWindowsAuth;
        protected int port;
        protected int connectionTime;
        protected SqlConnection myConnection;

        //The maximum attempts to query on a deadlock detection *1 second intervalts per try*
        protected int maxDeadlockTry = 5;
        
    }
}