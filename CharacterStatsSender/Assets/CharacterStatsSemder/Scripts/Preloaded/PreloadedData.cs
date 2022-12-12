using Assets.CharacterStatsSemder.Scripts.Character;
using Assets.CharacterStatsSemder.Scripts.Character.Level;
using Newtonsoft.Json;

namespace Assets.CharacterStatsSemder.Scripts.Preloaded
{
    public struct PreloadedData
    {
        [JsonProperty("level_bounds")]
        public ExpPerLevelBounds expPerLevelBounds;

        [JsonProperty("is_character_created")]
        public bool isCharacterCreated;

        [JsonProperty("character")]
        public CharacterData characterData;
    }
}