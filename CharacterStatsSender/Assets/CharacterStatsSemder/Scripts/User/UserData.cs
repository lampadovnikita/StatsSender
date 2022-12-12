using Newtonsoft.Json;

namespace Assets.CharacterStatsSemder.Scripts.User
{
    public struct UserData
    {
        [JsonProperty("login")]
        public string login;

        [JsonProperty("password")]
        public string password;

        public UserData(string login, string password)
        { 
            this.login = login;
            this.password = password;
        }
    }
}