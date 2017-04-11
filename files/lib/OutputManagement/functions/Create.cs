using System;
using System.IO;

namespace DotNetSDB.output
{
    public partial class OutputManagement
    {
        //This function creates a new log file
        private bool CreateNewLogFile(string filename)
        {
            try
            {
                //Saves the file and closes the returned stream
                using (FileStream fs = File.Create(Path.Combine(info.directory, filename))) { }
                return true;
            }
            catch { }
            return false;
        }

        //This function checks if the file exists, if it does not it creates it ready before returning the name
        private string GetFileName(DateTime dateObject)
        {
            try
            {
                //Gets the date object formated correctly
                DateTime now = dateObject;
                if (now != DateTime.MinValue)
                {
                    //Processes the filename
                    string fileName = $"{(now.ToString().Split(' '))[0].Replace('/', '-')}.log";
                    fileName = $@"{info.logName}.{fileName}";

                    //Checks if it exists, if not creates it before returning the name
                    if (!FileExists(fileName))
                    {
                        CreateNewLogFile(fileName);
                        if (!FileExists(fileName))
                        {
                            //If after creating the file it still does not exist then return null as its not been created for some reason.
                            return null;
                        }
                    }
                    return fileName;
                }
            }
            catch { }
            return null;
        }
    }
}