using System;
using System.IO;

namespace DotNetSDB.output
{
    public partial class OutputManagement
    {
        /// <summary>
        /// This function cleans up the logs if the cleanUpDays variable is not 0 and the logs are older than the supplied days
        /// </summary>
        /// <returns></returns>
        public bool CleanupLogs()
        {
            try
            {
                //Actives the cleaning process if the number of days is not zero "zero means no cleanup"
                if (info.CleanUpDays > 0)
                {
                    object locker = new object();
                    lock (locker)
                    {
                        //Calculates the start and end dates for the cleaning up process
                        DateTime start = Now.AddDays(~info.CleanUpDays).Date;
                        DateTime end = Now.Date;

                        //Gets all the current log files in the directory
                        string[] files = Directory.GetFiles(info.Directory, "*.log");
                        foreach (string file in files)
                        {
                            //Sorts the filename out
                            string name = Path.GetFileNameWithoutExtension(file).Split('.')[1].Replace('-', '/');

                            //Checks if the log file is outside the start and end dates
                            DateTime dt = Convert.ToDateTime(name).Date;
                            if (dt < start || dt > end)
                            {
                                //Removes it if so
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