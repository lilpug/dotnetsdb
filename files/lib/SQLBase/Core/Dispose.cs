using System;
using System.Collections.Generic;

namespace DotNetSDB
{
    public abstract partial class SQLBase : IDisposable
    {
        /*##########################################*/
        /*          Database Cleanup Variable       */
        /*##########################################*/

        protected bool _disposed = false;

        //This is the dispose method for disposing of the connection
        //Note: called at the end of a using statement
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        //This disposes of the connection
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    disposeAll();
                }
                _disposed = true;
            }
        }

        //This disposes all the query objects once finished
        protected void disposeAll()
        {
            foreach (query q in theQueries)
            {
                q.Dispose();                
            }

            theQueries.Clear();

            ExtraDispose();
        }

        //This function is used as an extra hook for inheritance
        protected virtual void ExtraDispose()
        {
        }
    }
}