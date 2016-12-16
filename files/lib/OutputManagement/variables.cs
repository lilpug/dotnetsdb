using System;
using System.IO;

namespace DotNetSDB.output
{
    public class OutputManagementVariable
    {
        //Variables
        public string directory { get; set; }

        public string logName { get; set; }
        public int cleanUpDays { get; set; }
        public bool singleLineLog { get; set; }
        public string cultureInfo { get; set; }

        //Constructor
        public OutputManagementVariable(string directoryPath, string theLogName, bool multiLineLogging = false, int cleanUpDaysAmount = 0, string cultureTimeInfo = "en-gb")
        {
            if (!string.IsNullOrWhiteSpace(directoryPath) && !string.IsNullOrWhiteSpace(theLogName))
            {
                if (!Directory.Exists(directoryPath))
                {
                    throw new Exception("OutputManagementVariable Error: The directory provided does not exist and is required.");
                }

                //Fills the variables in if everything is ok
                directory = directoryPath;
                logName = theLogName;
                singleLineLog = !multiLineLogging;//Inverts the output
                cleanUpDays = cleanUpDaysAmount;
                cultureInfo = cultureTimeInfo;
            }
            else
            {
                throw new Exception("OutputManagementVariable Error: Some of the information provided was empty and is required.");
            }
        }
    }

    public partial class OutputManagement : IDisposable
    {
        //Holds all the variables required for this library
        private OutputManagementVariable info;

        public OutputManagement(OutputManagementVariable variables)
        {
            if (variables != null)
            {
                info = variables;

                //Attempts to give the directory correct permissions, if it cannot it errors
                if (!GrantAccess(info.directory))
                {
                    throw new Exception(string.Format("OutputManagement Error: Could not give the correct permissions required to the directory '{0}'.", info.directory));
                }

                //Checks if cleanup is scheduled and if it failed
                if (info.cleanUpDays > 0 && !cleanupLogs())
                {
                    throw new Exception(string.Format("OutputManagement Error: The cleanup has failed."));
                }
            }
            else
            {
                throw new Exception(string.Format("OutputManagement Error: The variables object is not initialised."));
            }
        }

        //This is the dispose method for disposing of the object
        public void Dispose()
        {
            info = null;
        }
    }
}