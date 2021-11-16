using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

namespace Battleships.ServerScripts
{
    public class ConnectServer : MonoBehaviourPunCallbacks
    {
        // Start is called before the first frame update
        void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
        }

        public void CreateRoom()
        {
            PhotonNetwork.CreateRoom("ARShips");
        }

        public void JoinRoom()
        {
            PhotonNetwork.JoinRoom("ARShips");
        }
        public override void OnJoinedLobby()
        {
            SceneManager.LoadScene("Twoships");
        }
    }

}