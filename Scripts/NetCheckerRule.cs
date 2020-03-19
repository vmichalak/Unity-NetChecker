namespace VMichalak.NetChecker
{
    public class NetCheckerRule
    {
        public static readonly NetCheckerRule APPLE_HOTSPOT = new NetCheckerRule("https://captive.apple.com/hotspot-detect.html", 200, "<HTML><HEAD><TITLE>Success</TITLE></HEAD><BODY>Success</BODY></HTML>\n");
        public static readonly NetCheckerRule GOOGLE_204 = new NetCheckerRule("https://clients3.google.com/generate_204", 204, "");
        public static readonly NetCheckerRule MICROSOFT_CONNECT_TEST = new NetCheckerRule("http://www.msftconnecttest.com/connecttest.txt", 200, "Microsoft Connect Test");
        
#if UNITY_IOS || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
        public static readonly NetCheckerRule PLATFORM = APPLE_HOTSPOT;
#elif UNITY_XBOXONE || UNITY_WSA || UNITY_WSA_10_0 || UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        public static readonly NetCheckerRule PLATFORM = MICROSOFT_CONNECT_TEST;
#elif UNITY_ANDROID
        public static readonly NetCheckerRule PLATFORM = GOOGLE_204;
#else 
        public static readonly NetCheckerRule PLATFORM = GOOGLE_204;
#endif
            
        public readonly string Target;
        public readonly long ExpectedHttpCode;
        public readonly string ExpectedContent;

        public NetCheckerRule(string target, long expectedHttpCode, string expectedContent)
        {
            Target = target;
            ExpectedHttpCode = expectedHttpCode;
            ExpectedContent = expectedContent;
        }
    }
}