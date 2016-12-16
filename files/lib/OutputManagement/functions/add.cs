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
                    //Used to determine whether or not the function failed
                    bool failed = true;

                    //Locks the method calling to ensure its thread safe
                    object locker = new object();
                    lock (locker)
                    {
                        //Gets the date and time in uk format
                        DateTime now = GetDateNow();
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
                                    string errorOutput = "Error Occured: " + now.ToString() + " || ";

                                    //Gets the two functions and their parameters chained down from hitting this function
                                    errorOutput += "Function Trace: " + stackTrace.GetFrame(2).GetMethod().Name + "(" + GetAllParameters(stackTrace.GetFrame(2).GetMethod().GetParameters()) + ")";
                                    errorOutput += " -> " + stackTrace.GetFrame(1).GetMethod().Name + "(" + GetAllParameters(stackTrace.GetFrame(1).GetMethod().GetParameters()) + ") || ";
                                    errorOutput += "Error Details: " + errorMessage;
                                    using (TextWriter tw = new StreamWriter(Path.Combine(info.directory, fileName), true))
                                    {
                                        tw.WriteLine(errorOutput);
                                    }
                                }
                                else//Writes the multi-line log
                                {
                                    using (TextWriter tw = new StreamWriter(Path.Combine(info.directory, fileName), true))
                                    {
                                        tw.WriteLine("Error Occured: " + now.ToString());
                                        tw.WriteLine("Function Trace: " + stackTrace.GetFrame(2).GetMethod().Name + "(" + GetAllParameters(stackTrace.GetFrame(2).GetMethod().GetParameters()) + ") -> " + stackTrace.GetFrame(1).GetMethod().Name + "(" + GetAllParameters(stackTrace.GetFrame(1).GetMethod().GetParameters()) + ")");
                                        tw.WriteLine("Error Details: " + errorMessage);
                                        tw.WriteLine("");
                                    }
                                }

                                //Tells the function its completed without failure
                                failed = false;
                            }
                        }
                    }

                    //This is required as in a lock statement the compiler will move the return type to the end of the lock
                    if (!failed)
                    {
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }
    }
}