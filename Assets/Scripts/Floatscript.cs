using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Floatscript : MonoBehaviour
{

    public Rigidbody rigidbody;
    protected float depthBeforeSubmerged = 0.05f; //float values (Pun intended)
    protected float displacementAmount = 2f;

    private void FixedUpdate()
    {
        float waveHeight = WaveManager.instance.GetWaveHeight(transform.position.x);

        // If floater is under water
        if (transform.position.y < waveHeight)
        {
            //clamped between 0 and 1, once out of the water it wont be pulled back in, and once fully submerged it wont float any harder if its deeper underwater.
            float displacementMultiplier = Mathf.Clamp01((waveHeight-transform.position.y) / depthBeforeSubmerged) * displacementAmount;

            //The displacement multiplier decides how "hard" the boat is being pushed up
            rigidbody.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), ForceMode.Acceleration );
        }
    }
}
