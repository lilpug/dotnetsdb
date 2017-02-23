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
                mainCounter += theQueries[i].insertRealValues.Count;
                mainCounter += theQueries[i].updateRealValues.Count;
                mainCounter += theQueries[i].whereRealValues.Count;
                mainCounter += theQueries[i].customRealValues.Count;
            }
            return mainCounter;
        }
    }
}