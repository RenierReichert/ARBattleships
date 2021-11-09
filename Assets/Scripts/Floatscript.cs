using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Floatscript : MonoBehaviour
{

    public Rigidbody rigidbody;
    protected float depthBeforeSubmerged = 0.08f; //float values (Pun intended)
    protected float displacementAmount = 4f;
    protected int floaterCount = 4;
    protected float waterDrag = 2f, waterAngularDrag = 0.5f;
    

    private void FixedUpdate()
    {
        rigidbody.AddForceAtPosition(Physics.gravity / floaterCount, transform.position, ForceMode.Acceleration);
        
        float waveHeight = WaveManager.instance.GetWaveHeight(transform.position.x) ;

       //  Debug.Log("Wave cos position:" + WaveManager.instance.GetWaveHorizontal(transform.position.x));
       //  Debug.Log("Current position is" + transform.position.y + " And current waveheight is" + waveHeight);

        // If floater is under water
        if (transform.position.y < waveHeight)
        {
            //clamped between 0 and 1, once out of the water it wont be pulled back in, and once fully submerged it wont float any harder if its deeper underwater.
            float displacementMultiplier = Mathf.Clamp01((waveHeight-transform.position.y) / depthBeforeSubmerged) * displacementAmount;


            //The displacement multiplier decides how "hard" the boat is being pushed up. 

            //TODO: Replace new vector3 with the normals of the waves

            Vector3 waterNormal = WaveManager.instance.GetWaterNormal(transform.position.x);

            Debug.Log(waterNormal);
            // rigidbody.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f),transform.position, ForceMode.Acceleration );
             rigidbody.AddForceAtPosition(new Vector3(waterNormal.x, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f),transform.position, ForceMode.Acceleration );

           // rigidbody.AddForceAtPosition(waterNormal * displacementMultiplier, transform.position, ForceMode.Acceleration);



            //To avoid the ships making backflips
            rigidbody.AddForce(displacementMultiplier * -rigidbody.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);

            //Addforce will make the whole ship move, use torque to apply a force to make it rotate
            rigidbody.AddTorque(displacementMultiplier * -rigidbody.angularVelocity * waterAngularDrag* Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }
}
