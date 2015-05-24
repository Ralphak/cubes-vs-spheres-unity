using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour 
{
	public GUISkin skin;
	public Texture2D[] cubeIcon;
	public Texture2D[] cubeLife;
	Texture2D icon;
	Texture2D life;
	
	float baseX;
	int playerID;
	public int lifeMax;
	public int lifeCounter;
	public int score;
	
	float combo;
	float shield;
	Texture2D comboBar;
	Texture2D shieldBar;
	Texture2D defaultColor;
	Texture2D specialColor;
	public Texture2D comboBorder;
	public Texture2D[] comboColors;
	public bool shieldMode;
	public bool specialMode;
	
	
	public void Setup(int PlayerID)
	{
		playerID=PlayerID;
		icon=cubeIcon[PlayerID];
		life=cubeLife[PlayerID];
		defaultColor=comboColors[PlayerID];
		comboBar=defaultColor;
	}
	
	void Start () 
	{
		baseX = Screen.width*0.01f*(27.75f*playerID);
		if(Static.isPvP) lifeMax=1;
		else lifeMax=3;
		lifeCounter=lifeMax;
		specialColor=comboColors[4];
		shieldBar=comboColors[5];
	}
	
	
	void Update () 
	{
		Cursor.visible=false;		
		
		if(Time.timeScale!=0)
		{
			//Lower down combo bar if not shielding. Removes special when it reaches zero.
			if(!shieldMode && combo>0) 
			{
				if(specialMode) combo-=0.4f;
				else combo-=0.15f;
			}
			else if(combo<=0)
			{ 
				specialMode=false;
				comboBar=defaultColor;
			}
			
			//Enter special mode if combo bar fills up
			if(combo>=100)
			{
				combo=100;
				if(!specialMode)
				{
					specialMode=true;
					shieldMode=true;
					shield=100;
					comboBar=specialColor;
				}
			}
			
			//Shield mode
			if(shieldMode)
			{
				shield-=0.8f;
				if(shield<=0)
				{
					shieldMode=false;
					shield=0;
				}
			}
		}//End of pause check
	}
	
	public void AddCombo(float comboValue)
	{
		if(!shieldMode) combo+=comboValue;
	}
	
	
	void OnGUI()
	{
		//Cube icon
		GUI.DrawTexture(new Rect(baseX,0,icon.width/2,icon.height/2), icon);
		
		//Life
		for(int i=0; i<lifeCounter; i++)
			GUI.DrawTexture(new Rect(baseX+icon.width/2+life.width*0.75f*i, 5, life.width/2, life.height/2), life);
		
		//Score
		GUI.Label(new Rect(baseX+icon.width/2, 11, 0,0), score.ToString(), skin.customStyles[4]);
		
		//Combo bar
		if(!Static.isPvP) 
		{
			GUI.DrawTexture(new Rect(baseX+3, icon.height/2-2, comboBorder.width, comboBorder.height), comboBorder);
			GUI.DrawTexture(new Rect(baseX+5, icon.height/2, combo,10), comboBar);
			GUI.DrawTexture(new Rect(baseX+5, icon.height/2, shield,10), shieldBar);
		}
	}
}
