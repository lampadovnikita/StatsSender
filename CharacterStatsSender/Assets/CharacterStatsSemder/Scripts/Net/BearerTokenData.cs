using Newtonsoft.Json;

namespace Assets.CharacterStatsSemder.Scripts.Net
{
    public struct BearerTokenData
    {
        [JsonProperty("bearer_token")]
        public string token;

        public BearerTokenData(string token)
        {
            this.token = token;
        }
    }
}