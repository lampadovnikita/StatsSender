using Assets.CharacterStatsSemder.Scripts.Net;
using Assets.CharacterStatsSemder.Scripts.Scenes;
using Assets.CharacterStatsSemder.Scripts.UI.Login;
using Assets.CharacterStatsSemder.Scripts.User;
using Newtonsoft.Json;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.CharacterStatsSemder.Scripts
{
    public class LoginBehaviour : MonoBehaviour
    {
        [SerializeField]
        private LoginUI loginUI = default;

        [SerializeField]
        private AuthenticationUI authenticationUI = default;

        [SerializeField]
        private RegistrationUI registrationUI = default;

        private void Awake()
        {
            Assert.IsNotNull(loginUI);
            Assert.IsNotNull(authenticationUI);
            Assert.IsNotNull(registrationUI);
        }

        private void Start()
        {
            authenticationUI.AuthButtonClicked += OnAuthenticateButtonClicked;
            registrationUI.RegisterButtonClicked += OnRegisterButtonClicked;
        }

        private void OnDestroy()
        {            
            authenticationUI.AuthButtonClicked -= OnAuthenticateButtonClicked;
            registrationUI.RegisterButtonClicked -= OnRegisterButtonClicked;
        }

        private async void OnAuthenticateButtonClicked()
        {
            await Authenticate();
        }

        private async void OnRegisterButtonClicked()
        {
            await Register();
        }

        private async Task Authenticate()
        {
            bool isSuccess = await SendAuthData(authenticationUI.GetUserData);
            if (isSuccess == true)
            {
                SceneLoader.LoadSceneAsync(Scene.LOADER);
            }
        }

        private async Task<bool> SendAuthData(UserData userData)
        {
            string data = JsonConvert.SerializeObject(userData);

            ServerResponse response;

            response = await NetRequester.PostRequest(ApiUris.POST_USER_AUTH, data);
            if (response.IsSuccess == true)
            {
                BearerTokenData bearerTokenData = JsonConvert.DeserializeObject<BearerTokenData>(response.Text);

                NetRequester.BearerToken = bearerTokenData.token;

                return true;
            }
            else 
            {
                return false;
            }
        }

        private async Task Register()
        {
            bool isSuccess = await SendRegisterData(registrationUI.GetUserData);
            if (isSuccess == true)
            {
                loginUI.SwitchScreenToAuth();
            }
        }

        private async Task<bool> SendRegisterData(UserData userData)
        {
            string data = JsonConvert.SerializeObject(userData);

            ServerResponse response;

            response = await NetRequester.PostRequest(ApiUris.POST_USER_REGISTER, data);

            return response.IsSuccess;
        }
    }
}