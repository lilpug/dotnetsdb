using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;

namespace DotNetSDB.output
{
    public partial class OutputManagement
    {
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
                    StringBuilder sb = new StringBuilder();

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        if (i == 0)
                        {
                            sb.Append(parameters[i]);
                        }
                        else
                        {
                            sb.Append($", {parameters[i]}");
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