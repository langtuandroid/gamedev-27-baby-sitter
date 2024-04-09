using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TemplateScripts;
using UnityEngine.Serialization;

public class MiniGame1BS : MonoBehaviour {

	private int selectedMenuU = 1;
	
	[SerializeField] private Sprite rmbActive;
	[SerializeField] private Sprite rmbInactive;
	
	[SerializeField] private Image[] rightMenuButtonImages;

	[FormerlySerializedAs("topMenu")] [SerializeField] private TopMenuBS topMenuBs;
	[SerializeField] private Animator animRightMenu;
	[SerializeField] private ProgressBar progressBar;

	public static int CompletedActionNoN = 0;
	
	[FormerlySerializedAs("ButtonHome")] [SerializeField] private GameObject buttonHome;
	[FormerlySerializedAs("ButtonNext")] [SerializeField] private GameObject buttonNext;
	
	[FormerlySerializedAs("babyC")] [SerializeField] private BabyControllerBs babyCC;
	[FormerlySerializedAs("psLevelCompleted")] [SerializeField] private ParticleSystem psLevelCompletedD;

	private void Awake()
	{
		buttonNext.SetActive(false);

	}

	private IEnumerator Start () {
		
		CompletedActionNoN = 0;
		
		BlockClicksBs.Instance.SetBlockAllL(true);
		BlockClicksBs.Instance.SetBlockAllDelayY(.2f,false);

		animRightMenu.Play("ShowRightMenu");

		
		yield return new WaitForSeconds(.1f);
		
		babyCC.GetComponent<SetBabyAtlasBS>().SetBabyY(GameDataBS.GetSelectedBabyY());

		babyCC.BabyCryingIdle();
		yield return new WaitForSeconds(1);
		LevelTransitionBS.Instance.ShowScene();

		TutorialBS.Instance.ShowTutorial(0);
	}

	public void RightMenuButtonClickedD(int menu)
	{
		if(CompletedActionNoN>=2)
		{
			rightMenuButtonImages[selectedMenuU-1].sprite = rmbInactive;
			rightMenuButtonImages[menu-1].sprite = rmbActive;
			selectedMenuU = menu;

			topMenuBs.SetMenuItemsS(menu);
			ShowTutT();
		}
		if(SoundManagerBS.Instance!=null) SoundManagerBS.Instance.Play_ButtonClickK();
	}


	private void ShowTutT()
	{
		if(CompletedActionNoN == 2 )
		{
			if( selectedMenuU !=1 )
				TutorialBS.Instance.ShowTutorial( 3 );
			else
				TutorialBS.Instance.ShowTutorial( 2 );
		}
		else if(CompletedActionNoN == 3 )
		{
			if( selectedMenuU !=2 )
				TutorialBS.Instance.ShowTutorial( 4 );
			else
				TutorialBS.Instance.ShowTutorial( 5 );
		}
		else if(CompletedActionNoN == 4 )
		{
			if( selectedMenuU !=3 )
				TutorialBS.Instance.ShowTutorial( 6 );
			else
				TutorialBS.Instance.ShowTutorial( 7 );
		}
		else if(CompletedActionNoN == 5 )
		{
			if( selectedMenuU !=4 )
				TutorialBS.Instance.ShowTutorial( 8 );
			else
				TutorialBS.Instance.ShowTutorial( 9 );
		}
 
	}

	public void DirtyClothesInBasketT(int itemNo)
	{
		CompletedActionNoN++;
		progressBar.SetProgressBar(CompletedActionNoN/6f , true );
		if(CompletedActionNoN == 2)
		{
			RightMenuButtonClickedD(1);
			topMenuBs.ShowTopMenuU(); 
		}
		//Debug.Log("TUT");
		TutorialBS.Instance.ShowTutorial(CompletedActionNoN);

	}

	public void CompletedActionN()
	{
		CompletedActionNoN++;
		progressBar.SetProgressBar(CompletedActionNoN/6f , true );

		if(CompletedActionNoN >2) //==3)
		{
			babyCC.BabySmile();
		}

		if(CompletedActionNoN == 6)
		{
			if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.PlaySound( SoundManagerBS.Instance.MinigameCompleted);
			StartCoroutine(nameof(LevelCompletedD));
		}
		else 
		{
			ShowTutT();

			if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.PlaySound( SoundManagerBS.Instance.ActionCompleted);
		}
	}


	private IEnumerator LevelCompletedD()
	{
		//Debug.Log("Completed");
		GameDataBS.BCompletedMiniGameE = true;
		psLevelCompletedD.gameObject.SetActive(true);
		yield return new WaitForEndOfFrame();
		psLevelCompletedD.Stop();
		psLevelCompletedD.Play();
 
		yield return new WaitForSeconds(1);
		buttonNext.SetActive(true);
		buttonHome.SetActive(false);
	}

	public void ButtonHomeClickedD( )
	{
		CompletedActionNoN = 0;
		if(SoundManagerBS.Instance!=null) SoundManagerBS.Instance.Play_ButtonClickK();
		StopAllCoroutines();
		GameDataBS.BCompletedMiniGameE = false;
		LevelTransitionBS.Instance.HideSceneAndLoadNext("SelectMinigame"); 
	 
		BlockClicksBs.Instance.SetBlockAllL(true);
		//Implementation.Instance.ShowInterstitial();
	}

	public void ButtonNextClickedD()
	{
		CompletedActionNoN = 0;
		BlockClicksBs.Instance.SetBlockAllL(true);
		if(SoundManagerBS.Instance!=null) SoundManagerBS.Instance.Play_ButtonClickK();
		LevelTransitionBS.Instance.HideSceneAndLoadNext("SelectMinigame");
		StopAllCoroutines();
	}

}
