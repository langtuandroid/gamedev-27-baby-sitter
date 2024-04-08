using UnityEngine;

namespace TemplateScripts
{
	public class GlobalVariables : MonoBehaviour {
		
		public static int GameMod = 0; 
		public static int CurrentLvl = 0;

		public static bool CategoriesBought = false;
	
		public static int NumberOfStart = 0;
		public static string MgDrawShaperSavedProgress = "";

		public static bool HomeFlexClosed;
		public static bool BabySelectFlexClosed;
 
		public static bool RemoveAdsOwned = false;
		public static string ApplicationID;

		public static int HomeInterstitialCounter;
		public static int NextInterstitialCounter;
		public static int BabyInterstitialCounter;
		public static bool PanelFirstTimeShown;
		
		private void Awake () {

			HomeInterstitialCounter = 0;
			NextInterstitialCounter = 0;
			BabyInterstitialCounter = 0;
			PanelFirstTimeShown = false;

			HomeFlexClosed = false;
			BabySelectFlexClosed = false;

			DontDestroyOnLoad(gameObject);
#if UNITY_ANDROID || UNITY_EDITOR_WIN
			ApplicationID = "com.test.package.name";
#elif UNITY_IOS
		applicationID = ""; // "bundle.ID";
#endif
		}

		public void DisableLog(string message)
		{
			Debug.unityLogger.logEnabled = false;
		}
	}
}
