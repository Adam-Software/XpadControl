using System.IO;
using XpadControl.Extensions;

namespace XpadControl
{
    public class PathCollection
    {
        public PathCollection(string zeroPositionConfigPath, string buttonBindingsConfigPath) 
        {
            zeroPositionConfigPath = zeroPositionConfigPath.ToAbsolutePath();
            buttonBindingsConfigPath = buttonBindingsConfigPath.ToAbsolutePath();

            if (!File.Exists(zeroPositionConfigPath))
                throw new FileNotFoundException($"Can read AdamZeroPositionConfig. File {zeroPositionConfigPath} does not exist");

            if (!File.Exists(buttonBindingsConfigPath))
                throw new FileNotFoundException($"Can read ButtonBindingsConfigPath. File {buttonBindingsConfigPath} does not exist");

            ZeroPositionConfigPath = zeroPositionConfigPath;
            ButtonBindingsConfigPath = buttonBindingsConfigPath;
        }

        public string ZeroPositionConfigPath { get; private set; }
        public string ButtonBindingsConfigPath { get; private set; }
    }
}
