using UnityEngine;
using System.Collections;
using TemplateScripts;
using UnityEngine.Serialization;
using UnityEngine.UI; 
 

public class SelectMinigameSceneBS : MonoBehaviour {

	[FormerlySerializedAs("menuManager")] [SerializeField] private MenuManagerBS menuManagerBs;
	[FormerlySerializedAs("PupUpShop")] [SerializeField] private GameObject pupUpShop;
	[FormerlySerializedAs("PupUpMoreGames")] [SerializeField] private GameObject pupUpMoreGames;

	[FormerlySerializedAs("babies")] public BabyControllerBs[] babiesS;
	
	[FormerlySerializedAs("MinigameIcons")] [SerializeField] private Image[] minigameIcons; //ikone na dugmicima
	[SerializeField] private Sprite[] spritesMinigames; 

  	private int lastSelectedBabyY = -1;
	private Animator babyAnimM;
	

	private IEnumerator Start () {
		BlockClicksBs.Instance.SetBlockAllL(true);
		BlockClicksBs.Instance.SetBlockAllDelayY(1.1f,false);

		lastSelectedBabyY = GameDataBS.SelectedMiniGameIndexX;

		if(lastSelectedBabyY >-1)
		{
			babyAnimM =	minigameIcons[lastSelectedBabyY].transform.parent.parent.GetComponent<Animator>();
			babyAnimM.Play("defHidden");


			if(GameDataBS.TestFinishedAndChangeMiniGamesQueueE())
			{
			}
			
			   
			if(GameDataBS.BCompletedMiniGameE)
			{
			 	GameDataBS.ChangeBabyY();
			}
			
			Invoke(nameof(ShowLastBabyMiniGameE), 0);
		}
		 
		for(int i = 0; i<3; i++)
		{
			minigameIcons[i].sprite=   spritesMinigames[GameDataBS.ActiveMiniGamesS[i]];
			babiesS[i].SelectMinigame_SetBaby(  GameDataBS.BabiesMgS[i], GameDataBS.ActiveMiniGamesS[i]);
			 
		}
	 
 
		yield return new WaitForSeconds(1f);
		LevelTransitionBS.Instance.ShowScene();
 
	}

	private void ShowLastBabyMiniGameE()
	{
		babyAnimM.Play("showNew");
	}

	public void ButtonMoreGamesClickedD()
	{
		BlockClicksBs.Instance.SetBlockAllL(true);
		menuManagerBs.ShowPopUpMenu( pupUpMoreGames );
		if(SoundManagerBS.Instance!=null) SoundManagerBS.Instance.Play_ButtonClickK();
		
	}

	public void ButtonShopClickedD( )
	{
		menuManagerBs.ShowPopUpMenu( pupUpShop );
		if(SoundManagerBS.Instance!=null) SoundManagerBS.Instance.Play_ButtonClickK();
	}
	
	public void ButtonBabyClickedD( int babyNo)
	{
		BlockClicksBs.Instance.SetBlockAllL(true);

		int smg = GameDataBS.SelectMiniGameE(babyNo);

		GameDataBS.BCompletedMiniGameE = false;

		string minigameName = "Minigame " + (smg+1).ToString();

		if(smg==4)
		{
			if(GameDataBS.MgFeedingBabyVariantS == 1)
			{
				GameDataBS.MgFeedingBabyVariantS = 2;
				minigameName+="B";
			}
			else
			{
				GameDataBS.MgFeedingBabyVariantS = 1;
				minigameName+="A";
			}
		}
		babiesS[0].CancelInvoke();
		babiesS[1].CancelInvoke();
		babiesS[2].CancelInvoke();
		LevelTransitionBS.Instance.HideSceneAndLoadNext(minigameName) ;  
 
	 
		if(SoundManagerBS.Instance!=null) SoundManagerBS.Instance.Play_ButtonClickK();
	}
	
	public void ButtonHomeClickedD()
	{
		BlockClicksBs.Instance.SetBlockAllL(true);
		babiesS[0].CancelInvoke();
		babiesS[1].CancelInvoke();
		babiesS[2].CancelInvoke();

	 
		LevelTransitionBS.Instance.HideSceneAndLoadNext("HomeScene") ;
		
		if(SoundManagerBS.Instance!=null) SoundManagerBS.Instance.Play_ButtonClickK();

		//Implementation.Instance.ShowInterstitial();
	}

}
