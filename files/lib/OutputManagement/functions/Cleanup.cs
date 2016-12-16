using System;
using System.IO;

namespace DotNetSDB.output
{
    public partial class OutputManagement
    {
        public bool cleanupLogs()
        {
            try
            {
                //Actives the cleaning process
                if (info.cleanUpDays > 0)
                {
                    object locker = new object();
                    lock (locker)
                    {
                        //Gets the start and end days
                        DateTime start = GetDateNow().AddDays(~info.cleanUpDays).Date;
                        DateTime end = GetDateNow().Date;

                        //Gets all the current log files in the directory
                        string[] files = Directory.GetFiles(info.directory, "*.log");
                        foreach (string file in files)
                        {
                            //Sorts the filename out
                            string name = Path.GetFileNameWithoutExtension(file).Split('.')[1].Replace('-', '/');
                            DateTime dt = Convert.ToDateTime(name).Date;
                            if (dt < start || dt > end)
                            {
                                File.Delete(file);
                            }
                        }
                    }
                }

                return true;
            }
            catch { }
            return false;
        }
    }
}