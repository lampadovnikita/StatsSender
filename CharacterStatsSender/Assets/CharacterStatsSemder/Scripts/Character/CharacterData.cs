using Assets.CharacterStatsSemder.Scripts.Character.Level.Progress;
using Assets.CharacterStatsSemder.Scripts.Character.Stats;
using Newtonsoft.Json;
using System;

namespace Assets.CharacterStatsSemder.Scripts.Character
{
    public struct CharacterData : IEquatable<CharacterData>
    {
        [JsonProperty("name")]
        public string name;

        [JsonProperty("level_progress")]
        public LevelProgressData levelProgressData;

        [JsonProperty("stats")]
        public StatsData statsData;

        public bool Equals(CharacterData other)
        {
            if ((string.Equals(name, other.name) == true) &&
                (levelProgressData.Equals(other.levelProgressData) == true) &&
                (statsData.Equals(other.statsData) == true))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}