﻿namespace DotNetSDB
{
    public partial class SqlServer2016 : SqlServerCore
    {
        /*##########################################*/
        /*          Compiling Offset functions      */
        /*##########################################*/

        protected virtual void CompileOffset(query3 current)
        {
            compiled_build += " " + current.offset;
            current.offset = "";
        }
    }
}