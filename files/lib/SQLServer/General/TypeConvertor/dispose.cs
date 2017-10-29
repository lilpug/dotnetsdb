using System;
namespace DotNetSDB
{
    /// <summary>
    /// Convert a base data type to another base data type
    /// </summary>
    public partial class SqlServerTypeConvertor : IDisposable
    {
        /*##########################################*/
        /*              Dispose functions           */
        /*##########################################*/

        /// <summary>
        /// Core variable for determining if the object has already been disposed of
        /// </summary>
        protected bool isDisposed = false;

        /// <summary>
        /// This is the core dispose method for the SQL Server type convertor object
        /// </summary>
        public virtual void Dispose()
        {
            if (!isDisposed)
            {   
                TypeList.Clear();
                TypeList = null;

                isDisposed = true;
            }
            GC.SuppressFinalize(this);
        }
    }
}