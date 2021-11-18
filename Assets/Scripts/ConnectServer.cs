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
        private Text connectButtonText;
        private Coroutine loadingAnimation;

        // Start is called before the first frame update
        void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
            connectButtonText = GameObject.Find("Canvas/CreateRoomButton/Text").GetComponent<Text>();
            loadingAnimation = StartCoroutine(LoadAnimation());

        }

        public override void OnConnectedToMaster()
        {
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinLobby();
                // PhotonNetwork.ReconnectAndRejoin();
            }
            else
            {
                Debug.Log("Something has gone wrong");
            }
        }
        public override void OnJoinedLobby()
        {
            Debug.Log("OnJoinedLobby called");
            StopCoroutine(loadingAnimation);
            connectButtonText.text = "Join\nmultiplayer";
            // SceneManager.LoadScene("Twoships");
        }

        public void JoinOrCreateRoom()
        {
            Debug.Log("JoinOrCreateRoom called");

            if (PhotonNetwork.CurrentRoom is null)
            {
                Photon.Realtime.RoomOptions roomOptions = new Photon.Realtime.RoomOptions();
                roomOptions.IsVisible = false;
                roomOptions.MaxPlayers = 4;
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

        private IEnumerator LoadAnimation()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.25f);

                if (connectButtonText.text.Length > 13)
                    connectButtonText.text = "Connecting";
                else
                    connectButtonText.text += ".";
            }
        }
    }

}