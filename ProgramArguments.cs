using CommandLineParser.Arguments;

namespace XpadControl
{
    public class ProgramArguments
    {
        private const string DefaultConfigName = $"Configs/appsettings.default.json";

        [ValueArgument(typeof(string), 'c', 
            LongName = "config", 
            Description = "Set the path to the settings file",
            DefaultValue = DefaultConfigName,
            ValueOptional = true)]
        public string ConfigPathName = DefaultConfigName;

        [SwitchArgument('s', "show-config-path", 
            defaultValue: false, 
            Description = "Shows path loaded config and close app.")]
        public bool ShowConfigPath;

        [SwitchArgument('v',  "version", 
            defaultValue: false,
            Description = "Show app version and close app.")]
        public bool ShowVersion; 

        [SwitchArgument('h', "help", defaultValue: false, 
            Description = "Show this help and close app.")]
        public bool ShowHelp;
    }
}
