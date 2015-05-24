using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour 
{
	public GameObject enemy;
	Vector2[] spawnTotal;
	int spawnSlot;
	float spawnTimer;
	float timerScale=1;
	float timeRate;
	float spawnDelay;
	
	void Start () 
	{
		spawnTotal= new Vector2[48];		
		SetSpawnSlots("up");
		SetSpawnSlots("right");
		SetSpawnSlots("down");
		SetSpawnSlots("left");
		
		spawnSlot=0;
		spawnDelay=200;
	}
	
	void SetSpawnSlots(string side)
	{
		switch(side)
		{
		case "up":
			for(float i=-0.1f; i<=1.1f; i+=0.1f)
			{
				spawnTotal[spawnSlot]= Camera.main.ViewportToWorldPoint(new Vector2(i,1.1f));
				spawnSlot++;
			}
			break;
			
		case "right":
			for(float i=1.1f; i>=-0.1f; i-=0.1f)
			{
				spawnTotal[spawnSlot]= Camera.main.ViewportToWorldPoint(new Vector2(1.1f,i));
				spawnSlot++;
			}
			break;
			
		case "down":
			for(float i=1.1f; i>=-0.1f; i-=0.1f)
			{
				spawnTotal[spawnSlot]= Camera.main.ViewportToWorldPoint(new Vector2(i,-0.1f));
				spawnSlot++;
			}
			break;
			
		case "left":
			for(float i=-0.1f; i<=1.1f; i+=0.1f)
			{
				spawnTotal[spawnSlot]= Camera.main.ViewportToWorldPoint(new Vector2(-0.1f,i));
				spawnSlot++;
			}
			break;
		}
	}
	
	void Update () 
	{	
		if(Time.timeScale!=0)
		{
			spawnTimer+=timerScale;
			if(spawnTimer>=spawnDelay)
			{
				spawnSlot= Random.Range(0,spawnTotal.Length);
				Instantiate(enemy, spawnTotal[spawnSlot], transform.rotation);
				spawnTimer=0;				
				timeRate+=4;
				timerScale = Mathf.Log(timeRate);
			}
		}
	}
}
