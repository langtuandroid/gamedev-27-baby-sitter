using UnityEngine;
using System.Collections;

public class GameData  {

	public static int SelectedMiniGameIndex =-1;//  // bebica koja je selektovana (0,1,2)
	public static int SelectedMiniGame = -1; //mini igra koju selektovana bebica trazi
	public static bool  BCompletedMiniGame = false;
	public static int[] ActiveMiniGames;

	private static readonly Queue MiniGamesQueue = new Queue(5);

	public static int MgFeedingBabyVariant = 1;
	
	public static readonly int[] BabiesMg = {0,1,2,3};

	//public static string Unlocked = "dafE1A";
	 
	//----------------------------------------------------------------
	public static void  SetMiniGamesQueue()
	{
		if(ActiveMiniGames !=null) return;

//		Debug.Log("SET MG");
		ActiveMiniGames = new int[3];
		int[] mg = new int[] {0,1,2,3,4,5,6,7};
		int   a,j,k = 0;
		for(int i = 0; i <100;i++)
		{
			j = Random.Range(0,mg.Length);
			a = mg[j];
			k = Random.Range(0,mg.Length);
			mg[j]= mg[k];
			mg[k] = a;
		}
 
		for(int i = 0; i <3;i++)
		{
			ActiveMiniGames[i] = mg[i];
		}
		for(int i = 3; i <8;i++)
		{
			MiniGamesQueue.Enqueue(mg[i]);
		}

		//bebice
		for(int i = 0; i <100;i++)
		{
			j = Random.Range(0,BabiesMg.Length);
			a = BabiesMg[j];
			k = Random.Range(0,BabiesMg.Length);
			BabiesMg[j]= BabiesMg[k];
			BabiesMg[k] = a;
		}
	 
	}
	
	public static int SelectMiniGame(int index)
	{
		BCompletedMiniGame = false;
		SelectedMiniGameIndex = index;
		SelectedMiniGame =  ActiveMiniGames[index];
		return SelectedMiniGame;
	}

	public static int GetSelectedBaby( )
	{
		return BabiesMg[SelectedMiniGameIndex];
	}

	public static void ChangeBaby()
	{
		(BabiesMg[SelectedMiniGameIndex], BabiesMg[3]) = (BabiesMg[3], BabiesMg[SelectedMiniGameIndex]);
	}

	public static bool TestFinishedAndChangeMiniGamesQueue()
	{
		if(BCompletedMiniGame && SelectedMiniGameIndex>-1)
		{
			int mg = ActiveMiniGames[SelectedMiniGameIndex];
			ActiveMiniGames[SelectedMiniGameIndex] = (int) MiniGamesQueue.Dequeue();
			MiniGamesQueue.Enqueue(mg);

			return true;
		}

		return false;
	}


	//------------------------------------------------------------------


	private static string _sTestiranje = "";//InternetOff;";

  
	public static void Init()
	{
		//-----------------------------------------------------------------
 
# if UNITY_EDITOR
		if( true  ) 
		{
			_sTestiranje = "Test;"

			 ;

		}
		//-----------------------------------------------------------------------
#endif
 


		GetUnlockedItems();

		SetMiniGamesQueue();
		MgFeedingBabyVariant = (Random.Range(0,9) > 4) ? 1 : 2;
	}



	public static string UnlockedItems = "";
	public static void SaveUnlockedItemsToPp(string itemName)
	{
		if(!UnlockedItems.Contains(itemName))
		{
			UnlockedItems += (itemName + ";" );
			PlayerPrefs.SetString("Data1",UnlockedItems);
		}
	}

	public static void GetUnlockedItems( )
	{
		MenuItems mi = new MenuItems();
		UnlockedItems = PlayerPrefs.GetString("Data1", "");
//			Debug.Log("UNLOCKED:  "+ UnlockedItems);
		string[] unl_items = UnlockedItems.Split(new char[] {';'},System.StringSplitOptions.RemoveEmptyEntries);
		for(int i = 0; i<unl_items.Length;i++)
		{
			if(MenuItems.mitd.ContainsKey("M0"+unl_items[i]))
			{
				MenuItems.mitd["M0"+unl_items[i]].Locked = false;
			}
		}
	}
	
	private static void SetUnlockedFromString( ref bool[] unlockedItems, string data)
	{
		if(data != "")
		{
			string[] pom = data.Split(';');
			for(int i = 0; i< pom.Length;i++)
			{
				int item = 0;
				int.TryParse(pom[i],out item);
				if(item < unlockedItems.Length) unlockedItems[item] = true;
			}
		}
	}
	
	public static void IncrementButtonNextClickedCount()
	{
		//Implementation.Instance.ShowInterstitial();
	}
 

	public static void IncrementButtonHomeClickedCount()
	{
		//Implementation.Instance.ShowInterstitial();
	}

	//---------------------------------------------------------------------------------------------------------------------------



}
 


 
 
