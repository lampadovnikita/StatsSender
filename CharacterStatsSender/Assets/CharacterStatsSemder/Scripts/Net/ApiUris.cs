namespace Assets.CharacterStatsSemder.Scripts.Net
{
    public static class ApiUris
    {
        public static string PING;

        public static string GET_PRELOAD_DATA;

        public static string GET_CHARACTER_DATA;

        public static string GET_CHARACTER_LEVEL_BOUNDS;

        public static string POST_USER_AUTH;

        public static string POST_USER_REGISTER;

        public static string POST_CHARACTER_CREATE;

        private static string BASE_URI = "http://localhost:8081";

        static ApiUris()
        {
            PING = BuildUri("ping");

            GET_CHARACTER_DATA = BuildUri("character");

            GET_CHARACTER_LEVEL_BOUNDS = BuildUri("level/bounds");

            GET_PRELOAD_DATA = BuildUri("preload");

            POST_USER_AUTH = BuildUri("user/authenticate");

            POST_USER_REGISTER = BuildUri("user/register");

            POST_CHARACTER_CREATE = BuildUri("character/create");
        }

        private static string BuildUri(string apiUri)
        {
            return $"{BASE_URI}/{apiUri}";
        }
    }
}