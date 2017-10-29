using System;

namespace DotNetSDB.output
{
    /// <summary>
    /// This class is used to store the settings for the OutputManagement object
    /// </summary>
    public class OutputManagementSettings
    {
        /// <summary>
        /// Stores the directory the logs will be stored in
        /// </summary>
        public string Directory { get; set; }

        /// <summary>
        /// Stores the name of the log that will be created
        /// </summary>
        public string LogName { get; set; }

        /// <summary>
        /// Stores the amount of days old logs can be before the cleanup function will remove them
        /// </summary>
        public int CleanUpDays { get; set; }

        /// <summary>
        /// Stores whether each log entry should be put onto a single line or multiple
        /// </summary>
        public bool SingleLineLog { get; set; }

        /// <summary>
        /// Stores the timezones the OutputManagement should timestamp the logs with
        /// </summary>
        public TimeZoneInfo TimezoneInfo { get; set; }

        /// <summary>
        /// This function is the constructor for the OutputManagementSettings object
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <param name="theLogName"></param>
        /// <param name="timezone"></param>
        /// <param name="multiLineLogging"></param>
        /// <param name="cleanUpDaysAmount"></param>
        public OutputManagementSettings(string directoryPath, string theLogName, TimeZoneInfo timezone, bool multiLineLogging = false, int cleanUpDaysAmount = 0)
        {
            if (!string.IsNullOrWhiteSpace(directoryPath) && !string.IsNullOrWhiteSpace(theLogName))
            {
                if (!System.IO.Directory.Exists(directoryPath))
                {
                    throw new Exception("OutputManagementSettings Error: The directory provided does not exist and is required.");
                }

                //Fills the variables in if everything is ok
                Directory = directoryPath;
                LogName = theLogName;
                SingleLineLog = !multiLineLogging;//Inverts the output
                CleanUpDays = cleanUpDaysAmount;
                TimezoneInfo = timezone;
            }
            else
            {
                throw new Exception("OutputManagementSettings Error: Some of the information provided was empty and is required.");
            }
        }
    }

    /// <summary>
    /// This class deals with the OutputManagement for errors
    /// </summary>
    public partial class OutputManagement : IDisposable
    {
        /// <summary>
        /// stores all the variables required for this library
        /// </summary>
        private OutputManagementSettings info;

        /// <summary>
        /// On request creates a datetime object based the timezone information passed
        /// </summary>
        protected DateTime Now
        {
            get
            {
                return TimeZoneInfo.ConvertTime(DateTime.Now, info.TimezoneInfo);
            }
        }

        /// <summary>
        /// Used to lock the thread while we add data to the log to ensure threadsafe compatibility
        /// </summary>
        private static object locker = new object();

        /// <summary>
        /// This function initiates the OutputManagement object with the supplied settings
        /// </summary>
        /// <param name="variables"></param>
        public OutputManagement(OutputManagementSettings variables)
        {
            if (variables != null)
            {
                info = variables;

                //Attempts to give the directory correct permissions
                if (!GrantAccess(info.Directory))
                {
                    throw new Exception($"OutputManagement Error: Could not give the correct permissions required to the directory '{info.Directory}'.");
                }
            }
            else
            {
                throw new Exception("OutputManagement Error: The variables object is not initialised.");
            }
        }

        /// <summary>
        /// This is the dispose method for disposing of the object
        /// </summary>
        public void Dispose()
        {
            info = null;
        }
    }
}