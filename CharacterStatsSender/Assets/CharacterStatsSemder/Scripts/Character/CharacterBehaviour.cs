using Assets.CharacterStatsSemder.Scripts.Character.Level.Progress;
using Assets.CharacterStatsSemder.Scripts.Character.Stats;
using Assets.CharacterStatsSemder.Scripts.Observable;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.CharacterStatsSemder.Scripts.Character
{
    public class CharacterBehaviour : MonoBehaviour
    {
        [SerializeField]
        private LevelProgressBehaviour levelProgressBehaviour = default;

        [SerializeField]
        private StatsBehaviour statsBehaviour = default;

        public string Name { get; private set; }

        private ObservableValue<CharacterData> characterDataObservable;

        private void Awake()
        {
            Assert.IsNotNull(levelProgressBehaviour);
            Assert.IsNotNull(statsBehaviour);

            characterDataObservable = FindObjectOfType<DataStorageBehaviour>().characterDataObservable;
            Name = characterDataObservable.Value.name;
        }

        private void Start()
        {
            characterDataObservable.ValueChanged += OnCharacterDataChanged;
        }

        private void OnDestroy()
        {
            characterDataObservable.ValueChanged -= OnCharacterDataChanged;
        }

        private void OnCharacterDataChanged(CharacterData prevValue, CharacterData newValue)
        {
            if (string.Equals(Name, newValue.name) == false)
            {
                Name = newValue.name;
            }
        }
    }
}