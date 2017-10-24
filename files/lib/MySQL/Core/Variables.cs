using DotNetSDB.output;
using MySql.Data.MySqlClient;

namespace DotNetSDB
{
    public partial class MySQLCore : SQLBase
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

        //Class variables for the database connection strings
        protected string user, pwd, server, db, connection, connectionStringExtra, integratedSecurity;

        protected bool isWindowsAuth;
        protected int port;
        protected int connectionTime;
        protected MySqlConnection myConnection;

        //The maximum attempts to query on a deadlock detection *1 second intervalts per try*
        protected int maxDeadlockTry = 5;
    }
}