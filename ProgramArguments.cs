using CommandLineParser.Arguments;
using XpadControl.Extensions;

namespace XpadControl
{
    public class ProgramArguments
    {
        //if the key is not specified, the configuration is loaded from here
        private const string DefaultConfigPath = $"Configs/appsettings.default.json";

        private string mConfigPath;

        [ValueArgument(typeof(string), 'c',
            LongName = "set-config-path",
            Description = "Loads the settings from the specified configuration file path and runs the program",
            DefaultValue = DefaultConfigPath,
            ValueOptional = true)]
        public string ConfigPath 
        { 
            get 
            { 
                return mConfigPath; 
            } 
            private set
            {
                mConfigPath = value.ToAbsolutePath();
            }
        }

        [SwitchArgument('s', "show-config-path", 
            defaultValue: false, 
            Description = "Shows path loaded config and close app.")]
        public bool ShowConfigPath { get; private set; }

        [SwitchArgument('v',  "version", 
            defaultValue: false,
            Description = "Show app version and close app.")]
        public bool ShowVersion { get; private set; } 

        [SwitchArgument('h', "help", defaultValue: false, 
            Description = "Show this help and close app.")]
        public bool ShowHelp { get; private set;  }
    }
}
