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
                Debug.Log("OnconnectedToMaster called");
                PhotonNetwork.JoinLobby();
                // PhotonNetwork.ReconnectAndRejoin();
            }
            else
            {
                Debug.Log("OnConnectedToMaster called");
                PhotonNetwork.JoinLobby();
            }
        }
        public override void OnJoinedLobby()
        {
            Debug.Log("OnJoinedLobby called");
            // SceneManager.LoadScene("Twoships");
        }

        public void JoinOrCreateRoom()
        {
            Debug.Log("JoinOrCreateRoom called");

            if (PhotonNetwork.CurrentRoom is null)
            {
                Photon.Realtime.RoomOptions roomOptions = new Photon.Realtime.RoomOptions();
                roomOptions.IsVisible = false;
                roomOptions.MaxPlayers = 2;
                PhotonNetwork.JoinOrCreateRoom("ARShips", roomOptions, typedLobby: default);

                Debug.Log("Name of the current room: " + PhotonNetwork.CurrentRoom);
            }
            else
                Debug.Log("Attempting to join room failed, already in room named: " + PhotonNetwork.CurrentRoom);
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            Debug.Log("OnJoinedRoom called, room name: " + PhotonNetwork.CurrentRoom + " |  Spawning player in");

            PhotonNetwork.Instantiate("Player2", Vector3.zero, Quaternion.identity);
        }


        public void OnDestroy()
        {
            PhotonNetwork.Disconnect();
            Debug.Log(PhotonNetwork.NetworkClientState);
        }
    }

}