using UnityEngine;
namespace Assets.CharacterStatsSemder.Scripts.Character.Level.Progress
{
    public abstract class LevelProgressBehaviour : MonoBehaviour
    {       
        public LevelProgress StoredLevelProgress { get; protected set; }

        protected void Awake()
        {
            StoredLevelProgress = InitializeLevelProgress();
        }
       
        protected abstract LevelProgress InitializeLevelProgress();
    }
}