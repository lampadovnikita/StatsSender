using Assets.CharacterStatsSemder.Scripts.Observable;
using UnityEngine.Assertions;

namespace Assets.CharacterStatsSemder.Scripts.Character.Stats
{
    public class PlayerStatsBehaviour : StatsBehaviour
    {
        private ObservableValue<CharacterData> observableCharacterData;

        protected override Stats InitializeStats()
        {
            DataStorageBehaviour dataStorageBehaviour = FindObjectOfType<DataStorageBehaviour>();
            Assert.IsNotNull(dataStorageBehaviour);

            observableCharacterData = dataStorageBehaviour.characterDataObservable;

            return new Stats(observableCharacterData.Value.statsData);
        }

        private void Start()
        {
            observableCharacterData.ValueChanged += OnStatsDataChanged;
        }

        private void OnDestroy()
        {
            observableCharacterData.ValueChanged -= OnStatsDataChanged;
        }

        private void OnStatsDataChanged(CharacterData prevData, CharacterData newData)
        {
            StoredStats.StatsData = newData.statsData;
        }
    }
}