using Newtonsoft.Json;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.CharacterStatsSemder.Scripts.Character.Level
{
    [Serializable]
    public class ExpPerLevelBounds : IEquatable<ExpPerLevelBounds>
    {
        [SerializeField]
        [JsonProperty("exp_bounds")]
        private int[] expBounds;

        public int LevelCap => expBounds.Length + 1;

        public ExpPerLevelBounds(int[] expBounds)
        {
            Assert.IsTrue(expBounds.Length > 0);

            for (int i = 0; i < expBounds.Length; i++)
            {
                Assert.IsTrue(expBounds[i] > 0);
            }

            this.expBounds = new int[expBounds.Length];
            expBounds.CopyTo(this.expBounds, 0);
        }

        public bool Equals(ExpPerLevelBounds other)
        {
            if (expBounds.Equals(other) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetExpBound(int level)
        {
            Assert.IsTrue(level > 0);

            if ((level - 1) < expBounds.Length)
            {
                return expBounds[level - 1];
            }
            else
            {
                return expBounds[expBounds.Length - 1];
            }
        }
    }
}