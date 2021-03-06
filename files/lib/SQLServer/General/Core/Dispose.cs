﻿namespace DotNetSDB
{
    public partial class SqlServerCore
    {

        /// <summary>
        /// This is the dispose method for disposing of the connection
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();

            //Clears all the connection information
            myConnection.Dispose();
            user = "";
            pwd = "";
            server = "";
            db = "";
            connection = "";
            loggerDetails = null;
            maxDeadlockTry = -1;
            connectionTime = -1;
        }
    }
}