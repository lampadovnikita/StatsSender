using Assets.CharacterStatsSemder.Scripts.Character;
using Assets.CharacterStatsSemder.Scripts.Character.Level;
using Assets.CharacterStatsSemder.Scripts.Observable;
using UnityEngine;

namespace Assets.CharacterStatsSemder.Scripts
{
    public class DataStorageBehaviour : MonoBehaviour
    {
        [HideInInspector]
        public ObservableValue<CharacterData> characterDataObservable;

        [HideInInspector]
        public ObservableValue<bool> isCharacterCreatedObservable;

        [HideInInspector]
        public ExpPerLevelBounds expPerLevelBounds;

        public void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}