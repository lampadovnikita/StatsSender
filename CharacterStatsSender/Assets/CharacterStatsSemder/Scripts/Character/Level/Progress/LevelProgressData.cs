using Newtonsoft.Json;
using System;

namespace Assets.CharacterStatsSemder.Scripts.Character.Level.Progress
{
    public struct LevelProgressData : IEquatable<LevelProgressData>
    {
        [JsonProperty("total_exp")]
        public int totalExp;

        public LevelProgressData(int totalExp)
        {
            this.totalExp = totalExp;
        }

        public bool Equals(LevelProgressData other)
        {
            if (totalExp == other.totalExp)
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