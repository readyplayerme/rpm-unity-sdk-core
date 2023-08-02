namespace ReadyPlayerMe.Core
{
    public static class AnalyticsRuntimeLogger
    {
        internal static readonly IAnalyticsRuntimeLogger EventLogger;

        static AnalyticsRuntimeLogger()
        {
            EventLogger = new AnalyticsRuntimeEventLogger();
        }
    }
}
