using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.CharacterStatsSemder.Scripts.UI.Login
{
    public class LoginUI : MonoBehaviour
    {
        [SerializeField]
        private AuthenticationUI authenticationUI = default;

        [SerializeField]
        private RegistrationUI registrationUI = default;

        public void SwitchScreenToAuth()
        {
            registrationUI.gameObject.SetActive(false);
            authenticationUI.gameObject.SetActive(true);
        }

        public void SwitchScreenToRegister()
        {
            authenticationUI.gameObject.SetActive(false);
            registrationUI.gameObject.SetActive(true);
        }

        private void Awake()
        {
            Assert.IsNotNull(authenticationUI);
            Assert.IsNotNull(registrationUI);
        }

        private void Start()
        {
            SwitchScreenToAuth();

            authenticationUI.SwitchToRegistrationButtonClicked += SwitchScreenToRegister;
            registrationUI.SwitchToAuthenticationButtonClicked += SwitchScreenToAuth;
        }

        private void OnDestroy()
        {
            authenticationUI.SwitchToRegistrationButtonClicked -= SwitchScreenToRegister;
            registrationUI.SwitchToAuthenticationButtonClicked -= SwitchScreenToAuth;
        }
    }
}
