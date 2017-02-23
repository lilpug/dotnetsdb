using System.Collections.Generic;

namespace DotNetSDB
{
    public partial class SqlServer2016 : SqlServerCore
    {
        //Extends the backup facility on runing a debug compile to the new functionality of query3
        private List<Query3> theBackup3 = new List<Query3>();

        protected override void CompileBackup()
        {
            base.CompileBackup();
            theBackup3.Clear();
            for (int i = 0; i < theQueries3.Count; i++)
            {
                theBackup3.Add(DeepClone(theQueries3[i]));
            }
        }

        protected override void CompileRestore()
        {
            base.CompileRestore();
            theQueries3.Clear();
            for (int i = 0; i < theBackup3.Count; i++)
            {
                theQueries3.Add(DeepClone(theBackup3[i]));
            }
            theBackup3.Clear();
        }
    }
}