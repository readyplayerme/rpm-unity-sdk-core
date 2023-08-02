namespace ReadyPlayerMe.Core.Analytics
{
    public static class AnalyticsRuntimeLogger
    {
        internal static readonly IAnalyticsRuntimeLogger EventLogger;

        static AnalyticsRuntimeLogger()
        {
            EventLogger = new AmplitudeRuntimeLogger();
        }
    }
}
