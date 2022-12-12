using Assets.CharacterStatsSemder.Scripts.Character;
using Assets.CharacterStatsSemder.Scripts.Character.Level.Progress;
using Assets.CharacterStatsSemder.Scripts.Character.Stats;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Assets.CharacterStatsSemder.Scripts.UI.Character
{
    public class CharacterCreationUI : MonoBehaviour
    {
        public event Action<CharacterData> CreationSubmited;

        [SerializeField]
        private TMP_InputField nameInput = default;

        [SerializeField]
        private TMP_InputField strengthInput = default;

        [SerializeField]
        private TMP_InputField wisdomInput = default;

        [SerializeField]
        private TMP_InputField agilityInput = default;

        [SerializeField]
        private Button createButton = default;

        private bool IsValid =>
            (nameInput.text != string.Empty) &&
            (strengthInput.text != string.Empty) &&
            (agilityInput.text != string.Empty) &&
            (wisdomInput.text != string.Empty);

        private void Awake()
        {
            Assert.IsNotNull(nameInput);
            Assert.IsNotNull(strengthInput);
            Assert.IsNotNull(wisdomInput);
            Assert.IsNotNull(agilityInput);
            Assert.IsNotNull(createButton);
        }

        private void Start()
        {
            createButton.onClick.AddListener(AttemptCreate);
        }

        private void OnDestroy()
        {            
            createButton.onClick.RemoveListener(AttemptCreate);
        }

        private void AttemptCreate()
        {
            if (IsValid == false)
            {
                return;
            }

            StatsData statsData;
            bool isParsed;

            isParsed = int.TryParse(strengthInput.text, out statsData.strength);
            if (isParsed == false)
            {
                return;
            }
            
            isParsed = int.TryParse(wisdomInput.text, out statsData.wisdom);
            if (isParsed == false)
            {
                return;
            }

            isParsed = int.TryParse(agilityInput.text, out statsData.agility);
            if (isParsed == false)
            {
                return;
            }

            CharacterData characterData;
            characterData.name = nameInput.text;
            characterData.statsData = statsData;
            characterData.levelProgressData = new LevelProgressData(0);

            CreationSubmited?.Invoke(characterData);
        }
    }
}