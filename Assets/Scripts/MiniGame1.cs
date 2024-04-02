using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TemplateScripts;
using UnityEngine.Serialization;

public class MiniGame1 : MonoBehaviour {

	private int selectedMenu = 1;
	
	[SerializeField] private Sprite rmbActive;
	[SerializeField] private Sprite rmbInactive;
	
	[SerializeField] private Image[] rightMenuButtonImages;

	[SerializeField] private TopMenu topMenu;
	[SerializeField] private Animator animRightMenu;
	[SerializeField] private ProgressBar progressBar;

	public static int CompletedActionNo = 0;
	
	[FormerlySerializedAs("ButtonHome")] [SerializeField] private GameObject buttonHome;
	[FormerlySerializedAs("ButtonNext")] [SerializeField] private GameObject buttonNext;
	
	[SerializeField] private BabyController babyC;
	[SerializeField] private ParticleSystem psLevelCompleted;

	private void Awake()
	{
		buttonNext.SetActive(false);

	}

	private IEnumerator Start () {
		
		CompletedActionNo = 0;
		
		BlockClicks.Instance.SetBlockAll(true);
		BlockClicks.Instance.SetBlockAllDelay(.2f,false);

		animRightMenu.Play("ShowRightMenu");

		
		yield return new WaitForSeconds(.1f);

	 
		//podesi bebicu 
		//Debug.Log("Selektovana bebica:  " +  (GameData.selectedMinigameIndex+1) );
		babyC.GetComponent<SetBabyAtlas>().SetBaby(GameData.GetSelectedBaby());

		babyC.BabyCryingIdle();
		yield return new WaitForSeconds(1);
		LevelTransition.Instance.ShowScene();

		Tutorial.Instance.ShowTutorial(0);
	}

	public void RightMenuButtonClicked(int menu)
	{
		if(CompletedActionNo>=2)
		{
			rightMenuButtonImages[selectedMenu-1].sprite = rmbInactive;
			rightMenuButtonImages[menu-1].sprite = rmbActive;
			selectedMenu = menu;

			topMenu.SetMenuItems(menu);
			ShowTut();
		}
		if(SoundManager.Instance!=null) SoundManager.Instance.Play_ButtonClick();
	}


	private void ShowTut()
	{
		if(CompletedActionNo == 2 )
		{
			if( selectedMenu !=1 )
				Tutorial.Instance.ShowTutorial( 3 ); //ako nije odabran odgovarajuci meni pokazati da se klikne na 1 meni
			else
				Tutorial.Instance.ShowTutorial( 2 );//ako jeste pokazati da treba da se prevuce pelena
		}
		else if(CompletedActionNo == 3 )
		{
			if( selectedMenu !=2 )
				Tutorial.Instance.ShowTutorial( 4 );//ako nije odabran odgovarajuci meni pokazati da se klikne na 2 meni
			else
				Tutorial.Instance.ShowTutorial( 5 );//ako jeste pokazati da treba da se prevuce benkica
		}
		else if(CompletedActionNo == 4 )
		{
			if( selectedMenu !=3 )
				Tutorial.Instance.ShowTutorial( 6 );//ako nije odabran odgovarajuci meni pokazati da se klikne na 3 meni
			else
				Tutorial.Instance.ShowTutorial( 7 );//ako jeste pokazati da treba da se prevuku cipelice
		}
		else if(CompletedActionNo == 5 )
		{
			if( selectedMenu !=4 )
				Tutorial.Instance.ShowTutorial( 8 );//ako nije odabran odgovarajuci meni pokazati da se klikne na 3 meni
			else
				Tutorial.Instance.ShowTutorial( 9 );//ako jeste pokazati da treba da se prevuku cipelice
		}
 
	}

	public void DirtyClothesInBasket(int itemNo)
	{
		 
		CompletedActionNo++;
		progressBar.SetProgress(CompletedActionNo/6f , true );
		if(CompletedActionNo == 2)
		{
			RightMenuButtonClicked(1);
			topMenu.ShowTopMenu(); 
		}
		//Debug.Log("TUT");
		Tutorial.Instance.ShowTutorial(CompletedActionNo);

	}

	public void CompletedAction()
	{
		//brojku mnozim sa 2 jer treba da postoji medjufaza za klik na dugme u meniju..
		//skripta 


		//----------------------------------------------------
		CompletedActionNo++;
		progressBar.SetProgress(CompletedActionNo/6f , true );

		if(CompletedActionNo >2) //==3)
		{
			babyC.BabySmile();
		}

		if(CompletedActionNo == 6)
		{
			if( SoundManager.Instance!=null)  SoundManager.Instance.Play_Sound( SoundManager.Instance.MinigameCompleted);
			StartCoroutine("LevelCompleted");
			//babyC.BabySmile();
		}
		else 
		{
			ShowTut();

			if( SoundManager.Instance!=null)  SoundManager.Instance.Play_Sound( SoundManager.Instance.ActionCompleted);
		}
	}


	private IEnumerator LevelCompleted()
	{
		//Debug.Log("Completed");
		GameData.BCompletedMiniGame = true;
		psLevelCompleted.gameObject.SetActive(true);
		yield return new WaitForEndOfFrame();
		psLevelCompleted.Stop();
		psLevelCompleted.Play();
 
		yield return new WaitForSeconds(1);
		buttonNext.SetActive(true);
		buttonHome.SetActive(false);
	}

	public void ButtonHomeClicked( )
	{
		CompletedActionNo = 0;
		if(SoundManager.Instance!=null) SoundManager.Instance.Play_ButtonClick();
		StopAllCoroutines();
		GameData.BCompletedMiniGame = false;
		LevelTransition.Instance.HideSceneAndLoadNext("SelectMinigame"); 
	 
		BlockClicks.Instance.SetBlockAll(true);
		//Implementation.Instance.ShowInterstitial();
	}

	public void  ButtonNextClicked()
	{
		CompletedActionNo = 0;
		BlockClicks.Instance.SetBlockAll(true);
		if(SoundManager.Instance!=null) SoundManager.Instance.Play_ButtonClick();
		LevelTransition.Instance.HideSceneAndLoadNext("SelectMinigame");
		StopAllCoroutines();
		//Implementation.Instance.ShowInterstitial();
	}

}
