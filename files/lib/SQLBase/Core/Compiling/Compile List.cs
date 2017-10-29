namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*      Main Compiling List functions       */
        /*##########################################*/

        /// <summary>
        /// This function runs through all the order list options and compiles the right query sections in the supplied order
        /// </summary>
        /// <param name="current"></param>
        protected virtual void CompileKeyList(Query current)
        {
            foreach (string key in current.OrderList)
            {
                if (key == "select")
                {
                    CompileSelect(current);
                }
                else if (key == "join")
                {
                    CompileJoin(current);
                }
                else if (key == "delete")
                {
                    CompileDelete(current);
                }
                else if (key == "drop")
                {
                    CompileDrop(current);
                }
                else if (key == "where")
                {
                    CompileWhere(current);
                }
                else if (key == "orderby")
                {
                    CompileOrderBy(current);
                }
                else if (key == "groupby")
                {
                    CompileGroup(current);
                }
                else if (key == "create")
                {
                    CompileCreate(current);
                }
                else if (key == "insert")
                {
                    CompileInsert(current);
                }
                else if (key == "update")
                {
                    CompileUpdate(current);
                }
                else if (key == "pure_sql")
                {
                    CompilePureSQL(current);
                }

                //This is used for any extra keys
                ExtraCompileList(current, key);
            }
        }

        /// <summary>
        /// This is an empty method which allows inheritance to hook onto something within the list for extra functions
        /// </summary>
        /// <param name="current"></param>
        /// <param name="key"></param>
        protected virtual void ExtraCompileList(Query current, string key)
        {
        }
    }
}