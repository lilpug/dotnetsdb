using System;
using System.IO;

namespace DotNetSDB
{
    public abstract partial class SQLBase : IDisposable
    {
        /*##########################################*/
        /*               Helper Functions           */
        /*##########################################*/

        //This function is used to obtain values between a specific segment within a string
        protected string GetBetweenStringValue(string value, string startValue, string endValue)
        {
            if (!string.IsNullOrWhiteSpace(value) && value.IndexOf(startValue) > -1 && value.IndexOf(endValue) > -1)
            {
                int startPosition = value.IndexOf(startValue) + startValue.Length;
                int totalLength = value.IndexOf(endValue, startPosition) - startPosition;

                if (
                    startPosition >= 0 &&
                    totalLength > 0 &&
                    (startPosition + totalLength) <= value.Length
                    )
                {
                    return value.Substring(startPosition, totalLength);
                }
            }

            return null;
        }

        //This function deletes a file
        protected static bool DeleteFile(string theFilePath)
        {
            int maxWait = 10000;
            int count = 0;

            //Remove the file
            File.Delete(theFilePath);

            //waits a little bit before continuing to ensure the file has been removed.
            //Note: this function does not wait permanently max of 10 seconds then it loops out
            while (count < maxWait && File.Exists(theFilePath))
            {
                System.Threading.Thread.Sleep(1);
                count++;
            }

            if (!File.Exists(theFilePath))
            {
                return true;
            }

            return false;
        }

        //This function creates a file
        protected static bool CreateFile(string theFilePath, string data)
        {
            int maxWait = 10000;
            int count = 0;

            //If the file already exists it deletes it before we create the new version
            if (File.Exists(theFilePath))
            {
                DeleteFile(theFilePath);
            }

            //Writes the file
            File.WriteAllText(theFilePath, data);

            //waits a little bit before continuing to ensure the file has been created.
            //Note: this function does not wait permanently max of 10 seconds then it loops out
            while (count < maxWait && !File.Exists(theFilePath))
            {
                System.Threading.Thread.Sleep(1);
                count++;
            }

            //Checks the file has been created
            if (!File.Exists(theFilePath))
            {
                return true;
            }

            return false;
        }

        //This function appends a file
        protected static bool AppendFile(string theFilePath, string data)
        {
            //Checks to ensure the file already exists
            if (!File.Exists(theFilePath))
            {
                return false;
            }

            //Appends the text to the current file
            File.AppendAllText(theFilePath, data);

            return true;
        }
    }
}