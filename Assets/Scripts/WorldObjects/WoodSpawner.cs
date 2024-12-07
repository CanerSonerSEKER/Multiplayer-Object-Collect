using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

namespace WorldObjects
{
    public class WoodSpawner : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Vector2Int _terrainSize;
        [SerializeField] private List<Wood> _woods = new List<Wood>();
        [SerializeField] private int _initWoodCount = 50;
        [SerializeField] private float _woodSpawnFreq = 1f;
        [SerializeField] private Coroutine _coroutine;
        [SerializeField] private GameObject _woodPrefab;
        [SerializeField] private float _spawnOffSetY = 0.1f;

        private PhotonView _photonView;

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
            
            SpawnAllWoods();

            _coroutine = StartCoroutine(SpawnRoutine());
        }

        private void Start()
        {
            //_photonView.RPC(nameof(SpawnAllWoods), RpcTarget.MasterClient);
            
            //if (PhotonNetwork.IsMasterClient ==  false) return;

        }

        private void SpawnAllWoods()
        {
            //_woods = new List<Wood>();

            if (!_photonView.IsMine) return;

            for (int i = 0; i < _initWoodCount; i++)
            {
                SpawnRandWoods();
            }
        }

        private void SpawnRandWoods()
        {
            float randX = Random.Range(0f, _terrainSize.x);
            float randZ = Random.Range(0f, _terrainSize.y);

            Vector3 newWoodPos = new Vector3(randX, _spawnOffSetY, randZ);

            GameObject newWoodGo = PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefabs", "WoodBox"), newWoodPos, Quaternion.identity);

            Wood newWood = newWoodGo.GetComponent<Wood>();
            
            _woods.Add(newWood);

            newWood.Pickable += OnPickable;
        }

        private void OnPickable(Wood pickedWood)
        {
            Debug.LogWarning($"SpawnerPreRemove : {pickedWood}");
            pickedWood.Pickable -= OnPickable;
            _woods.Remove(pickedWood);
            PhotonNetwork.Destroy(pickedWood.gameObject);

        }
        
        private IEnumerator SpawnRoutine()
        {
            while (true)
            {
                if (_woods.Count < _initWoodCount)
                {
                    SpawnRandWoods();
                }

                yield return new WaitForSeconds(_woodSpawnFreq);
            }
        }
    }
}