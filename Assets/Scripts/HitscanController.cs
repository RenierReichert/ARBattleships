using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitscanController : MonoBehaviour
{

    private bool leftSide;
    private BoatControl parentBoat;

    private void Start()
    {
        switch (this.name)
        {
            case "LeftHitbox":
                leftSide = true;
                break;

            case "RightHitbox": 
                leftSide = false;
                break;

            default:
                break;
        }

        parentBoat = this.GetComponentInParent<BoatControl>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            if (leftSide)
            {
                parentBoat.leftHittable.Add(other.gameObject);
            }
            else
            {
                parentBoat.rightHittable.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.layer == 6)
        {
            if (leftSide)
            {
                parentBoat.leftHittable.Remove(other.gameObject);
            }
            else
            {
                parentBoat.rightHittable.Remove(other.gameObject);
            }
        }
    }
}

