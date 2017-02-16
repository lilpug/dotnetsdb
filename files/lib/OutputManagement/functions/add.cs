using System;
using System.Diagnostics;
using System.IO;

namespace DotNetSDB.output
{
    public partial class OutputManagement
    {
        //Need to look at how to make this thread safe as it will work only in procedural methods!!
        public bool AddToLog(string errorMessage)
        {
            try
            {
                //Checks if cleanup is activated and does its job if so
                if (cleanupLogs())
                {
                    //Gets the date and time in uk format
                    DateTime now = info.currentDateTime;
                    if (now != DateTime.MinValue)
                    {
                        //Gets the stack trace object ready for obtaining method names
                        StackTrace stackTrace = new StackTrace();

                        //Note: while getting the filename this creates the filename if it does not already exist ready for adding text
                        string fileName = GetFileName(now);
                        if (!string.IsNullOrWhiteSpace(fileName))
                        {
                            //Writes the single line log
                            if (info.singleLineLog)
                            {
                                //Creates the error message which will be put into the log file
                                string errorOutput = string.Format("Error Occured: {0} || ", now.ToString());

                                //Gets the two functions and their parameters chained down from hitting this function
                                errorOutput += string.Format("Function Trace: {0} ({1})", stackTrace.GetFrame(2).GetMethod().Name, GetAllParameters(stackTrace.GetFrame(2).GetMethod().GetParameters()));
                                errorOutput += string.Format("-> {0} ({1}) ||", stackTrace.GetFrame(1).GetMethod().Name, GetAllParameters(stackTrace.GetFrame(1).GetMethod().GetParameters()));
                                errorOutput += string.Format("Error Details: {0}", errorMessage);

                                //Locks the section to be thread safe as we are now processing a file
                                lock (locker)
                                {
                                    using (TextWriter tw = new StreamWriter(Path.Combine(info.directory, fileName), true))
                                    {
                                        tw.WriteLine(errorOutput);
                                    }
                                }
                            }
                            else//Writes the multi-line log
                            {
                                //Locks the section to be thread safe as we are now processing a file
                                lock (locker)
                                {
                                    using (TextWriter tw = new StreamWriter(Path.Combine(info.directory, fileName), true))
                                    {
                                        tw.WriteLine(string.Format("Error Occured: {0}", now.ToString()));
                                        tw.WriteLine(string.Format("Function Trace: {0} ({1}) -> {2} ({3})", stackTrace.GetFrame(2).GetMethod().Name, GetAllParameters(stackTrace.GetFrame(2).GetMethod().GetParameters()), stackTrace.GetFrame(1).GetMethod().Name, GetAllParameters(stackTrace.GetFrame(1).GetMethod().GetParameters())));
                                        tw.WriteLine(string.Format("Error Details: {0}", errorMessage));
                                        tw.WriteLine("");
                                    }
                                }
                            }

                            return true;
                        }
                    }
                    
                }
            }
            catch { }
            return false;
        }
    }
}