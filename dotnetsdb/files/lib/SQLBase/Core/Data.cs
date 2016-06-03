using System;
using System.Collections.Generic;

namespace DotNetSDB
{
    public abstract partial class SQLBase : IDisposable
    {
        /*##########################################*/
        /*          Adding raw data functions       */
        /*##########################################*/

        //This function looks at the data and if a cast exception is thrown it then uses a different approach to be able to type cast it correctly
        //Note: This is done as string[] will type cast normally but elements like byte[] will not the index's do not match
        protected virtual object[] add_data(object data)
        {
            object[] results;

            try
            {
                //This is needed as otherwise it will not add object[] with 1 element of null it will simply make object[] null
                if (data == null)
                {
                    results = new object[] { null };
                }
                //Most arrays will cast to object[] perfectly i.e. a string[2] will be object[2] but not byte[] as they should be singular 1 per item
                //REASON: its possible that a string[2] is two values for that particular part of the query were as byte[] is 1 object only
                else if(data.GetType().IsArray && data.GetType() != typeof(byte[]))
                {
                    results = (object[])data;                    
                }
                else//Everything else becomes singular inserted
                {
                    List<object> store = new List<object>();
                    store.Add(data);
                    results = store.ToArray();                    
                }
            }
            catch (InvalidCastException)
            {
                //If cast has failed insert it singular
                List<object> store = new List<object>();
                store.Add(data);
                results = store.ToArray();
            }

            return results;
        }
    }
}