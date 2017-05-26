namespace DotNetSDB
{
    public partial class SQLServer2012 : SqlServerCore
    {
        /*##########################################*/
        /*      Main Compiling List functions       */
        /*##########################################*/

        protected override void ExtraCompileList(Query current, string key)
        {
            //Runs the base first then executes the extras
            base.ExtraCompileList(current, key);

            //Gets the index of the current query we are on
            int index = theQueries.IndexOf(current);

            if (key == "offset")
            {
                CompileOffset(theQueries3[index]);
            }
        }
    }
}