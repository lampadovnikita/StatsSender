using Assets.CharacterStatsSemder.Scripts.Net;
using Assets.CharacterStatsSemder.Scripts.UI.Character;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.CharacterStatsSemder.Scripts.Character
{
    public class CharacterCreationBehaviour : MonoBehaviour
    {
        [SerializeField]
        private CharacterCreationUI characterCreationUI = default;

        [SerializeField]
        private CharacterUI characterUI = default;

        private DataStorageBehaviour dataStorageBehaviour;

        private void Awake()
        {
            Assert.IsNotNull(characterCreationUI);
            Assert.IsNotNull(characterUI);

            dataStorageBehaviour = FindObjectOfType<DataStorageBehaviour>();
            Assert.IsNotNull(dataStorageBehaviour);
        }

        private void Start()
        {
            characterCreationUI.CreationSubmited += SendCharacterCreationData;
        }

        private void OnDestroy()
        {            
            characterCreationUI.CreationSubmited -= SendCharacterCreationData;
        }

        private async void SendCharacterCreationData(CharacterData characterData)
        {
            string data = JsonConvert.SerializeObject(characterData);

            ServerResponse serverResponse = await NetRequester.PostRequest(ApiUris.POST_CHARACTER_CREATE, data);
            if (serverResponse.IsSuccess == true)
            {
                dataStorageBehaviour.characterDataObservable.Value = characterData;
                dataStorageBehaviour.isCharacterCreatedObservable.Value = true;

                characterUI.SwitchToChracterScreen();
            }
        }
    }
}