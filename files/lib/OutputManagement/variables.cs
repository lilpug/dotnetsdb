using System;
using System.IO;

namespace DotNetSDB.output
{
    public class OutputManagementSettings
    {
        //Variables
        public string directory { get; set; }

        public string logName { get; set; }
        public int cleanUpDays { get; set; }
        public bool singleLineLog { get; set; }
        public DateTime currentDateTime { get; set; }

        //Constructor
        public OutputManagementSettings(string directoryPath, string theLogName, DateTime datetime, bool multiLineLogging = false, int cleanUpDaysAmount = 0)
        {
            if (!string.IsNullOrWhiteSpace(directoryPath) && !string.IsNullOrWhiteSpace(theLogName))
            {
                if (!Directory.Exists(directoryPath))
                {
                    throw new Exception("OutputManagementSettings Error: The directory provided does not exist and is required.");
                }

                //Fills the variables in if everything is ok
                directory = directoryPath;
                logName = theLogName;
                singleLineLog = !multiLineLogging;//Inverts the output
                cleanUpDays = cleanUpDaysAmount;
                currentDateTime = datetime;
            }
            else
            {
                throw new Exception("OutputManagementSettings Error: Some of the information provided was empty and is required.");
            }
        }
    }

    public partial class OutputManagement : IDisposable
    {
        //Holds all the variables required for this library
        private OutputManagementSettings info;

        //Used to lock the thread while we add data to the log to ensure threadsafe compatibility
        private static object locker = new object();

        public OutputManagement(OutputManagementSettings variables)
        {
            if (variables != null)
            {
                info = variables;

                //Attempts to give the directory correct permissions
                if (!GrantAccess(info.directory))
                {
                    throw new Exception($"OutputManagement Error: Could not give the correct permissions required to the directory '{info.directory}'.");
                }
            }
            else
            {
                throw new Exception("OutputManagement Error: The variables object is not initialised.");
            }
        }

        //This is the dispose method for disposing of the object
        public void Dispose()
        {
            info = null;
        }
    }
}