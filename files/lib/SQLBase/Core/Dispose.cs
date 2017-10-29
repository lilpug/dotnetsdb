using System;

namespace DotNetSDB
{
    public abstract partial class SQLBase : IDisposable
    {
        /*##########################################*/
        /*          Database Cleanup Variable       */
        /*##########################################*/
        /// <summary>
        /// Core variable for determining if the object has already been disposed of
        /// </summary>
        protected bool IsDisposed { get; set; }

        /// <summary>
        /// This is the dispose method for disposing of the connection
        /// </summary>
        public virtual void Dispose()
        {
            if (!IsDisposed)
            {   
                DisposeAll();
                IsDisposed = true;
            }
            GC.SuppressFinalize(this);
        }
        
        /// <summary>
        /// This disposes all the query objects once finished
        /// </summary>
        protected void DisposeAll()
        {
            foreach (Query q in theQueries)
            {
                q.Dispose();                
            }

            theQueries.Clear();

            ExtraDispose();
        }

        /// <summary>
        /// This function is used as an extra hook for inheritance
        /// </summary>
        protected virtual void ExtraDispose()
        {
        }
    }
}