using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class BoatControl : MonoBehaviourPunCallbacks
{
    public Text debugText;
    
    public Button leftCannon, rightCannon;
    public Rigidbody boat, cannonBall;
    public GameObject mast;
    public GameObject portSideCannon, starBoardCannon;
    public List<GameObject> leftHittable, rightHittable;
    protected Vector3 sailingDirection;
    private Slider sails, wheel;
    private float verticalInput, horizontalInput;
    private Material myBoat;

    private void Start()
    {
        sails = GameObject.Find("Canvas/Sailslider").GetComponent<Slider>();
        wheel = GameObject.Find("Canvas/ToBeWheel").GetComponent<Slider>();
        leftCannon = GameObject.Find("Canvas/LeftCannon").GetComponent<Button>();
        rightCannon = GameObject.Find("Canvas/RightCannon").GetComponent<Button>();

        leftCannon.onClick.AddListener(ShootLeftCannon);
        rightCannon.onClick.AddListener(ShootRightCannon);
        Debug.Log(GetComponent<MeshRenderer>().material);

        myBoat = Resources.Load<Material>("Materials/pinktexture");
        Debug.Log(myBoat);

        if (this.photonView.IsMine)
        {
            GetComponent<MeshRenderer>().material = (myBoat);
            Debug.Log(GetComponent<MeshRenderer>().material);
        }
    }
    private void FixedUpdate()
    {
        verticalInput = sails.value; 
        horizontalInput = wheel.value;

        sailingDirection = transform.forward * verticalInput;


        //Boat should not be able to sail itself downwards or upwards strongly
        sailingDirection.y /= 8;
        boat.AddForce(sailingDirection, ForceMode.Acceleration);

        boat.AddTorque(transform.up * horizontalInput * 0.5f);
    }

    private void UpdateSails()
    {
        verticalInput = sails.value;
    }

    private void UpdateWheel()
    {
        horizontalInput = wheel.value;
        // TODO: Wheel animation
    }

    private void ShootLeftCannon()
    {
        if (!this.photonView.IsMine)
            return;

        boat.AddForceAtPosition(boat.transform.right * 3 , mast.transform.position , ForceMode.Impulse);

        
        if (leftHittable.Count > 0)
            HitLeftCannon();
        else
        {
            Rigidbody cannonBallClone;
            cannonBallClone = PhotonNetwork.Instantiate("Cannonball", portSideCannon.transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            Vector3 direction = (boat.transform.right * -5);
            direction.y += 2;
            cannonBallClone.velocity = direction;
            Destroy(cannonBallClone.gameObject, 5);
        }
    }
    private void HitLeftCannon()
    {
        Rigidbody cannonBallClone;
        cannonBallClone = PhotonNetwork.Instantiate("Cannonball", portSideCannon.transform.position, Quaternion.identity).GetComponent<Rigidbody>();
        Vector3 autoAim = (leftHittable[0].transform.position - portSideCannon.transform.position);
        float distance = autoAim.magnitude;
        float secondsNeeded = distance / (-Physics.gravity.y);
        autoAim = autoAim.normalized;
        autoAim.y += secondsNeeded/2;
        cannonBallClone.velocity = autoAim *5;
        Destroy(cannonBallClone.gameObject, 5);
    }

    private void ShootRightCannon()
    {
        if (!this.photonView.IsMine)
            return;

        boat.AddForceAtPosition(boat.transform.right * -3, mast.transform.position, ForceMode.Impulse);
        Debug.Log(GetComponent<MeshRenderer>().material);
        if (rightHittable.Count > 0)
            HitRightCannon();
        else
        {
            Rigidbody cannonBallClone;
            cannonBallClone = PhotonNetwork.Instantiate("Cannonball", starBoardCannon.transform.position, Quaternion.identity).GetComponent<Rigidbody>(); ;
            Vector3 direction = (boat.transform.right * 5);
            direction.y += 2;
            cannonBallClone.velocity = direction;
            Destroy(cannonBallClone.gameObject, 5);
        }
    }

    private void HitRightCannon()
    {
        Rigidbody cannonBallClone;
        cannonBallClone = PhotonNetwork.Instantiate("Cannonball", starBoardCannon.transform.position, Quaternion.identity).GetComponent<Rigidbody>();
        Vector3 autoAim = (rightHittable[0].transform.position - starBoardCannon.transform.position);
        float distance = autoAim.magnitude;
        float secondsNeeded = distance / (-Physics.gravity.y);
        autoAim = autoAim.normalized;
        autoAim.y += secondsNeeded / 2;
        cannonBallClone.velocity = autoAim * 5;
        Destroy(cannonBallClone.gameObject, 5);
    }
    private void OnTriggerEnter(Collider cannonHit)
    {
        // Debug.Log("Collided with: " + cannonHit.gameObject.name + cannonHit.gameObject.layer);


        switch (cannonHit.gameObject.layer)
        {
            case 8:
                break;
            case 7:
                boat.AddForceAtPosition(cannonHit.GetComponent<Rigidbody>().velocity * 6 + new Vector3(0, 30, 0), boat.transform.position, ForceMode.Impulse);
                boat.AddTorque(boat.transform.forward * 60, ForceMode.Impulse);
                Destroy(cannonHit, 0.2f);
                break;

            case 6:
                boat.AddForceAtPosition((cannonHit.GetComponent<Rigidbody>().position - boat.position) * -500, cannonHit.transform.position, ForceMode.Force);
                break;
        }
    }

}
