using Assets.CharacterStatsSemder.Scripts.User;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Assets.CharacterStatsSemder.Scripts.UI.Login
{
    public class AuthenticationUI : MonoBehaviour
    {
        public event Action AuthButtonClicked;

        public event Action SwitchToRegistrationButtonClicked;

        [SerializeField]
        private TMP_InputField loginInputField = default;

        [SerializeField]
        private TMP_InputField passwordInputField = default;

        [SerializeField]
        private Button authButton = default;

        [SerializeField]
        private Button switchToRegistrationButton = default;

        public UserData GetUserData => new UserData(loginInputField.text, passwordInputField.text);

        private void Awake()
        {
            Assert.IsNotNull(loginInputField);
            Assert.IsNotNull(passwordInputField);

            Assert.IsNotNull(authButton);
            Assert.IsNotNull(switchToRegistrationButton);
        }

        private void Start()
        {
            authButton.onClick.AddListener(OnAuthButtonClicked);
            switchToRegistrationButton.onClick.AddListener(OnSwitchToRegistrationButtonClicked);
        }

        private void OnDestroy()
        {
            authButton.onClick.RemoveListener(OnAuthButtonClicked);
            switchToRegistrationButton.onClick.RemoveListener(OnSwitchToRegistrationButtonClicked);
        }

        private void OnAuthButtonClicked()
        {
            AuthButtonClicked?.Invoke();
        }

        private void OnSwitchToRegistrationButtonClicked()
        {
            SwitchToRegistrationButtonClicked?.Invoke();
        }
    }
}