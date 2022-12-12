using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.CharacterStatsSemder.Scripts.UI
{
    public class ExpUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI expTMPro = default;

        private int gainedExp;

        private int expBound;

        public int GainedExp
        {
            get
            {
                return gainedExp;
            }
            set
            {
                gainedExp = value;

                UpdateExpText();
            }
        }

        public int ExpBound
        {
            get
            {
                return expBound;
            }

            set
            {
                expBound = value;

                UpdateExpText();
            }
        }

        private void Awake()
        {
            Assert.IsNotNull(expTMPro);
        }

        private void UpdateExpText()
        {
            expTMPro.text = $"{gainedExp}/{expBound}";
        }
    }
}