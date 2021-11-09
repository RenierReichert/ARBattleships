using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatControl : MonoBehaviour
{

    public Rigidbody boat;
    public GameObject mast;
    protected Vector3 correctedDirection;

    // Update is called once per frame
    void Update()
    {
        //These keys will be replaced with device orientation on a steering wheel on the screen.
        float verticalInput = Input.GetAxis("Vertical");

        //Alternatively: Fix the boat's orientation to world coordinates.
        correctedDirection = transform.forward;

        Debug.Log("BoatDirection: " + correctedDirection + "Tranform forward: " + transform.forward);

        boat.AddForce( (correctedDirection.normalized * verticalInput), ForceMode.Acceleration);

        // To see wether the direction needs to be normalized
        Debug.Log("Boatpower: " + correctedDirection.magnitude);

        


        float horizontalInput = Input.GetAxis("Horizontal");

        boat.AddTorque(transform.up * horizontalInput * 0.15f);

    }
}
