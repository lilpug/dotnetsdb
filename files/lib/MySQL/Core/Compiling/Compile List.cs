namespace DotNetSDB
{
    /// <summary>
    /// This is the Core MySQL class
    /// </summary>
    public partial class MySQLCore
    {
        /// <summary>
        /// This function hooks into the extra compiling function so we can run the extra compiling features for query extensions
        /// </summary>
        /// <param name="current"></param>
        /// <param name="key"></param>
        protected override void ExtraCompileList(Query current, string key)
        {
            //Runs the base first then executes the extras
            base.ExtraCompileList(current, key);
            
            if (key == "limit")
            {
                CompileLimit(current);
            }
        }
    }
}