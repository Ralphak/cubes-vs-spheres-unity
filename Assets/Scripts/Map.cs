using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour 
{
	public GameObject wallBase;
	Vector2 screenSize;
	
	public GameObject playerModel;
	public Material[] playerColor;
	Vector2[] spawnSpot = new Vector2[4];
	bool gameOver;
	public GUISkin skin;
	public Texture2D iconBack;
	int frameCounter;
	bool UploadedToKongregate;
	int timeSeconds;
	int timeMinutes;
	int secondsPassed;
	
	void Start () 
	{
		//Stop playing background music
		GameObject BGM = GameObject.Find("BGM(Clone)");
		if(BGM!=null) Destroy(BGM);
		
		screenSize= Camera.main.ViewportToWorldPoint(new Vector2(1.3f,1.3f));
		
		spawnSpot[0]= new Vector2(0.3f, 0.7f);
		spawnSpot[1]= new Vector2(0.7f, 0.7f);
		spawnSpot[2]= new Vector2(0.3f, 0.3f);
		spawnSpot[3]= new Vector2(0.7f, 0.3f);
		for(int i=0; i<spawnSpot.Length; i++) spawnSpot[i]=Camera.main.ViewportToWorldPoint(spawnSpot[i]);
		
		CreateWall(new Vector2(0.5f,0.9f), new Vector3(screenSize.x,0,0));
		CreateWall(new Vector2(0.5f,0.1f), new Vector3(screenSize.x,0,0));
		CreateWall(new Vector2(0.1f,0.5f), new Vector3(0,screenSize.y,0));
		CreateWall(new Vector2(0.9f,0.5f), new Vector3(0,screenSize.y,0));
		if(Static.isPvP)
		{
			CreateWall(new Vector2(0.5f,0.7f), new Vector3(0,screenSize.y/3,0));			
			CreateWall(new Vector2(0.5f,0.3f), new Vector3(0,screenSize.y/3,0));			
			CreateWall(new Vector2(0.3f,0.5f), new Vector3(screenSize.x/3,0,0));			
			CreateWall(new Vector2(0.7f,0.5f), new Vector3(screenSize.x/3,0,0));
		}
		
		SpawnPlayers();
	}
	
	void CreateWall(Vector2 ViewportPosition, Vector3 scaleSum)
	{
		GameObject wall= (GameObject)Instantiate(wallBase, Camera.main.ViewportToWorldPoint(ViewportPosition), Quaternion.identity);
		wall.transform.Translate(new Vector3(0,0,-Camera.main.transform.position.z));
		wall.transform.localScale+=scaleSum;
	}
	
	void SpawnPlayers()
	{
		int playerCount=0;
				
		for(int i=0; i<Static.playerInput.Length; i++)
		{
			if(Static.playerInput[i]>=0)
			{
				//Instantiate player
				GameObject player = (GameObject)Instantiate(playerModel, spawnSpot[i], playerModel.transform.rotation);				
				
				//Setup script properties
				Player playerScript = player.GetComponent<Player>();
				playerScript.Setup(Static.playerInput[i], playerColor[i]);				
				
				HUD playerHUD = player.GetComponent<HUD>();
				playerHUD.Setup(playerCount);
				
				playerCount++;
			}
		}
	}
	
	void Update()
	{
		//Show game over if all players are dead
		if(!Static.isPvP && Static.playersAlive<=0) 
		{
			gameOver=true;
			Cursor.visible=true;
			if(!UploadedToKongregate) UploadToKongregate();
			if(Input.GetKeyDown(KeyCode.JoystickButton6)) Application.LoadLevel("menu");
		}
		
		//Return to menu if paused
		if(Time.timeScale==0)
		{
			if(Input.GetKeyDown(KeyCode.JoystickButton6)) Application.LoadLevel("menu");
		}
		
		//Time counter
		else if (!gameOver)
		{			
			frameCounter++;
			if(frameCounter>=60)
			{
				timeSeconds++;
				secondsPassed++;
				frameCounter=0;
			}
			if(timeSeconds>=60)
			{
				timeMinutes++;
				timeSeconds=0;
			}
		}
	}
	
	void OnGUI()
	{
		//Time counter
		GUI.Label(new Rect(Screen.width/2-50,Screen.height-40,100,40), timeMinutes+":"+timeSeconds, skin.customStyles[5]);
		
		if(gameOver)
		{
			BackButton("GAME  OVER", skin.customStyles[3]);
			GUI.Label(new Rect(Screen.width/2-200, Screen.height/2+20, 400, 50), "Spheres shot: "+Static.scoreTotal, skin.label);
		}
		
		//Pause menu
		if(Time.timeScale==0)
		{					
			GUI.Box(new Rect(0,0,Screen.width,Screen.height),"");
			BackButton("PAUSED", skin.customStyles[0]);
		}
	}
	
	void BackButton(string CenterText, GUIStyle style)
	{
		GUI.Label(new Rect(Screen.width/2, Screen.height/2-20, 0, 0), CenterText, style);	
		
		if(GUI.Button(new Rect(Screen.width/2-160,Screen.height*0.85f,320,60), "Back to Menu",skin.button))
			Application.LoadLevel("menu");			
		GUI.DrawTexture(new Rect(Screen.width/2-200, Screen.height*0.85f, iconBack.width,iconBack.height),iconBack);
	}
	
	void UploadToKongregate()
	{
		KongregateAPI.SubmitStatistic("Spheres shot", Static.scoreTotal);
		KongregateAPI.SubmitStatistic("Time elapsed", secondsPassed);
		UploadedToKongregate=true;
	}
}
