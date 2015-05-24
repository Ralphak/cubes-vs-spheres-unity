using UnityEngine;
using System.Collections;

public static class Static
{
	public static bool isPvP;	
	public static int[] playerInput = new int[4];
	public static int playersAlive;
	public static int scoreTotal;
	static float savedTimeScale;
	
	public static void ResetValues() 
	{
		for(int i=0; i<playerInput.Length; i++) playerInput[i]=-1;
		playersAlive=0;
		scoreTotal=0;
		Time.timeScale=1;
		AudioListener.pause=false;
	}	
	
	public static void PauseGame() 
	{
	    savedTimeScale = Time.timeScale;
	    Time.timeScale = 0;
	    AudioListener.pause = true;
	}
 
	public static void UnPauseGame() 
	{
	    Time.timeScale = savedTimeScale;
	    AudioListener.pause = false; 
	}
}
