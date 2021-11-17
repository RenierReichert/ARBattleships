using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class BoatControl : MonoBehaviourPunCallbacks
{
    public Text debugText;
    public Slider sails,wheel;
    public Rigidbody boat, cannonBall;
    public GameObject mast, front;
    public GameObject portSideCannon, starBoardCannon;
    protected Vector3 correctedDirection;
    private float verticalInput, horizontalInput;

    private void Start()
    {
        sails = GameObject.Find("Canvas/Sailslider").GetComponent<Slider>();
        wheel = GameObject.Find("Canvas/ToBeWheel").GetComponent<Slider>();
    }

    public void UpdateInput()
    {
    }

    void Update()
    {
       // debugText.text = $"Vertical: {verticalInput} Horizontal: {horizontalInput}";
    }

    private void FixedUpdate()
    {

        verticalInput = sails.value; //Input.GetAxis("Vertical");
        horizontalInput = wheel.value;// Input.GetAxis("Horizontal");

        // TODO: Fix the mesh
        correctedDirection = (front.transform.position - transform.position).normalized * verticalInput;

        Debug.Log($"Sails: {sails.name} {sails.value}");
        Debug.Log($"Wheel: {wheel.name} {wheel.value}");
        Debug.Log($"{front.transform.position} frond, middle {transform.position} -boatcontrol");
        Debug.Log(correctedDirection);
        //Boat should not be able to sail itself downwards or upwards strongly
        correctedDirection.y = (correctedDirection.y / 10);
        boat.AddForce(correctedDirection, ForceMode.Acceleration);

        boat.AddTorque(transform.up * horizontalInput * 0.5f);
    }

    public void ShootLeftCannon()
    {
        //TODO: I should fix the ship's mesh in blender so forward isnt the right side of the boat.
        boat.AddForceAtPosition(boat.transform.forward * 3 , mast.transform.position , ForceMode.Impulse);


        //TODO: if aimed right, call HitLeftCannnon();
        Rigidbody cannonBallClone;
        cannonBallClone = Instantiate(cannonBall, portSideCannon.transform.position, Quaternion.identity);
        cannonBallClone.velocity = (boat.transform.forward * -5);
        Destroy(cannonBallClone.gameObject, 5);
    }

    protected void HitLeftCannon()
    {
        Rigidbody cannonBallClone;
        cannonBallClone = Instantiate(cannonBall, portSideCannon.transform.position, Quaternion.identity);
        cannonBallClone.velocity = (boat.transform.forward * -8);
    }

    private void OnTriggerEnter(Collider cannonHit)
    {
        // Debug.Log("Collided with: " + cannonHit.gameObject.name + boat.position);

        //Check if we collide with a cannonball, remove cannonball
        if (cannonHit.gameObject.layer == 7)
        {
            boat.AddForceAtPosition(cannonHit.GetComponent<Rigidbody>().velocity  * 6 + new Vector3(0, 30, 0), boat.transform.position, ForceMode.Impulse);
            boat.AddTorque(boat.transform.right * -60, ForceMode.Impulse);
            Destroy(cannonHit, 0.2f);
        }
        else if(cannonHit.gameObject.layer == 6)
        {
            boat.AddForceAtPosition( (cannonHit.GetComponent<Rigidbody>().position - boat.position) * -500, cannonHit.transform.position, ForceMode.Force);
        }
    }

    public void ShootRightCannon()
    {
        //TODO: I should fix the ship's mesh in blender so forward isnt the right side of the boat.
        boat.AddForceAtPosition(boat.transform.forward * -3, mast.transform.position, ForceMode.Impulse);
        Rigidbody cannonBallClone;
        cannonBallClone = Instantiate(cannonBall, starBoardCannon.transform.position, Quaternion.identity);
        cannonBallClone.velocity = (boat.transform.forward * 5);
        Destroy(cannonBallClone.gameObject , 5);
    }
    
}
