using System.Reflection;
using Serilog;
using Serilog.Core;

namespace Discord_Bot_FS_I.Logging;

public static class LoggerFactory
{
    private const string ContextPropertyName = Constants.SourceContextPropertyName;

    private const string OutputTemplate = "[{Timestamp:HH:mm:ss} {" + ContextPropertyName +
                                          "} {Level:u3}] {Message:lj}{NewLine}{Exception}";

    public static Logger ForContext(MemberInfo type)
    {
        return ForContext(type.Name);
    }

    public static Logger ForContext(string context)
    {
        return BaseConfig()
            .Enrich.WithProperty(ContextPropertyName, context)
            .CreateLogger();
    }

    public static Logger ForUnknownContext()
    {
        return BaseConfig().CreateLogger();
    }


    private static LoggerConfiguration BaseConfig()
    {
        return new LoggerConfiguration()
            .WriteTo.Console(outputTemplate: OutputTemplate)
            .MinimumLevel.Debug();
    }
}
