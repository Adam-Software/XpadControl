using CommandLineParser.Arguments;

namespace XpadControl
{
    internal class ProgramArguments
    {
        internal const string DefaultConfigName = $"appsettings.default.json";

        [ValueArgument(typeof(string), 'c', 
            LongName = "config", 
            Description = "Set the path to the settings file",
            DefaultValue = DefaultConfigName,
            ValueOptional = true)]
        public string ConfigPathName;

        [SwitchArgument('v',  "version", 
            defaultValue: false,
            Description = "Show app version")]
        public bool ShowVersion; 

        [SwitchArgument('h', "help", defaultValue: false, 
            Description = "Show this help")]
        public bool ShowHelp;
    }
}
