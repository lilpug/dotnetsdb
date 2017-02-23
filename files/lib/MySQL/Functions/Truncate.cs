﻿using System;

namespace DotNetSDB
{
    public partial class MySLQCore
    {
        /*##########################################*/
        /*           Truncate SQL function          */
        /*##########################################*/

        //Note: This parameter can not be sanatized as its not in the where clause etc
        /// <summary>
        /// This function truncates the table
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool truncate_table(string tableName)
        {
            //Checks if the value passed is null
            if (!string.IsNullOrWhiteSpace(tableName))
            {
                //Gives the sql string to the compiled_build
                compiledSql = string.Format("Truncate Table {0}", tableName);

                run();
                return true;
            }

            return false;
        }
    }
}