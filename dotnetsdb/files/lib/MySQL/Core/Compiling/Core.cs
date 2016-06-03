using System.Collections.Generic;

namespace DotNetSDB
{
    public partial class MysqlCore
    {
        //Extends the backup facility on runing a debug compile to the new functionality of query2
        private List<query2> theBackup2 = new List<query2>();

        protected override void CompileBackup()
        {
            //Runs the base first then executes the extras
            base.CompileBackup();

            theBackup2.Clear();
            for (int i = 0; i < theQueries2.Count; i++)
            {
                theBackup2.Add(DeepClone(theQueries2[i]));
            }
        }

        protected override void CompileRestore()
        {
            //Runs the base first then executes the extras
            base.CompileRestore();

            theQueries2.Clear();
            for (int i = 0; i < theBackup2.Count; i++)
            {
                theQueries2.Add(DeepClone(theBackup2[i]));
            }
            theBackup2.Clear();
        }
    }
}