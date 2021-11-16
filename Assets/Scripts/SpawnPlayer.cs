using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayer : MonoBehaviour
{

    public GameObject player2;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.Instantiate("Player 2" , player2.transform.position, player2.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
