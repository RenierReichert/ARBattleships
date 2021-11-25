using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Floatscript : MonoBehaviour
{
    // Float properties. Puns are intended.

    public float displacementAmount = 2f;
    protected Rigidbody parentBoatRigidbody;
    private float depthBeforeSubmerged = 0.08f;    
    private int floaterCount = 4;
    private float waterDrag = 3f, waterAngularDrag = 0.5f;

    public void Start()
    {
        parentBoatRigidbody = GetComponentInParent<Rigidbody>();

        // Regular-sized boat has a scale of 0.05f
        depthBeforeSubmerged *= transform.parent.localScale.y / 0.05f;

        /* Make the mast float slightly so the ship wont be stuck upside down,
         * but its less affected by gravity so it doesnt topple easily */
        if (gameObject.name == "Mast")
        {
            depthBeforeSubmerged *= 2;
            floaterCount = 10;
        }
    }


    private void FixedUpdate()
    {
        // Apply gravity to each floater, divided by floater count.
        parentBoatRigidbody.AddForceAtPosition(Physics.gravity / floaterCount, transform.position, ForceMode.Acceleration);
        float waveHeight = WaveManager.instance.GetWaveHeight(transform.position.x);

        if (transform.position.y < waveHeight)
        {
            float displacementMultiplier = Mathf.Clamp01((waveHeight-transform.position.y) / depthBeforeSubmerged) * displacementAmount;
            Vector3 waterNormal = WaveManager.instance.GetWaterNormal(transform.position.x);

            //Bouyancy, waterdrag, and angular water drag applications.
            parentBoatRigidbody.AddForceAtPosition(new Vector3(waterNormal.x, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f),transform.position, ForceMode.Acceleration );
            parentBoatRigidbody.AddForce(displacementMultiplier * -parentBoatRigidbody.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            parentBoatRigidbody.AddTorque(displacementMultiplier * -parentBoatRigidbody.angularVelocity * waterAngularDrag* Time.fixedDeltaTime, ForceMode.VelocityChange);
            
        }
    }
}
