namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*           Main Front function            */
        /*##########################################*/

        /// <summary>
        /// This function returns the current amount of parameter binding occurrences
        /// </summary>
        /// <returns></returns>
        public virtual int get_bind_count()
        {
            int mainCounter = 0;
            for (int i = 0; i < theQueries.Count; i++)
            {
                mainCounter += theQueries[i].insert_real_values.Count;
                mainCounter += theQueries[i].update_real_values.Count;
                mainCounter += theQueries[i].where_real_values.Count;
                mainCounter += theQueries[i].custom_real_values.Count;
            }
            return mainCounter;
        }
    }
}