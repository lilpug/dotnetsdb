using System;
namespace DotNetSDB
{
    /// <summary>
    /// Converts a base data type to a database data type
    /// </summary>
    public partial class DatabaseTypeConvertor : IDisposable
    {
        /*##########################################*/
        /*              Dispose functions           */
        /*##########################################*/

        /// <summary>
        /// Core variable for determining if the object has already been disposed of
        /// </summary>
        protected bool IsDisposed { get; set; }

        /// <summary>
        /// This is the core dispose method for the SQL Server type convertor object
        /// </summary>
        public virtual void Dispose()
        {
            if (!IsDisposed)
            {   
                TypeList.Clear();
                TypeList = null;

                IsDisposed = true;
            }
            GC.SuppressFinalize(this);
        }
    }
}