using Assets.CharacterStatsSemder.Scripts.User;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Assets.CharacterStatsSemder.Scripts.UI.Login
{
    public class RegistrationUI : MonoBehaviour
    {
        public event Action RegisterButtonClicked;

        public event Action SwitchToAuthenticationButtonClicked;

        [SerializeField]
        private Button switchToAuthenticationButton = default;

        [SerializeField]
        private Button registerButton = default;

        [SerializeField]
        private TMP_InputField loginInputField = default;

        [SerializeField]
        private TMP_InputField passwordInputField = default;

        public UserData GetUserData => new UserData(loginInputField.text, passwordInputField.text);

        private void Awake()
        {
            Assert.IsNotNull(switchToAuthenticationButton);
            Assert.IsNotNull(registerButton);

            Assert.IsNotNull(loginInputField);
            Assert.IsNotNull(passwordInputField);
        }

        private void Start()
        {        
            registerButton.onClick.AddListener(OnRegisterButtonClicked);
            switchToAuthenticationButton.onClick.AddListener(OnSwitchToAuthenticationButtonClicked);
        }

        private void OnDestroy()
        {
            registerButton.onClick.RemoveListener(OnRegisterButtonClicked);
            switchToAuthenticationButton.onClick.RemoveListener(OnSwitchToAuthenticationButtonClicked);
        }

        private void OnSwitchToAuthenticationButtonClicked()
        {
            SwitchToAuthenticationButtonClicked?.Invoke();
        }

        private void OnRegisterButtonClicked()
        {
            RegisterButtonClicked?.Invoke();
        }
    }
}