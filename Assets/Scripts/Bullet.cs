using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{	
	Vector2 origin;
	float distance;
	
	void Start()
	{
		//Gets starting position and fires a ray at the hit point
		origin=transform.position;
		RaycastHit hit;
		if(Physics.Raycast(origin, transform.up, out hit))
		{
			distance = Vector2.Distance(origin,hit.point);
		}
	}
	
	void Update () 
	{
		//Moves forward and updates distance
		transform.Translate(Vector3.up*10);
		distance-=10;
		
		//Destroys bullet when it reaches hit point
		if(distance<=0) Destroy(gameObject);		
	}
}
