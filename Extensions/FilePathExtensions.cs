using System;
using System.IO;

namespace XpadControl.Extensions
{
    public static class FilePathExtensions
    {
        public static string ToAbsolutePath(this string relativePath)
        {
            string workingDirrectory = AppDomain.CurrentDomain.BaseDirectory;
            string nestedDir = Path.GetDirectoryName(relativePath);
            string fileName = Path.GetFileName(relativePath);
            string fullSettingsPath = Path.Combine(workingDirrectory, nestedDir, fileName);
            
            return fullSettingsPath;
        }
    }
}
