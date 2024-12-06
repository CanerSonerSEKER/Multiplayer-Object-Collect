using System.IO;
using Photon.Pun;
using UnityEngine;

namespace Game.PlayersScripts
{
    public class PlayerManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private PhotonView _photonView;

        private void Start()
        {
            if (_photonView.IsMine)
            {
                CreateController();
            }            
        }

        private void CreateController()
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerRobot"), Vector3.up * 0.1f, Quaternion.identity);
        }

    }
}