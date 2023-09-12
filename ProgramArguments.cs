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
        internal string ConfigPathName;

        [SwitchArgument('v',  "version", 
            defaultValue: false,
            Description = "Show app version")]
        internal bool ShowVersion; 

        [SwitchArgument('h', "help", defaultValue: false, 
            Description = "Show this help")]
        internal bool ShowHelp;
    }
}
