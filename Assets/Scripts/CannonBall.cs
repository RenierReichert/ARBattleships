using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    protected Rigidbody rb;
    // Update is called once per frame
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        other.attachedRigidbody.AddForceAtPosition(rb.velocity, rb.transform.position, ForceMode.Impulse);
        Destroy(this, 1f);
    }
}
