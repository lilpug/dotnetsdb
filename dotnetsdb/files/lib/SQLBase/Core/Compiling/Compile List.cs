namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*      Main Compiling List functions       */
        /*##########################################*/

        protected virtual void CompileKeyList(query current)
        {
            foreach (string key in current.orderList)
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

        //This is an empty method which allows inheritance to hook onto something within the list for extra functions
        protected virtual void ExtraCompileList(query current, string key)
        {
        }
    }
}