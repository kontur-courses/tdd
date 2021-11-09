using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace TagsCloudVisualization
{
    internal static class DirectoryInfoExtensions
    {
        internal static bool CanWrite(this DirectoryInfo directory)
        {
            var attrs = directory.GetAccessControl(AccessControlSections.Access);
            var rules = attrs.GetAccessRules(true, true, typeof(NTAccount));
            var NtAccountName = $"{Environment.UserDomainName}\\{Environment.UserName}";
            foreach (AuthorizationRule rule in rules)
            {
                if (rule.IdentityReference.Value.Equals(NtAccountName, StringComparison.CurrentCultureIgnoreCase))
                {
                    var filesystemAccessRule = (FileSystemAccessRule) rule;

                    if ((filesystemAccessRule.FileSystemRights & FileSystemRights.WriteData) > 0 &&
                        filesystemAccessRule.AccessControlType != AccessControlType.Deny)
                        return true;
                    return false;
                }
            }

            return false;
        }
    }
}