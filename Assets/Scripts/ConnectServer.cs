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
            if (PhotonNetwork.IsConnected)
            {
                Debug.Log("Reconnected");
                PhotonNetwork.ReconnectAndRejoin();
            }
            else
            {
                Debug.Log("OnConnectedToMaster called");
                PhotonNetwork.JoinLobby();
            }
        }

        public void CreateRoom()
        {
            Photon.Realtime.RoomOptions roomOptions = new Photon.Realtime.RoomOptions();
            roomOptions.IsVisible = false;
            roomOptions.MaxPlayers = 2;
            PhotonNetwork.JoinOrCreateRoom("ARShips", roomOptions, typedLobby:default);
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("OnJoinedLobby called");
           // SceneManager.LoadScene("Twoships");
        }

        public void OnDestroy()
        {
            PhotonNetwork.Disconnect();
            Debug.Log(PhotonNetwork.NetworkClientState);
        }
    }

}