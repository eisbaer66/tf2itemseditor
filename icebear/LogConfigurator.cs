using log4net.Appender;
using log4net.Config;
using log4net.Layout;

namespace TF2Items.ValvePak.Tests
{
    public static class LogConfigurator
    {
        private static bool _configured;

        public static void  ForTest()
        {
            if (_configured)
                return;

            BasicConfigurator.Configure(new ConsoleAppender { Layout = new PatternLayout("%date [%thread] %-5level %logger [%property{NDC}] - %message%newline") });
            _configured = true;
        }
    }
}