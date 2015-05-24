/*
 * Kongregate stats/login API interface by Raven Black
 * 1. call KongregateAPI.Initialize() once when your application starts
 * (calling again won't cause a problem but does cause an unnecessary lookup)
 *
 * 2. call KongregateAPI.SubmitStatistic("statname",value) whenever a statistic changes
 * or, for things like achievements, whenever you go to a menu screen or something
 * so that missed achievements will be resubmitted later. Or, rather than at every
 * menu, you can do something like the line containing 'Achievements' in the code below,
 * and only otherwise send when a statistic changes.
 * 
 * 3. be sure your code doesn't actively delete all objects in a scene, or care
 * that there's going to be an object named Kongregate lurking there!
*/ 

using UnityEngine;
using System.Collections;

public class KongregateAPI : MonoBehaviour 
{ 
	static public int userId;
	static public string userName="Guest";
	static public string gameAuthToken;
	static public bool isKongregate;
	
	
	static public void Initialize() { 
		if (GameObject.Find("Kongregate")==null) {
			GameObject ko=new GameObject("Kongregate");
			ko.AddComponent<KongregateAPI>();
		}
	}
	
	// Use this for initialization
	void Start () {
//		Debug.Log("Kongregate object initalized.");
		DontDestroyOnLoad(gameObject);
		Application.ExternalEval(@"
		  function submitStat(statName,value) {
		   kongregate.stats.submit(statName,value);
		  }
		  if(typeof(kongregateUnitySupport) != 'undefined'){
		   kongregateUnitySupport.initAPI('Kongregate', 'OnKongregateAPILoaded');
		  }
		  "
		);
	}
	
	void OnKongregateAPILoaded(string userInfoString) {
		isKongregate = true;
		OnKongregateUserSignedIn(userInfoString);
		//the following has to be done here rather than in the previous eval because
		//the 'kongregate' object it references doesn't exist until this callback!
		Application.ExternalEval(@"
		   kongregate.services.addEventListener('login', function(){
		    var services = kongregate.services;
		    var params=[services.getUserId(), services.getUsername(), services.getGameAuthToken()].join('|');
		    kongregateUnitySupport.getUnityObject().SendMessage('Kongregate', 'OnKongregateUserSignedIn', params);
		   });
		   "
		);
		                         
	}

	// this is called when the API loads, from OnKongregateAPILoaded, 
	// and when a user logs in, from the javascript event listener we prepared, 
	// so the name can update from 'Guest' to the user's name.
	void OnKongregateUserSignedIn(string userInfoString) {
		// Split the user info up into tokens
		string[] param=userInfoString.Split('|');
		int.TryParse(param[0],out userId);
		userName = param[1];
		gameAuthToken = param[2];
		// *** something like this calling to your other code can be added to check and send achievements on login
		//Achievements.SendToKongregate(); 
	}
	
	static public void SubmitStatistic(string name,int val) {
		if (isKongregate) {
			Application.ExternalEval("submitStat('"+name+"',"+val+");");
		}
	}
}
