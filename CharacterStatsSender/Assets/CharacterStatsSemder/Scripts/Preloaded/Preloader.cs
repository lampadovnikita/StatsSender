using Assets.CharacterStatsSemder.Scripts.Character;
using Assets.CharacterStatsSemder.Scripts.Net;
using Assets.CharacterStatsSemder.Scripts.Observable;
using Assets.CharacterStatsSemder.Scripts.Scenes;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.CharacterStatsSemder.Scripts.Preloaded
{
    public class Preloader : MonoBehaviour
    {
        [SerializeField]
        private GameObject DataStoragePrefab = default;

        public PreloadedData LoadedData { get; set; }

        private DataStorageBehaviour dataStorageBehaviour;

        private async void Awake()
        {
            DontDestroyOnLoad(this);
           
            ServerResponse response;
            do
            {
                response = await NetRequester.GetRequest(ApiUris.GET_PRELOAD_DATA);
            }
            while (response.IsSuccess != true);

            LoadedData = JsonConvert.DeserializeObject<PreloadedData>(response.Text);


            if (dataStorageBehaviour == null)
            {
                dataStorageBehaviour = FindObjectOfType<DataStorageBehaviour>();
                if (dataStorageBehaviour == null)
                {
                    dataStorageBehaviour = Instantiate(DataStoragePrefab).GetComponent<DataStorageBehaviour>();
                    Assert.IsNotNull(dataStorageBehaviour);                    
                }
            }

            CopyDataToStorage();

            SceneLoader.LoadSceneAsync(Scene.MAIN);
        }

        private void CopyDataToStorage()
        {
            dataStorageBehaviour.characterDataObservable = new ObservableValue<CharacterData>();
            dataStorageBehaviour.characterDataObservable.Value = LoadedData.characterData;

            dataStorageBehaviour.isCharacterCreatedObservable = new ObservableValue<bool>();
            dataStorageBehaviour.isCharacterCreatedObservable.Value = LoadedData.isCharacterCreated;
            
            dataStorageBehaviour.expPerLevelBounds = LoadedData.expPerLevelBounds;
        }
    }
}