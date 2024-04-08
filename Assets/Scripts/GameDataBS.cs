using UnityEngine;
using System.Collections;

public class GameDataBS  {

	public static int SelectedMiniGameIndexX =-1;
	public static int SelectedMiniGameE = -1; 
	public static bool  BCompletedMiniGameE = false;
	public static int[] ActiveMiniGamesS;

	private static readonly Queue MiniGamesQueueE = new Queue(5);

	public static int MgFeedingBabyVariantS = 1;
	
	public static readonly int[] BabiesMgS = {0,1,2,3};
	
	public static void SetMiniGamesQueueE()
	{
		if(ActiveMiniGamesS !=null) return;
		
		ActiveMiniGamesS = new int[3];
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
			ActiveMiniGamesS[i] = mg[i];
		}
		for(int i = 3; i <8;i++)
		{
			MiniGamesQueueE.Enqueue(mg[i]);
		}
		
		for(int i = 0; i <100;i++)
		{
			j = Random.Range(0,BabiesMgS.Length);
			a = BabiesMgS[j];
			k = Random.Range(0,BabiesMgS.Length);
			BabiesMgS[j]= BabiesMgS[k];
			BabiesMgS[k] = a;
		}
	 
	}
	
	public static int SelectMiniGameE(int index)
	{
		BCompletedMiniGameE = false;
		SelectedMiniGameIndexX = index;
		SelectedMiniGameE =  ActiveMiniGamesS[index];
		return SelectedMiniGameE;
	}

	public static int GetSelectedBabyY( )
	{
		return BabiesMgS[SelectedMiniGameIndexX];
	}

	public static void ChangeBabyY()
	{
		(BabiesMgS[SelectedMiniGameIndexX], BabiesMgS[3]) = (BabiesMgS[3], BabiesMgS[SelectedMiniGameIndexX]);
	}

	public static bool TestFinishedAndChangeMiniGamesQueueE()
	{
		if(BCompletedMiniGameE && SelectedMiniGameIndexX>-1)
		{
			int mg = ActiveMiniGamesS[SelectedMiniGameIndexX];
			ActiveMiniGamesS[SelectedMiniGameIndexX] = (int) MiniGamesQueueE.Dequeue();
			MiniGamesQueueE.Enqueue(mg);

			return true;
		}

		return false;
	}
	

	private static string _sTestiranje = "";

  
	public static void Init()
	{

# if UNITY_EDITOR
		if( true  ) 
		{
			_sTestiranje = "Test;"

			 ;

		}
#endif
 


		GetUnlockedItemsS();

		SetMiniGamesQueueE();
		MgFeedingBabyVariantS = (Random.Range(0,9) > 4) ? 1 : 2;
	}



	public static string UnlockedItemsS = "";
	public static void SaveUnlockedItemsToPpP(string itemName)
	{
		if(!UnlockedItemsS.Contains(itemName))
		{
			UnlockedItemsS += (itemName + ";" );
			PlayerPrefs.SetString("Data1",UnlockedItemsS);
		}
	}

	public static void GetUnlockedItemsS( )
	{
		MenuItemsBS mi = new MenuItemsBS();
		UnlockedItemsS = PlayerPrefs.GetString("Data1", "");
		string[] unl_items = UnlockedItemsS.Split(new char[] {';'},System.StringSplitOptions.RemoveEmptyEntries);
		for(int i = 0; i<unl_items.Length;i++)
		{
			if(MenuItemsBS.mitdD.ContainsKey("M0"+unl_items[i]))
			{
				MenuItemsBS.mitdD["M0"+unl_items[i]].Locked = false;
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
}
 


 
 
