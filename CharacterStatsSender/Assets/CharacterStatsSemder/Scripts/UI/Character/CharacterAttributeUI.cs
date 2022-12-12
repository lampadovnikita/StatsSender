using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.CharacterStatsSemder.Scripts.UI.Character
{
    public class CharacterAttributeUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI valueTMPro = default;

        public string ValueText
        {
            get
            {
                return valueTMPro.text;
            }

            set
            {
                valueTMPro.text = value;
            }
        }

        private void Awake()
        {
            Assert.IsNotNull(valueTMPro);
        }
    }
}