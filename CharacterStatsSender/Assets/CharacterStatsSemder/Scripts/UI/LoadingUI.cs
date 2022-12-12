using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.CharacterStatsSemder.Scripts.UI
{
    public class LoadingUI : MonoBehaviour
    {
        private static string[] LOADING_TEXTS = { "Loading", "Loading.", "Loading..", "Loading..." };

        [SerializeField]
        private TextMeshProUGUI loadingTMPro;

        [SerializeField]
        [Tooltip("In seconds")]
        private float textSwitchPeriod = 1.0f;

        private IEnumerator loadingTextSwitchRoutine;

        private void Awake()
        {
            Assert.IsNotNull(loadingTMPro);

            loadingTextSwitchRoutine = LoadingTextSwitchRoutine();
        }

        private void OnEnable()
        {
            StartCoroutine(loadingTextSwitchRoutine);
        }

        private void OnDisable()
        {
            StopCoroutine(loadingTextSwitchRoutine);
        }

        private IEnumerator LoadingTextSwitchRoutine()
        {
            int currentLoadingTextIndex = 0;

            while (true)
            {
                loadingTMPro.text = LOADING_TEXTS[currentLoadingTextIndex];

                yield return new WaitForSeconds(textSwitchPeriod);

                currentLoadingTextIndex++;
                if (currentLoadingTextIndex == LOADING_TEXTS.Length)
                {
                    currentLoadingTextIndex = 0;
                }
            }
        }
    }
}