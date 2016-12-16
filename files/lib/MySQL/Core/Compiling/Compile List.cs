namespace DotNetSDB
{
    public partial class MysqlCore
    {
        //Hooks into the extra compiling function so we can run the extra compiling features for query2
        protected override void ExtraCompileList(query current, string key)
        {
            //Runs the base first then executes the extras
            base.ExtraCompileList(current, key);

            //Gets the index of the current query we are on
            int index = theQueries.IndexOf(current);

            if (key == "offset")
            {
                CompileLimit(theQueries2[index]);
            }
        }
    }
}