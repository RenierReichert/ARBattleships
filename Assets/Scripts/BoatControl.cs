using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoatControl : MonoBehaviour
{
    public Text txt;
    public Slider sails,wheel;
    public Rigidbody boat;
    public GameObject mast, front;
    protected Vector3 correctedDirection;

    // Update is called once per frame
    void Update()
    {
        //These keys will be replaced with device orientation on a steering wheel on the screen.
        //TODO: put this in OnValueChanged
        float verticalInput = sails.value / 2; //Input.GetAxis("Vertical");
        float horizontalInput = wheel.value;// Input.GetAxis("Horizontal");

        // correctedDirection = transform.forward;

        //  Debug.Log("BoatDirection: " + correctedDirection + "Tranform forward: " + transform.forward);

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
        boat.AddForceAtPosition(boat.transform.forward * 5 , mast.transform.position , ForceMode.Impulse);
       
    }

    public void ShootRightCannon()
    {
        //TODO: I should fix the ship's mesh in blender so forward isnt the right side of the boat.
        boat.AddForceAtPosition(boat.transform.forward * -5, mast.transform.position, ForceMode.Impulse);

    }
}
