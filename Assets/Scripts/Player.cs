using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	public HUD hud;
	public GameObject Cube;
	public CharacterController controller;
	public Transform weaponPos;
	public Transform weaponAltPos;
	public Transform sparkPos;
	public Transform sparkAltPos;
	public GameObject laser;
	public GameObject laserAlt;
	public GameObject bullet;
	public Texture2D crosshair;
	public GameObject fireSpark;
	public Material shieldMat;
	public GameObject deathParticle;
	bool ShieldSoundPlayed;
	
	int inputID;
	Vector2 startPos;
	Vector2 move;
	float speed=0.025f;
	float speedMax=0.3f;
	Vector2 look;
	Quaternion lastRotation;
	Vector2 mousePos;
	bool triggerPressed;
	bool fireCooldown;
	int cooldownTimer;
	bool altFire;
	int altTimer;
	bool respawning;
	bool respawnPartial;
	Material defaultMat;
	int respawnTimer;
	
	//Input variables
	string horizontal;
	string vertical;
	string lookX;
	string lookY;
	string fire;
	string fireButton;
	string pause;
	
	public void Setup(int inputUsed, Material color)
	{
		inputID=inputUsed;
		defaultMat=color;
		Cube.GetComponent<Renderer>().material=defaultMat;
		laser.GetComponent<Renderer>().material=defaultMat;
		laserAlt.GetComponent<Renderer>().material=defaultMat;
	}
	
	void Start () 
	{	
		Static.playersAlive++;
		startPos=Cube.transform.position;
		
		//Set inputs based on player ID
		if(inputID==0) //Mouse and keyboard
		{
			horizontal="Horizontal";
			vertical="Vertical";
			fireButton="Fire";
			pause="Pause";
		}
		else //Gamepad
		{
			horizontal="Joy"+inputID+" Horizontal";
			vertical="Joy"+inputID+" Vertical";
			lookX="Joy"+inputID+" Look X";
			lookY="Joy"+inputID+" Look Y";
			fire="Joy"+inputID+" Fire";
			fireButton="Joy"+inputID+" Fire Button";
			pause="Joy"+inputID+" Pause";
		}
	}
	
	
	void Update () 
	{
		//Makes sure Z position is zero
		if(transform.position.z!=0) 
			transform.Translate(new Vector3(0,0,-transform.position.z));
		
		//Movement speed
		if(Input.GetAxisRaw(horizontal)!=0)
		{
			if(move.x<speedMax && move.x>-speedMax) move.x+=Input.GetAxisRaw(horizontal)*speed;
		}
		else if (move.x!=0) move.x*=1-speed*2;
		else move.x=0;
		
		if(Input.GetAxisRaw(vertical)!=0)
		{
			if(move.y<speedMax && move.y>-speedMax) move.y+=Input.GetAxisRaw(vertical)*speed;
		}
		else if (move.y!=0) move.y*=1-speed*2;
		else move.y=0;
		
		if(Time.timeScale!=0)
		{
			//Move around
			if(!respawning) controller.Move(move);
		
			//Look at mouse cursor. If a gamepad, use right stick to look.
			if(inputID==0)
			{
				mousePos= Camera.main.ScreenToWorldPoint(Input.mousePosition);
				Cube.transform.LookAt(mousePos,Vector3.back);
				Cube.transform.Rotate(new Vector3(90,0,0));
			}
			else
			{			
				float deadzone = 0.5f;
				look = new Vector2(Input.GetAxis(lookX), Input.GetAxis(lookY));
				
				if(look.magnitude < deadzone)
				{
					look = Vector2.zero;
					Cube.transform.rotation=lastRotation;
				}
				else
				{
					Cube.transform.rotation=Quaternion.LookRotation(look,Vector3.back);
					Cube.transform.Rotate(new Vector3(90,0,0));
					lastRotation = Cube.transform.rotation;
				}
			}
			
			//Fire weapon
			if(!fireCooldown)
			{
				if(!respawning || respawnPartial)
				{
					if(inputID!=0)
					{			
						if(Input.GetAxisRaw(fire)!=0 || Input.GetButtonDown(fireButton)) 
						{
							if(!triggerPressed) 
							{
								FireWeapon();
								altFire=hud.specialMode;
							}
							triggerPressed=true;
						}
						else triggerPressed=false;
					}
					else if(Input.GetButtonDown(fireButton)) 
					{
						FireWeapon();
						altFire=hud.specialMode;
					}
				}
			}			
			//Cool down weapon before firing again
			else
			{
				cooldownTimer++;
				if(cooldownTimer>=20)
				{
					fireCooldown=false;
					cooldownTimer=0;
				}
			}
			
			//Fire alt weapon after some time
			if(altFire)
			{
				altTimer++;
				if(altTimer>=10)
				{
					FireWeapon(true);
					altTimer=0;
					altFire=false;
				}
			}
			
			//Respawns when on versus (see Kill function)
			if(respawning)
			{
				respawnTimer++;
				Cube.transform.position=startPos;
				if(respawnTimer>=180 && !respawnPartial)
				{
					transform.localScale=Vector3.one;
					hud.lifeCounter=hud.lifeMax;
					GetComponent<AudioSource>().Play();
					respawnPartial=true;
				}
				if(respawnTimer>=300)
				{
					Cube.GetComponent<Renderer>().material=defaultMat;
					Cube.GetComponent<Collider>().enabled=true;
					GetComponent<AudioSource>().Stop();
					respawning=false;
					respawnPartial=false;
					respawnTimer=0;
				}
			}
		}//End of pause check
				
		//Shows alt weapon when on special mode
		if(hud.specialMode) weaponAltPos.localScale=weaponPos.localScale;
		else weaponAltPos.localScale=Vector3.zero;
		
		//Change cube color when on shield mode
		if(!Static.isPvP)
		{
			if(hud.shieldMode) 
			{
				Cube.GetComponent<Renderer>().material=shieldMat;
				if(!ShieldSoundPlayed) GetComponent<AudioSource>().Play();
				ShieldSoundPlayed=true;
			}
			else
			{
				Cube.GetComponent<Renderer>().material=defaultMat;
				ShieldSoundPlayed=false;
				GetComponent<AudioSource>().Stop();
			}
		}
		
		//Pause game
		if(Input.GetButtonDown(pause))
		{
			if(Time.timeScale!=0) Static.PauseGame();
			else Static.UnPauseGame();
		}
	}
	
	
	void FireWeapon(bool isAltFire=false)
	{
		Transform firingPos;
		Transform sparkingPos;
				
		//Are you shooting the secondary weapon?
		if(isAltFire) 
		{
			firingPos=weaponAltPos;
			sparkingPos=sparkAltPos;
		}
		else 
		{
			firingPos=weaponPos;
			sparkingPos=sparkPos;
			fireCooldown=true;
		}
		
		//Create a spark and parent it to the weapon
		GameObject spark = (GameObject)Instantiate(fireSpark, sparkingPos.position, sparkingPos.rotation);
		spark.transform.parent = firingPos;
		
		//Instantiate bullet
		Instantiate(bullet, firingPos.position, firingPos.rotation);
		
		//Creates a raycast from weapon
		Ray bulletRay= new Ray(firingPos.position, firingPos.up);
		RaycastHit hit;
		if(Physics.Raycast(bulletRay, out hit))
		{
			switch(hit.collider.tag) 
			{
			case "Enemy": 
				hit.collider.SendMessage("Destruct");
				hud.score++;
				hud.AddCombo(10);
				break;
			case "Player":
				if(Static.isPvP)
				{
					hit.collider.SendMessageUpwards("Kill");
					hud.score++;
				}
				break;
			}
		}
	}
	
	void Kill()
	{
		if(!hud.shieldMode) hud.lifeCounter--;
		
		if(hud.lifeCounter<=0)
		{
			//Generate death particles
			GameObject deathSpark= (GameObject)Instantiate(deathParticle,Cube.transform.position,Cube.transform.rotation);
			deathSpark.GetComponent<Renderer>().material=defaultMat;
			
			// If on versus, prepare to respawn
			if(Static.isPvP)
			{
				Cube.GetComponent<Collider>().enabled=false;
				transform.localScale=Vector3.zero;
				Cube.GetComponent<Renderer>().material=shieldMat;
				respawning=true;
			}
			//If not, kill the player
			else
			{
				Static.scoreTotal+=hud.score;
				Static.playersAlive--;
				Destroy(gameObject);
			}
		}
	}
	
	void OnGUI()
	{
		//Adds crosshair for mouse player
		if(inputID==0)
		{
			Vector2 crosshairPos;
			crosshairPos.x= Input.mousePosition.x - crosshair.width/2;
			crosshairPos.y= -Input.mousePosition.y + Screen.height - crosshair.height/2;
			GUI.DrawTexture(new Rect(crosshairPos.x,crosshairPos.y,crosshair.width,crosshair.height), crosshair);
		}
	}
}
