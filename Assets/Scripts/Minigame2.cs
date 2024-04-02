using UnityEngine;
using System.Collections;
using TemplateScripts;

public class Minigame2 : MonoBehaviour {
	
	[SerializeField] private TopMenu topMenu;
	[SerializeField] private ProgressBar progressBar;
	
	public static int CompletedActionNo = 0;
	
	[SerializeField] private GameObject buttonNext;
	[SerializeField] private GameObject buttonHome;

	[SerializeField] private Animator animShower;
	[SerializeField] private Animator animBathWater;

	[SerializeField] private ParticleSystem psBabyWet1;
	[SerializeField] private ParticleSystem psBabyWet2;
	[SerializeField] private ParticleSystem psBabyWet3;
	
	private int phase;

	[SerializeField] private GameObject shampooHeadHolder;

	private BubblesListHolder bubblesAnimHolder;

	[SerializeField] private BabyController babyC;
	[SerializeField] private ParticleSystem psLevelCompleted;
	 

	private void Awake()
	{
		buttonNext.SetActive(false);
		bubblesAnimHolder = transform.GetComponent<BubblesListHolder>();
	}
	
	private IEnumerator Start () {
		//topMenu.ShowTopMenu();
		//topMenu.SetMenuItems(1);
		CompletedActionNo = 0;
		CleaningTool.OneToolEnabledNo = 0;
		
		BlockClicks.Instance.SetBlockAll(true);
		BlockClicks.Instance.SetBlockAllDelay(.2f,false);
		
		yield return new WaitForSeconds(.1f);
	
		
		//podesi bebicu 
		//Debug.Log("Selektovana bebica:  " +  (GameData.selectedMinigameIndex+1) );
		babyC.GetComponent<SetBabyAtlas>().SetBaby(GameData.GetSelectedBaby());
		babyC.BabyBath();
		
		yield return new WaitForSeconds(1);
		LevelTransition.Instance.ShowScene();
		 
		Tutorial.Instance.ShowTutorial(0);
	}


	public void NextPhase( string phaseState )
	{
		StartCoroutine(nameof(WaitNextPhase),phaseState);
	}

	private IEnumerator WaitNextPhase( string phaseState )
	{
		//Debug.Log (phase + "ss"+phaseState);
		//punjenj kade vodom
		if(phaseState == "ShowerTap" && phase == 0)
		{
			Tutorial.Instance.StopTutorial();
			phase = 1;
			CleaningTool.OneToolEnabledNo = 0;

			animShower.Play("FillWater");
			animBathWater.gameObject.SetActive(true);
			animBathWater.Play("Rise");
			yield return new WaitForSeconds(.1f);
			if( SoundManager.Instance!=null) 
			{
				SoundManager.Instance.Play_Sound( SoundManager.Instance.Shower);
				SoundManager.Instance.listStopSoundOnExit.Add( SoundManager.Instance.Shower);
			}
			yield return new WaitForSeconds(2);
			CompletedAction();

			yield return new WaitForSeconds(1.8f);
			if( SoundManager.Instance!=null)  SoundManager.Instance.Stop_Sound( SoundManager.Instance.Shower); 
			phase = 2;
			
			topMenu.ShowTopMenu(); 

			yield return new WaitForSeconds(1);
			Tutorial.Instance.ShowTutorial(1);
		}
		//stavljanje igracke i kadu
		else if(phaseState == "GiveToy" && phase == 2)
		{
			//Debug.Log("GiveToy");
			phase = 3;
			CompletedAction();
			CleaningTool.OneToolEnabledNo = 1;
			babyC.BabySmile();
			yield return new WaitForSeconds(1.5f);
			babyC.BabyBath();
			Tutorial.Instance.ShowTutorial(2);
		}
		else if(phaseState == "Soap" && phase == 3) //SAPUN
		{
			//Debug.Log("Soap");
			CleaningTool.ActiveToolNo = 0;
			CompletedAction();

			yield return new WaitForSeconds(1);
			CleaningTool.OneToolEnabledNo = 2;
			 
			yield return new WaitForSeconds(.3f);
			phase  = 4;
			babyC.BabyBath();
			Tutorial.Instance.ShowTutorial(3);
		}
		else if(phaseState == "Shampoo") //sampon
		{
			Tutorial.Instance.ShowTutorial(4);
		}


		else if(phaseState == "ShampooHead" && phase == 4) //sampon utrljavanje
		{
			CompletedAction();
//			Debug.Log("ShampooHead");
			CleaningTool.OneToolEnabledNo = 0;
			yield return new WaitForSeconds(1);

	 
			phase = 5;
		 
			Tutorial.Instance.ShowTutorial(5);

		}
		else if(phaseState == "ShowerTap" && phase == 5)
		{
			Tutorial.Instance.StopTutorial();
			if( SoundManager.Instance!=null)  SoundManager.Instance.Play_Sound( SoundManager.Instance.Shower);
//			Debug.Log("Wash Bubbles");
			phase = 6;
			animShower.Play("WashBabyStart");
			psBabyWet1.gameObject.SetActive(true);
			psBabyWet2.gameObject.SetActive(true);
			psBabyWet1.enableEmission = true;
			psBabyWet2.enableEmission = true;

			yield return new WaitForSeconds(1);
			bubblesAnimHolder.WashBubbles();


			phase = 7;
			Tutorial.Instance.ShowTutorial(6);
		}
		else if(phaseState == "ShowerTap" && phase == 7)
		{
			Tutorial.Instance.StopTutorial();
			if( SoundManager.Instance!=null)  SoundManager.Instance.Stop_Sound( SoundManager.Instance.Shower);
			animShower.Play("WashBabyEnd");
			phase = 8;
			CompletedAction();
			yield return new WaitForSeconds(1);
			phase = 9;

			CleaningTool.OneToolEnabledNo = 3;
			Tutorial.Instance.ShowTutorial(7);
		}

		else if(phaseState == "BathtubPlug" && phase == 9)
		{
			if( SoundManager.Instance!=null)  SoundManager.Instance.Play_Sound( SoundManager.Instance.Shower);
			topMenu.HideTopMenu(); 
			CleaningTool.OneToolEnabledNo = 0;
			phase = 10;
			animBathWater.Play("Empty");
			psBabyWet3.gameObject.SetActive(true);
			psBabyWet3.enableEmission = true;
			FloatInWater.BEmptyWater =  true;
			yield return new WaitForSeconds(1);
			CompletedAction();
			yield return new WaitForSeconds(2);
			if( SoundManager.Instance!=null)  SoundManager.Instance.Stop_Sound( SoundManager.Instance.Shower);
			phase = 11;
			
			CleaningTool.OneToolEnabledNo = 4;
			Tutorial.Instance.ShowTutorial(8);
		}
 
		else if(phaseState == "Towel" && phase == 11)
		{
			CleaningTool.OneToolEnabledNo = 0;

			psBabyWet1.enableEmission = false;
			psBabyWet2.enableEmission = false;
			psBabyWet3.enableEmission = false;
			CompletedAction();
 
		}
		yield return new WaitForEndOfFrame();

		 
	}

	public void HideWaterDropsTowel()
	{
		psBabyWet2.enableEmission = false;
		psBabyWet3.enableEmission = false;
	}


	public void CompletedAction()
	{
		CompletedActionNo++;
		progressBar.SetProgress(CompletedActionNo/7f , true );
		
		if(CompletedActionNo == 7)
		{
			babyC.BabySmile();
			if( SoundManager.Instance!=null)  SoundManager.Instance.Play_Sound( SoundManager.Instance.MinigameCompleted);
			StartCoroutine("LevelCompleted");
		}
		else 
		{
			if( SoundManager.Instance!=null)  SoundManager.Instance.Play_Sound( SoundManager.Instance.ActionCompleted);
		}
	}
	
	private IEnumerator LevelCompleted()
	{
//		Debug.Log("Completed");
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

	public void ButtonNextClicked()
	{
		CompletedActionNo = 0;
		BlockClicks.Instance.SetBlockAll(true);
		if(SoundManager.Instance!=null) SoundManager.Instance.Play_ButtonClick();
		LevelTransition.Instance.HideSceneAndLoadNext("SelectMinigame");
		StopAllCoroutines();
		//Implementation.Instance.ShowInterstitial();
	}

}
