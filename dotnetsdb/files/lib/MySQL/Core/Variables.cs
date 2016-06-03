using DotNetSDB.output;
using MySql.Data.MySqlClient;

namespace DotNetSDB
{
    public partial class MysqlCore : SQLBase
    {
        /*##########################################*/
        /*   OutputManagement Variable Definition   */
        /*##########################################*/

        protected OutputManagementVariable loggerDetails;

        /*##########################################*/
        /*     Sanitisation Variable Definitions    */
        /*##########################################*/

        protected const string exist_definition = "@exist_name";
        protected const string fields_definition = "@get_fields_name";

        /*##########################################*/
        /*      Database Connection Variables       */
        /*##########################################*/

        //Class variables for the database connections
        protected string user, pwd, server, db, connection;

        protected int connectionTime;
        protected MySqlConnection myConnection;

        //The maximum attempts to query on a deadlock detection *1 second intervalts per try*
        protected int maxDeadlockTry = 5;
    }
}