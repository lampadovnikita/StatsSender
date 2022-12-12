using Assets.CharacterStatsSemder.Scripts.Net;
using Assets.CharacterStatsSemder.Scripts.Observable;
using Assets.CharacterStatsSemder.Scripts.Preloaded;
using Assets.CharacterStatsSemder.Scripts.Scenes;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Assets.CharacterStatsSemder.Scripts.UI.Character
{
    public class CharacterUI : MonoBehaviour
    {
        [SerializeField]
        private CharacterAttributesTableUI characterAttributesTableUI = default;

        [SerializeField]
        private CharacterCreationUI characterCreationUI = default;

        [SerializeField]
        private Button logoutButton = default;

        private ObservableValue<bool> isCharacterCreatedObservable;

        public void SwitchToCreationScreen()
        {
            characterCreationUI.gameObject.SetActive(true);
            characterAttributesTableUI.gameObject.SetActive(false);

        }

        public void SwitchToChracterScreen()
        {
            characterAttributesTableUI.gameObject.SetActive(true);
            characterCreationUI.gameObject.SetActive(false);
        }

        private void Awake()
        {
            Assert.IsNotNull(characterAttributesTableUI);
            Assert.IsNotNull(characterCreationUI);
            Assert.IsNotNull(logoutButton);
        }

        private void Start()
        {
            logoutButton.onClick.AddListener(Logout);

            DataStorageBehaviour dataStorageBehaviour = FindObjectOfType<DataStorageBehaviour>();
            Assert.IsNotNull(dataStorageBehaviour);
            
            isCharacterCreatedObservable = dataStorageBehaviour.isCharacterCreatedObservable;
            if (isCharacterCreatedObservable.Value == true)
            {
                SwitchToChracterScreen();
            }
            else
            {
                SwitchToCreationScreen();
            }
        }

        private void OnDestroy()
        {
            logoutButton.onClick.RemoveListener(Logout);
        }

        private void Logout()
        {
            NetRequester.BearerToken = string.Empty;

            Preloader preloader = FindObjectOfType<Preloader>();
            Destroy(preloader.gameObject);

            SceneLoader.LoadSceneAsync(Scene.AUTH);
        }
    }
}