using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class BoatControl : MonoBehaviourPunCallbacks
{
    public Text debugText;
    public Slider sails,wheel;
    public Button leftCannon, rightCannon;
    public Rigidbody boat, cannonBall;
    public GameObject mast;
    public GameObject portSideCannon, starBoardCannon;
    protected Vector3 sailingDirection;
    private float verticalInput, horizontalInput;

    private void Start()
    {
        sails = GameObject.Find("Canvas/Sailslider").GetComponent<Slider>();
        wheel = GameObject.Find("Canvas/ToBeWheel").GetComponent<Slider>();
        leftCannon = GameObject.Find("Canvas/LeftCannon").GetComponent<Button>();
        rightCannon = GameObject.Find("Canvas/RightCannon").GetComponent<Button>();
        

        leftCannon.onClick.AddListener(ShootLeftCannon);
        rightCannon.onClick.AddListener(ShootRightCannon);
    }
    void Update()
    {
       // debugText.text = $"Vertical: {verticalInput} Horizontal: {horizontalInput}";
    }
    private void FixedUpdate()
    {
       // sails.onValueChanged.AddListener();
        verticalInput = sails.value; //Input.GetAxis("Vertical");
        horizontalInput = wheel.value;// Input.GetAxis("Horizontal");

        sailingDirection = transform.forward * verticalInput;

        /*
        Debug.Log($"Sails: {sails.name} {sails.value}");
        Debug.Log($"Wheel: {wheel.name} {wheel.value}");
        Debug.Log(correctedDirection);
        */

        //Boat should not be able to sail itself downwards or upwards strongly
        sailingDirection.y /= 10;
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

        //TODO: if aimed right, call HitLeftCannnon();
        Rigidbody cannonBallClone;
        cannonBallClone = PhotonNetwork.Instantiate("Cannonball", portSideCannon.transform.position, Quaternion.identity).GetComponent<Rigidbody>();
        cannonBallClone.velocity = (boat.transform.right * -5);
        Destroy(cannonBallClone.gameObject, 5);
    }
    private void HitLeftCannon()
    {
        Rigidbody cannonBallClone;
        cannonBallClone = Instantiate(cannonBall, portSideCannon.transform.position, Quaternion.identity);
        cannonBallClone.velocity = (boat.transform.right * -8);
    }

    private void ShootRightCannon()
    {
        if (!this.photonView.IsMine)
            return;
        //TODO: I should fix the ship's mesh in blender so forward isnt the right side of the boat.
        boat.AddForceAtPosition(boat.transform.right * -3, mast.transform.position, ForceMode.Impulse);
        Rigidbody cannonBallClone;
        cannonBallClone = PhotonNetwork.Instantiate("Cannonball", starBoardCannon.transform.position, Quaternion.identity).GetComponent<Rigidbody>(); ;
        cannonBallClone.velocity = (boat.transform.right * 5);
        Destroy(cannonBallClone.gameObject , 5);
    }

    private void HitRightCannon()
    {
        Rigidbody cannonBallClone;
        cannonBallClone = Instantiate(cannonBall, portSideCannon.transform.position, Quaternion.identity);
        cannonBallClone.velocity = (boat.transform.right * 8);
    }
    private void OnTriggerEnter(Collider cannonHit)
    {
        Debug.Log("Collided with: " + cannonHit.gameObject.name + boat.position);

        //Check if we collide with a cannonball, remove cannonball
        if (cannonHit.gameObject.layer == 7)
        {
            boat.AddForceAtPosition(cannonHit.GetComponent<Rigidbody>().velocity * 6 + new Vector3(0, 30, 0), boat.transform.position, ForceMode.Impulse);
            boat.AddTorque(boat.transform.forward * 60, ForceMode.Impulse);
            Destroy(cannonHit, 0.2f);
        }
        else if (cannonHit.gameObject.layer == 6)
        {
            boat.AddForceAtPosition((cannonHit.GetComponent<Rigidbody>().position - boat.position) * -500, cannonHit.transform.position, ForceMode.Force);
        }
    }
}
