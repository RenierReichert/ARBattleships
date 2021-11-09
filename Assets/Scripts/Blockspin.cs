using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blockspin : MonoBehaviour
{
	
	public GameObject ship;
	
	protected float tiltAngle = 10.0f, listingShipX = 0, listingShipY = 0;
	protected float timeLeft;
	protected float steeringDirection = 1; //debug purposes
	
	
    // Start is called before the first frame update
    void Start()
    {
		timeLeft = 5;
		// is taken from input, saved here for debug purposes
		
        
    }

    // Update is called once per frame
    void Update()
    {
		TimeUpdate();
		
		
		// Depending on which direction the player is steering (left is positive, right negative), the steeering direction will range from -1 to 1
		
		
		
		listingShipX = tiltAngle * steeringDirection;
	    listingShipY -= steeringDirection * 0.25f;
		
		Quaternion target = Quaternion.Euler(listingShipX,listingShipY,0);
		
		ship.transform.rotation = Quaternion.Slerp(ship.transform.rotation, target, 0.005f );
    }
	
	void TimeUpdate()
	{
		timeLeft -= Time.deltaTime;
		
		//Change direction every 5 seconds, debug purposes
		if(timeLeft < 0)
		{
			steeringDirection = (-steeringDirection);
			timeLeft = 5;
		}
		
		
		
	}
}
