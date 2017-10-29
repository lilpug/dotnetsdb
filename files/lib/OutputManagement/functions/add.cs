using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace DotNetSDB.output
{
    public partial class OutputManagement
    {
        /// <summary>
        /// This function adds a supplied error message to a log
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public bool AddToLog(string errorMessage)
        {
            try
            {
                //Checks if cleanup is activated and does its job if so
                if (CleanupLogs())
                {   
                    //Gets the stack trace object ready for obtaining method names
                    StackTrace stackTrace = new StackTrace();

                    //Note: while getting the filename this creates the filename if it does not already exist ready for adding text
                    string fileName = GetFileName(Now);
                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        //Writes the single line log
                        if (info.SingleLineLog)
                        {
                            //Holds the created error output
                            StringBuilder sb = new StringBuilder();

                            //Creates the error message which will be put into the log file
                            sb.Append($"Error Occured: {Now} || ");

                            //Gets the two functions and their parameters chained down from hitting this function
                            sb.Append($"Function Trace: {stackTrace.GetFrame(2).GetMethod().Name} ({GetAllParameters(stackTrace.GetFrame(2).GetMethod().GetParameters())})");
                            sb.Append($"-> {stackTrace.GetFrame(1).GetMethod().Name} ({GetAllParameters(stackTrace.GetFrame(1).GetMethod().GetParameters())}) ||");
                            sb.Append($"Error Details: {errorMessage}");

                            //Locks the section to be thread safe as we are now processing a file
                            lock (locker)
                            {
                                using (TextWriter tw = new StreamWriter(Path.Combine(info.Directory, fileName), true))
                                {
                                    tw.WriteLine(sb.ToString());
                                }
                            }
                        }
                        else//Writes the multi-line log
                        {
                            //Locks the section to be thread safe as we are now processing a file
                            lock (locker)
                            {
                                using (TextWriter tw = new StreamWriter(Path.Combine(info.Directory, fileName), true))
                                {
                                    tw.WriteLine($"Error Occured: {Now}");
                                    tw.WriteLine($"Function Trace: {stackTrace.GetFrame(2).GetMethod().Name} ({GetAllParameters(stackTrace.GetFrame(2).GetMethod().GetParameters())}) -> {stackTrace.GetFrame(1).GetMethod().Name} ({GetAllParameters(stackTrace.GetFrame(1).GetMethod().GetParameters())})");
                                    tw.WriteLine($"Error Details: {errorMessage}");
                                    tw.WriteLine("");
                                }
                            }
                        }

                        return true;
                    }
                }
            }
            catch { }
            return false;
        }
    }
}