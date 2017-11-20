namespace IPFS.Integration.Utils.Log
{
    public static class Logger
    {
        private static ILogger storedLogger;
        
        public static void SetupLogger(ILogger logger)
        {
            storedLogger = logger;    
        }
        
        public static ILogger Log<T>()
        {
            storedLogger.Context = typeof(T).FullName;
            return storedLogger;
        }
        
        public static ILogger Log()
        {
            storedLogger.Context = "Application Context";
            return storedLogger;
        }
    }
}