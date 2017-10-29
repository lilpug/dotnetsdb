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
        public virtual int get_parameter_binding_count()
        {
            int mainCounter = 0;
            for (int i = 0; i < theQueries.Count; i++)
            {
                mainCounter += theQueries[i].InsertRealValues.Count;
                mainCounter += theQueries[i].UpdateRealValues.Count;
                mainCounter += theQueries[i].WhereRealValues.Count;
                mainCounter += theQueries[i].CustomRealValues.Count;
            }
            return mainCounter;
        }
    }
}