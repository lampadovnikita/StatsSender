using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace Assets.CharacterStatsSemder.Scripts.Scenes
{
    public static class SceneLoader
    {
        private static Dictionary<Scene, string> scenesDictionary;

        static SceneLoader()
        {
            scenesDictionary = new Dictionary<Scene, string>() 
            {
                { Scene.AUTH, "AuthScene"},
                { Scene.LOADER, "LoaderScene"},
                { Scene.MAIN, "MainScene"}
            };
        }

        public static void LoadSceneAsync(Scene scene)
        {
            Assert.IsTrue(scenesDictionary.ContainsKey(scene));

            SceneManager.LoadSceneAsync(scenesDictionary[scene]);
        }

        public static void LoadScene(Scene scene)
        {
            Assert.IsTrue(scenesDictionary.ContainsKey(scene));

            SceneManager.LoadScene(scenesDictionary[scene]);
        }
    }
}