using System.IO;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Game
{
    public class SceneLoadManager : EventListenerMono
    {
        private static SceneLoadManager _instance;

        private void Awake()
        {
            if (_instance)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            _instance = this;
        }

        protected override void RegisterEvents()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (scene.buildIndex == 1)
            {
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero,
                    Quaternion.identity);
            }
        }

        protected override void UnRegisterEvents()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}