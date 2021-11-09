using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatControl : MonoBehaviour
{

    public Rigidbody boat;
    public GameObject mast, front;
    protected Vector3 correctedDirection;

    // Update is called once per frame
    void Update()
    {
        //These keys will be replaced with device orientation on a steering wheel on the screen.
        float verticalInput = Input.GetAxis("Vertical");


        // correctedDirection = transform.forward;

        //  Debug.Log("BoatDirection: " + correctedDirection + "Tranform forward: " + transform.forward);

        correctedDirection = ( (front.transform.position - transform.position).normalized * verticalInput / 5);

        //Boat should not be able to sail itself downwards or upwards strongly
        correctedDirection.y = (correctedDirection.y / 10); 


        // Hack-y way of doing this, but its faster than rotating using sin and cos, since transform.forward is sideways for the boat
        boat.AddForce(correctedDirection , ForceMode.Acceleration);

        

        


        float horizontalInput = Input.GetAxis("Horizontal");

        boat.AddTorque(transform.up * horizontalInput * 0.2f);

    }
}
