using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*         Main Compiling functions         */
        /*##########################################*/

        //This section is used to make a full deep backup of the query object if we are running compile only for debugging and not execution
        //Note: This means it backs up the query and then returns it to its original state before the compiling started
        private List<Query> theBackup = new List<Query>();

        protected virtual void CompileBackup()
        {
            theBackup.Clear();
            for (int i = 0; i < theQueries.Count; i++)
            {
                theBackup.Add(DeepClone(theQueries[i]));
            }
        }

        protected virtual void CompileRestore()
        {
            theQueries.Clear();
            for (int i = 0; i < theBackup.Count; i++)
            {
                theQueries.Add(DeepClone(theBackup[i]));
            }
            theBackup.Clear();
        }

        protected static T DeepClone<T>(T obj)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// This function is the main compiling function which deals with putting all the queries together into one massive statement to be executed.
        /// </summary>
        protected void compiling(bool backup = false)
        {
            if (backup)
            {
                CompileBackup();
            }

            compiledSql = " ";

            foreach (Query current in theQueries)
            {
                //Deals with adding the start wrapper around the query
                CompileStartWrapper(current);

                //Deals with adding all the query objects list items
                CompileKeyList(current);

                //Deals with adding the end wrapper around the query
                CompileEndWrapper(current);

                //Ends the query
                compiledSql += "; ";
            }

            compiledSql = compiledSql.Replace("  ", " "); //Removes any duplicate spaces with a space in the compile building
            compiledSql = compiledSql.Trim(); //Removes the whitespace at the start and end of the compile building

            if (backup)
            {
                CompileRestore();
            }
        }
    }
}