using UnityEngine;
using System.Collections;
using TemplateScripts;
using UnityEngine.Serialization;
using UnityEngine.UI; 
 

public class SelectMinigameScene : MonoBehaviour {

	[SerializeField] private MenuManager menuManager;
	[FormerlySerializedAs("PupUpShop")] [SerializeField] private GameObject pupUpShop;
	[FormerlySerializedAs("PupUpMoreGames")] [SerializeField] private GameObject pupUpMoreGames;

	public BabyController[] babies;
	
	[FormerlySerializedAs("MinigameIcons")] [SerializeField] private Image[] minigameIcons; //ikone na dugmicima
	[SerializeField] private Sprite[] spritesMinigames; 

  	private int lastSelectedBaby = -1;
	private Animator babyAnim;
	

	private IEnumerator Start () {
		BlockClicks.Instance.SetBlockAll(true);
		BlockClicks.Instance.SetBlockAllDelay(1.1f,false);

		lastSelectedBaby = GameData.SelectedMiniGameIndex;

		if(lastSelectedBaby >-1)
		{
			babyAnim =	minigameIcons[lastSelectedBaby].transform.parent.parent.GetComponent<Animator>();
			babyAnim.Play("defHidden");


			if(GameData.TestFinishedAndChangeMiniGamesQueue())
			{
			}
			
			   
			if(GameData.BCompletedMiniGame)
			{
			 	GameData.ChangeBaby();
			}
			
			Invoke(nameof(ShowLastBabyMiniGame), 0);
		}
		 
		for(int i = 0; i<3; i++)
		{
			minigameIcons[i].sprite=   spritesMinigames[GameData.ActiveMiniGames[i]];
			babies[i].SelectMinigame_SetBaby(  GameData.BabiesMg[i], GameData.ActiveMiniGames[i]);
			 
		}
	 
 
		yield return new WaitForSeconds(1f);
		LevelTransition.Instance.ShowScene();
 
	}

	private void ShowLastBabyMiniGame()
	{
		babyAnim.Play("showNew");
	}

	public void ButtonMoreGamesClicked()
	{
		BlockClicks.Instance.SetBlockAll(true);
		menuManager.ShowPopUpMenu( pupUpMoreGames );
		if(SoundManager.Instance!=null) SoundManager.Instance.Play_ButtonClick();
		
	}

	public void ButtonShopClicked( )
	{
		menuManager.ShowPopUpMenu( pupUpShop );
		if(SoundManager.Instance!=null) SoundManager.Instance.Play_ButtonClick();
	}
	
	public void ButtonBabyClicked( int babyNo)
	{
		BlockClicks.Instance.SetBlockAll(true);

		int smg = GameData.SelectMiniGame(babyNo);

		GameData.BCompletedMiniGame = false;

		string minigameName = "Minigame " + (smg+1).ToString();

		if(smg==4)
		{
			if(GameData.MgFeedingBabyVariant == 1)
			{
				GameData.MgFeedingBabyVariant = 2;
				minigameName+="B";
			}
			else
			{
				GameData.MgFeedingBabyVariant = 1;
				minigameName+="A";
			}
		}
		babies[0].CancelInvoke();
		babies[1].CancelInvoke();
		babies[2].CancelInvoke();
		LevelTransition.Instance.HideSceneAndLoadNext(minigameName) ;  
 
	 
		if(SoundManager.Instance!=null) SoundManager.Instance.Play_ButtonClick();
	}
	
	public void ButtonHomeClicked()
	{
		BlockClicks.Instance.SetBlockAll(true);
		babies[0].CancelInvoke();
		babies[1].CancelInvoke();
		babies[2].CancelInvoke();

	 
		LevelTransition.Instance.HideSceneAndLoadNext("HomeScene") ;
		
		if(SoundManager.Instance!=null) SoundManager.Instance.Play_ButtonClick();

		//Implementation.Instance.ShowInterstitial();
	}

}
