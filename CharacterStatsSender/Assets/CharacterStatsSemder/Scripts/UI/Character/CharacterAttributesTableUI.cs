using Assets.CharacterStatsSemder.Scripts.Character;
using Assets.CharacterStatsSemder.Scripts.Character.Level.Progress;
using Assets.CharacterStatsSemder.Scripts.Character.Stats;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.CharacterStatsSemder.Scripts.UI.Character
{
    public class CharacterAttributesTableUI : MonoBehaviour
    {
        [SerializeField]
        private CharacterAttributeUI nameAttributeUI = default;

        [SerializeField]
        private CharacterAttributeUI levelAttributeUI = default;

        [SerializeField]
        private ExpUI expUI = default;

        [SerializeField]
        private CharacterAttributeUI strengthAttributeUI = default;

        [SerializeField]
        private CharacterAttributeUI wisdomAttributeUI = default;

        [SerializeField]
        private CharacterAttributeUI agilityAttributeUI = default;

        [SerializeField]
        private LevelProgressBehaviour levelProgressBehaviour = default;

        [SerializeField]
        private StatsBehaviour statsBehaviour = default;

        [SerializeField]
        private CharacterBehaviour playerCharacterBehaviour = default;

        private LevelProgress levelProgress;

        private Stats stats;

        private void Awake()
        {
            Assert.IsNotNull(nameAttributeUI);
            
            Assert.IsNotNull(levelAttributeUI);
            Assert.IsNotNull(expUI);
            
            Assert.IsNotNull(strengthAttributeUI);
            Assert.IsNotNull(wisdomAttributeUI);
            Assert.IsNotNull(agilityAttributeUI);

            Assert.IsNotNull(levelProgressBehaviour);
            Assert.IsNotNull(statsBehaviour);
            Assert.IsNotNull(playerCharacterBehaviour);
        }

        private void Start()
        {
            levelProgress = levelProgressBehaviour.StoredLevelProgress;
            stats = statsBehaviour.StoredStats;

            SubscribeStats();
            SubscribeLevelProgress();

            InitializeTextValues();
        }

        private void OnDestroy()
        {
            UnsubscribeStats();
            UnsubscribeLevelProgress();
        }

        public void InitializeTextValues()
        {
            nameAttributeUI.ValueText = playerCharacterBehaviour.Name;

            levelAttributeUI.ValueText = levelProgress.CurrentLevel.ToString();

            expUI.GainedExp = levelProgress.CurrentLevelGainedExp;
            expUI.ExpBound = levelProgress.Bounds.GetExpBound(levelProgress.CurrentLevel);

            strengthAttributeUI.ValueText = stats.strengthObservable.Value.ToString();
            wisdomAttributeUI.ValueText = stats.wisdomObservable.Value.ToString();
            agilityAttributeUI.ValueText = stats.agilityObservable.Value.ToString();
        }

        private void SubscribeStats()
        {
            stats.strengthObservable.ValueChanged += OnStrengthChanged;
            stats.wisdomObservable.ValueChanged += OnWisdomChanged;
            stats.agilityObservable.ValueChanged += OnAgilityChanged;
        }

        private void UnsubscribeStats()
        {
            stats.strengthObservable.ValueChanged -= OnStrengthChanged;
            stats.wisdomObservable.ValueChanged -= OnWisdomChanged;
            stats.agilityObservable.ValueChanged -= OnAgilityChanged;
        }

        private void SubscribeLevelProgress()
        {
            levelProgress.LevelChanged += OnLevelChanged;
            levelProgress.expObservable.ValueChanged += OnExpChanged;
        }

        private void UnsubscribeLevelProgress()
        {
            levelProgress.LevelChanged -= OnLevelChanged;
            levelProgress.expObservable.ValueChanged -= OnExpChanged;
        }

        private void OnStrengthChanged(int prevStrength, int newStrength)
        {
            strengthAttributeUI.ValueText = newStrength.ToString();
        }

        private void OnWisdomChanged(int prevWisdom, int newWisdom)
        {
            strengthAttributeUI.ValueText = newWisdom.ToString();
        }

        private void OnAgilityChanged(int prevAgility, int newAgility)
        {
            strengthAttributeUI.ValueText = newAgility.ToString();
        }

        private void OnLevelChanged(int prevLevel, int newLevel)
        {
            levelAttributeUI.ValueText = newLevel.ToString();

            expUI.ExpBound = levelProgress.Bounds.GetExpBound(newLevel);
            expUI.GainedExp = levelProgress.CurrentLevelGainedExp;
        }

        private void OnExpChanged(int prevExp, int newExp)
        {
            expUI.GainedExp = levelProgress.CurrentLevelGainedExp;
        }
    }
}