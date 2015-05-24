using UnityEngine;
using System.Collections;

public class Help : MonoBehaviour {

	public GUISkin skin;
	public Texture2D mouseKeyboard;
	public Texture2D controller;
	public Texture2D iconL;
	public Texture2D iconR;
	public Texture2D iconRT;
	public Texture2D iconBack;
	
	void OnGUI () 
	{
		GUI.Label(new Rect(0, Screen.height*0.25f, 300,100), "Move",skin.customStyles[2]);
		GUI.Label(new Rect(0, Screen.height*0.45f, 300,100), "Rotate",skin.customStyles[2]);
		GUI.Label(new Rect(0, Screen.height*0.65f, 300,100), "Fire",skin.customStyles[2]);
		
		GUI.DrawTexture(new Rect(Screen.width/2-mouseKeyboard.width/2, Screen.height*0.05f, mouseKeyboard.width, mouseKeyboard.height), mouseKeyboard);
		GUI.Label(new Rect(Screen.width/2-150, Screen.height*0.25f, 300,100), "WASD or Arrows",skin.label);
		GUI.Label(new Rect(Screen.width/2-150, Screen.height*0.45f, 300,100), "Mouse",skin.label);
		GUI.Label(new Rect(Screen.width/2-150, Screen.height*0.65f, 300,100), "Left Click",skin.label);
		
		GUI.DrawTexture(new Rect(Screen.width*0.85f-controller.width/2, Screen.height*0.05f, controller.width, controller.height), controller);
		GUI.DrawTexture(new Rect(Screen.width*0.85f-iconL.width/2, Screen.height*0.25f+iconL.height/2, iconL.width, iconL.height), iconL);
		GUI.DrawTexture(new Rect(Screen.width*0.85f-iconR.width/2, Screen.height*0.45f+iconR.height/2, iconR.width, iconR.height), iconR);
		GUI.DrawTexture(new Rect(Screen.width*0.85f-iconRT.width/2, Screen.height*0.65f+iconRT.height*0.25f, iconRT.width, iconRT.height), iconRT);
		
		if(GUI.Button(new Rect(Screen.width/2-160,Screen.height*0.85f,320,60), "Back to Menu",skin.button))
			Application.LoadLevel("menu");
		GUI.DrawTexture(new Rect(Screen.width/2-200, Screen.height*0.85f, iconBack.width,iconBack.height),iconBack);
	}
	
	
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton6)) 
		{
			GetComponent<Camera>().GetComponent<AudioSource>().Play();
			Application.LoadLevel("menu");
		}
	}
}
