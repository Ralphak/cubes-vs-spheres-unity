using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour 
{
	public GUISkin skin;
	public Texture2D iconA;
	public Texture2D iconB;
	public Texture2D iconX;
	public Texture2D iconBack;
	public GameObject BGM;
	static bool toggleBGM = true;
	static string statusBGM = "Music ON";
	
	
	void Start () 
	{
		KongregateAPI.Initialize();
		Cursor.visible=true;
		Static.ResetValues();
		if(GameObject.Find("BGM(Clone)")==null && toggleBGM) Instantiate(BGM);
	}
	
	
	void Update () 
	{
		//Gamepad controls
		if(Input.GetKeyDown(KeyCode.JoystickButton0)) NewGame();
		if(Input.GetKeyDown(KeyCode.JoystickButton1)) NewGame(true);
		if(Input.GetKeyDown(KeyCode.JoystickButton2)) Application.LoadLevel("help");
	}
	
	
	void OnGUI()
	{
		GUI.Label(new Rect(Screen.width/2, Screen.height*0.075f, 0, 0), "Cubes  VS  Spheres", skin.customStyles[0]);
		
		
		GUI.BeginGroup(new Rect(0,Screen.height*0.25f,Screen.width,Screen.height));
		int buttonDistance=110;
		
		if(GUI.Button(new Rect(Screen.width/2-140,0,280,60), "New Game", skin.button)) NewGame();
		GUI.DrawTexture(new Rect(Screen.width/2-180,0,iconA.width,iconA.height), iconA);
		
		if(GUI.Button(new Rect(Screen.width/2-140,buttonDistance,280,60), "Versus", skin.button)) NewGame(true);
		GUI.DrawTexture(new Rect(Screen.width/2-180,buttonDistance,iconB.width,iconB.height), iconB);
		
		if(GUI.Button(new Rect(Screen.width/2-140,buttonDistance*2,280,60), "Help", skin.button)) Application.LoadLevel("help");
		GUI.DrawTexture(new Rect(Screen.width/2-180,buttonDistance*2,iconX.width,iconX.height), iconX);
		
		GUI.EndGroup();
		
		
		GUI.Label(new Rect(10,Screen.height-25,400,25), "A game developed by Rafael Alves");

		if (GUI.Button (new Rect (Screen.width - 150, Screen.height-40, 100, 25), statusBGM)) 
		{
			if (toggleBGM) 
			{
				Destroy(GameObject.Find("BGM(Clone)"));
				statusBGM = "Music OFF";
				toggleBGM = false;
			}
			else
			{
				Instantiate(BGM);
				statusBGM = "Music ON";
				toggleBGM = true;
			}
		}
	}
	
	void NewGame(bool isPvP=false)
	{
		Static.isPvP=isPvP;
		Application.LoadLevel("chooseplayer");
	}
}
