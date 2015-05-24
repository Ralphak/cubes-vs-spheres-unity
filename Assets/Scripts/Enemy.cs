using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
	GameObject[] totalPlayers;
	int random;
	float speed;
	Quaternion lastRotation;
	Quaternion nextRotation;
	public GameObject explosion;
	
	void Start () 
	{
		//Pick a random player to chase, assuming theres at least one
		if(Static.playersAlive!=0)
		{
			ChoosePlayer();
			transform.LookAt(totalPlayers[random].transform, Vector3.back);
		}
		else transform.LookAt(Vector3.zero, Vector3.back);
		
		transform.Rotate(new Vector3(90,0,0));
	}
	
	void ChoosePlayer()
	{
		totalPlayers = GameObject.FindGameObjectsWithTag("Player");
		random = Random.Range(0,totalPlayers.Length);
	}
	
	void Update () 
	{
		if(Static.playersAlive!=0) 
		{
			//Get current position (as last) and next one
			lastRotation=transform.rotation;
			if(totalPlayers[random]==null) ChoosePlayer();
			transform.LookAt(totalPlayers[random].transform, Vector3.back);
			transform.Rotate(new Vector3(90,0,0));
			nextRotation=transform.rotation;
			transform.rotation=lastRotation;
			
			//Rotates from current position to next one
			transform.rotation= Quaternion.Slerp(lastRotation,nextRotation,0.12f);
		}
		
		//Move forward
		if(Time.timeScale!=0)
		{
			if(speed<0.35f) speed+=0.0025f;
			transform.Translate(new Vector2(0,speed));
		}
	}
	
	
	void OnCollisionEnter(Collision other)
	{
		switch(other.collider.tag)
		{
		case "Player":
			Destruct();
			other.collider.SendMessageUpwards("Kill");
			break;
		case "Wall":
			Physics.IgnoreCollision(GetComponent<Collider>(),other.collider);
			break;
		}
	}
	
	
	public void Destruct()
	{
		Instantiate(explosion,transform.position,transform.rotation);
		Destroy(gameObject);
	}
}
