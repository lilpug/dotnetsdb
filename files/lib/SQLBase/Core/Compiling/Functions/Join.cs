﻿namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*            Compiling Join functions      */
        /*##########################################*/

        protected virtual void CompileJoin(Query current)
        {
            compiledSql += string.Format(" {0}", current.joinFields[0]);
            current.joinFields.RemoveAt(0);
        }
    }
}