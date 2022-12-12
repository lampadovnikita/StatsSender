using Assets.CharacterStatsSemder.Scripts.Observable;
using UnityEngine.Assertions;

namespace Assets.CharacterStatsSemder.Scripts.Character.Level.Progress
{
    public class PlayerLevelProgressBehaviour : LevelProgressBehaviour
    {
        private ObservableValue<CharacterData> observableCharacterData;

        protected override LevelProgress InitializeLevelProgress()
        {
            DataStorageBehaviour dataStorageBehaviour = FindObjectOfType<DataStorageBehaviour>();
            Assert.IsNotNull(dataStorageBehaviour);

            observableCharacterData = dataStorageBehaviour.characterDataObservable;
            ExpPerLevelBounds expPerLevelBounds = dataStorageBehaviour.expPerLevelBounds;

            return new LevelProgress(expPerLevelBounds, observableCharacterData.Value.levelProgressData.totalExp);
        }

        private void Start()
        {
            observableCharacterData.ValueChanged += OnCharacterDataChanged;
        }

        private void OnDestroy()
        {            
            observableCharacterData.ValueChanged += OnCharacterDataChanged;
        }

        private void OnCharacterDataChanged(CharacterData prevData, CharacterData newData)
        {
            if (StoredLevelProgress.expObservable.Value != newData.levelProgressData.totalExp)
            { 
                StoredLevelProgress.expObservable.Value = newData.levelProgressData.totalExp;
            }
        }
    }
}