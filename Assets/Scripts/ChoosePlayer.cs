using UnityEngine;
using System.Collections;

public class ChoosePlayer : MonoBehaviour {

	public GUISkin skin;
	string[] slotLabel= new string[4];
	GUIStyle[] slotStatus = new GUIStyle[4];
	
	string[] fireButton = new string[5];
	string[] fireTrigger = new string[5];
	string[] start = new string[5];
	
	public Texture2D[] cubeIcon;
	public Texture2D iconBack;
	
	bool[] triggerPressed =new bool[5];
	int readyCount;
	string map;
	
	void Start () 
	{
		if(Static.isPvP) map="mapversus";
		else map="map";
		
		//Get input from all devices
		fireButton[0]="Fire";
		for(int i=1; i<fireButton.Length; i++) 
		{
			fireButton[i]= "Joy"+i+" Fire Button";
			fireTrigger[i]= "Joy"+i+" Fire";
			start[i]= "Joy"+i+" Pause";
		}
		
		//Set boxes' content
		for(int i=0; i<slotLabel.Length; i++)
		{
			slotLabel[i]="Press Fire button";
			slotStatus[i]=skin.box;
		}
	}
	
	
	void Update()
	{
		//Get pressed fire input from mouse
		if(Input.GetButtonDown(fireButton[0])) Ready(0);
		
		//Get pressed fire input from gamepads
		for(int i=1; i<fireButton.Length; i++) 
		{
			if(Input.GetAxisRaw(fireTrigger[i])!=0 || Input.GetButtonDown(fireButton[i])) 
			{
				if(!triggerPressed[i]) 
				{
					Ready(i);
					triggerPressed[i]=true;
				}
			}
			else triggerPressed[i]=false;
		}
		
		//Enable start if there's at least one player ready		
		if(readyCount>0) 
		{
			if(Input.GetKeyDown(KeyCode.Return)) Application.LoadLevel(map);
			
			for(int i=1; i<start.Length; i++)
				if(Input.GetButtonDown(start[i])) Application.LoadLevel(map);
		}
		
		//Back to menu
		if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton6)) 
			Application.LoadLevel("menu");
	}
	
	
	void Ready(int inputUsed)
	{
		//Check if input was already assigned
		bool inputAlreadyExists=false;
		for(int i=0; i<Static.playerInput.Length; i++)
		{
			if(Static.playerInput[i]==inputUsed) inputAlreadyExists=true;
		}
		
		//If not, then use a slot if available
		if(!inputAlreadyExists && readyCount<Static.playerInput.Length)
		{
			Static.playerInput[readyCount]=inputUsed;			
			slotStatus[readyCount]=skin.customStyles[1];
			
			if(inputUsed==0) 
				slotLabel[readyCount]="Ready (Mouse/Keyboard)";
			else 
				slotLabel[readyCount]="Ready ("+Input.GetJoystickNames()[inputUsed-1]+")";
						
			readyCount++;
		}
	}
	
	
	void OnGUI () 
	{
		for(int i=0; i<slotLabel.Length; i++)
		{
			Vector2 position= new Vector2(Screen.width/2 - Screen.width*0.35f + cubeIcon[i].width/2, Screen.height*0.1f +100*i);
			GUI.Box(new Rect(position.x, position.y, Screen.width*0.7f, 60), slotLabel[i], slotStatus[i]);
			GUI.DrawTexture(new Rect(position.x-110, position.y-10, cubeIcon[i].width,cubeIcon[i].height), cubeIcon[i]);
		}		
		
		if(GUI.Button(new Rect(Screen.width/2-160,Screen.height*0.85f,320,60), "Back to Menu",skin.button))
			Application.LoadLevel("menu");
		GUI.DrawTexture(new Rect(Screen.width/2-200, Screen.height*0.85f, iconBack.width,iconBack.height),iconBack);
		
		if(readyCount>0) GUI.Label(new Rect(Screen.width/2-200,Screen.height*0.71f,400,100), "Press Start or Enter",skin.customStyles[2]);
	}
}
