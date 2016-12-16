using System;
namespace DotNetSDB
{
    /// <summary>
    /// Convert a base data type to another base data type
    /// </summary>
    public partial class MySqlTypeConvertor : IDisposable
    {
        /*##########################################*/
        /*              Dispose functions           */
        /*##########################################*/

        //This is used to ensure the clean up does not fire multiple times once its all cleared up
        protected bool isDisposed = false;

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        //This disposes of the types
        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    typeList.Clear();
                    typeList = null;
                }
                isDisposed = true;
            }
        }
    }
}