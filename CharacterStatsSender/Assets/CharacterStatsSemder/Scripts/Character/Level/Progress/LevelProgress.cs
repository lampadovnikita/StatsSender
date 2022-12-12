using Assets.CharacterStatsSemder.Scripts.Observable;
using System;
using UnityEngine.Assertions;

namespace Assets.CharacterStatsSemder.Scripts.Character.Level.Progress
{
    public class LevelProgress
    {
        public event Action<int, int> LevelChanged;

        public readonly ObservableValue<int> expObservable;

        private ExpPerLevelBounds bounds;

        public ExpPerLevelBounds Bounds
        {
            get
            {
                return bounds;
            }

            set
            {
                Assert.IsNotNull(value);

                bounds = value;

                RecalculateCurrentLevel();
            }
        }

        public LevelProgressData Data
        {
            get
            {
                return new LevelProgressData(expObservable.Value);
            }

            set
            {
                expObservable.Value = value.totalExp;    
            }
        }

        public int CurrentLevel { get; private set; }

        public int CurrentLevelGainedExp { get; private set; }

        public LevelProgress(ExpPerLevelBounds expPerLevelBounds)
        {
            expObservable = new ObservableValue<int>();

            this.Bounds = expPerLevelBounds;

            CurrentLevel = 1;

            expObservable.ValueChanged += OnExpChanged;
        }

        public LevelProgress(ExpPerLevelBounds expPerLevelBounds, int totalExp)
            : this(expPerLevelBounds)
        {
            expObservable.Value = totalExp;
        }

        private void OnExpChanged(int prevExp, int newExp)
        {
            RecalculateCurrentLevel();
        }

        private void RecalculateCurrentLevel()
        {            
            int lvl = 0;
            int expCapacitor = 0;

            do
            {
                lvl++;

                expCapacitor += Bounds.GetExpBound(lvl);
            }
            while (expCapacitor <= expObservable.Value);

            expCapacitor -= Bounds.GetExpBound(lvl);
            CurrentLevelGainedExp = expObservable.Value - expCapacitor;

            int prevLevel = CurrentLevel;
            CurrentLevel = lvl;

            LevelChanged?.Invoke(prevLevel, CurrentLevel);
        }
    }
}