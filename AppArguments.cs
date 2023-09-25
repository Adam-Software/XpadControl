using System.IO;
using XpadControl.Extensions;

namespace XpadControl
{
    public class AppArguments
    {
        public AppArguments(string zeroPositionConfigPath) 
        {
            zeroPositionConfigPath = zeroPositionConfigPath.ToAbsolutePath();

            if (!File.Exists(zeroPositionConfigPath))
                throw new FileNotFoundException($"Can read AdamZeroPositionConfig. File {zeroPositionConfigPath} does not exist");

            ZeroPositionConfigPath = zeroPositionConfigPath;
        }

        public string ZeroPositionConfigPath { get; private set; }
    }
}
