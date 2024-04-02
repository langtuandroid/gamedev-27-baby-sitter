using UnityEngine;
using System.Collections;
using UnityEngine.UI;

///<summary>
///<para>Scene:All/NameOfScene/NameOfScene1,NameOfScene2,NameOfScene3...</para>
///<para>Object:N/A</para>
///<para>Description: Sample Description </para>
///</summary>

public class GlobalVariables : MonoBehaviour {



	//***************************************************************

	public static int gameMod = 0; // 0 - re-create; 1 - create your own
	public static int currentLvl = 0;
//	public static int currentCategory = 0;
//	public static int currentEpisode = 0;
	public static bool categoriesBought = false;
	//public static bool removeAds = false;
	public static int numberOfStart = 0;
	public static string mgDrawShaper_SavedProgres = "";

	public static bool homeFlexiClosed;
	public static bool babySelectFlexiClosed;
 
	public static bool removeAdsOwned = false;
	public static string applicationID;

	public static int homeInterstitialCounter;
	public static int nextInterstitialCounter;
	public static int babyInterstitialCounter;
	public static bool panelFirstTimeShown;

	// Use this for initialization
	void Awake () {

		homeInterstitialCounter = 0;
		nextInterstitialCounter = 0;
		babyInterstitialCounter = 0;
		panelFirstTimeShown = false;

		homeFlexiClosed = false;
		babySelectFlexiClosed = false;

		DontDestroyOnLoad(gameObject);
		#if UNITY_ANDROID || UNITY_EDITOR_WIN
        applicationID = "com.test.package.name";
		#elif UNITY_IOS
		applicationID = ""; // "bundle.ID";
		#endif
	}

    public void DisableLog(string message)
    {
        Debug.unityLogger.logEnabled = false;
    }
}
