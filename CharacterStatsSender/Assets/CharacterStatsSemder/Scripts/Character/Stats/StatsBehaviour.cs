using System;
using UnityEngine;

namespace Assets.CharacterStatsSemder.Scripts.Character.Stats
{
    public abstract class StatsBehaviour : MonoBehaviour
    {
        public Stats StoredStats { get; protected set; }

        protected void Awake()
        {
            StoredStats = InitializeStats();
        }

        protected abstract Stats InitializeStats();
    }
}