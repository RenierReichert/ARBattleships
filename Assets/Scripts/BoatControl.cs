using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoatControl : MonoBehaviour
{
    public Text txt;
    public Slider sails,wheel;
    public Rigidbody boat, cannonBall;
    public GameObject mast, front;
    public GameObject portSideCannon, starBoardCannon;
    protected Vector3 correctedDirection;

    private void Start()
    {
        // Ignore collissions with the water plane here. For some reason it cant be disables using layers.
    }
    // Update is called once per frame
    void Update()
    {
        //These keys will be replaced with device orientation on a steering wheel on the screen.
        //TODO: put this in OnValueChanged
        float verticalInput = sails.value / 2; //Input.GetAxis("Vertical");
        float horizontalInput = wheel.value;// Input.GetAxis("Horizontal");



        correctedDirection = ( (front.transform.position - transform.position).normalized * verticalInput / 5);

        //Boat should not be able to sail itself downwards or upwards strongly
        correctedDirection.y = (correctedDirection.y / 10); 


        // Hack-y way of doing this, but its faster than rotating using sin and cos, since transform.forward is sideways for the boat
        boat.AddForce(correctedDirection , ForceMode.Acceleration);



        float waveHeight = WaveManager.instance.GetWaveHeight(transform.position.x);
        txt.text = "Under waves: " + (transform.position.y<waveHeight) + "\n Boat pos: " + transform.position.y + "\n Wave position: " + waveHeight;        
        boat.AddTorque(transform.up * horizontalInput * 0.2f);

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
        Debug.Log("Collided with: " + cannonHit.gameObject.name + boat.position);

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
           // cannonHit.attachedRigidbody.AddForceAtPosition((boat.position - cannonHit.GetComponent<Rigidbody>().position  ) * -50, cannonHit.transform.position, ForceMode.Force);
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
