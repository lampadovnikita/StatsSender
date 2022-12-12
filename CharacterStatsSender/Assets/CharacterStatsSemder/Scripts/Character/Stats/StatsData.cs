using Newtonsoft.Json;
using System;

namespace Assets.CharacterStatsSemder.Scripts.Character.Stats
{
    public struct StatsData : IEquatable<StatsData>
    {
        [JsonProperty("strength")]
        public int strength;

        [JsonProperty("wisdom")]
        public int wisdom;

        [JsonProperty("agility")]
        public int agility;

        public StatsData(int strength, int wisdom, int agility)
        {
            this.strength = strength;
            this.wisdom = wisdom;
            this.agility = agility;
        }

        public bool Equals(StatsData other)
        {
            if ((strength == other.strength) &&
                (wisdom == other.wisdom) &&
                (agility == other.agility))
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