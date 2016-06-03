using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;

namespace DotNetSDB.output
{
    public partial class OutputManagement
    {
        //This functions returns the datetime using the uk style
        private DateTime GetDateNow()
        {
            try
            {
                CultureInfo cultureinfo = new CultureInfo(info.cultureInfo);
                return DateTime.Parse(DateTime.Now.ToString(), cultureinfo);
            }
            catch { }
            return DateTime.MinValue;
        }

        //This function checks if the file exists
        private bool FileExists(string filename)
        {
            return File.Exists(Path.Combine(info.directory, filename));
        }

        //This function returns all the parameters in string format
        private string GetAllParameters(ParameterInfo[] parameters)
        {
            try
            {
                if (parameters.Length > 0)
                {
                    string concatParams = "";
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        if (i == 0)
                        {
                            concatParams += parameters[i];
                        }
                        else
                        {
                            concatParams += "," + parameters[i];
                        }
                    }
                    return concatParams;
                }
            }
            catch { }
            return null;
        }

        //This function gives permissions for the directory
        private bool GrantAccess(string fullPath)
        {
            DirectoryInfo dInfo = new DirectoryInfo(fullPath);
            dInfo.Attributes &= ~FileAttributes.ReadOnly;      //Removes any readonly options
            DirectorySecurity dSecurity = dInfo.GetAccessControl();
            dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
            dInfo.SetAccessControl(dSecurity);
            return true;
        }
    }
}