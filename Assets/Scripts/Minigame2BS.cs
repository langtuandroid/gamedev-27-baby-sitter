using UnityEngine;
using System.Collections;
using TemplateScripts;
using UnityEngine.Serialization;

public class Minigame2BS : MonoBehaviour {
	
	[FormerlySerializedAs("topMenuU")] [FormerlySerializedAs("topMenu")] [SerializeField] private TopMenuBS topMenuBsU;
	[FormerlySerializedAs("progressBar")] [SerializeField] private ProgressBar progressBarR;
	
	public static int CompletedActionNoN = 0;
	
	[FormerlySerializedAs("buttonNext")] [SerializeField] private GameObject buttonNextT;
	[FormerlySerializedAs("buttonHome")] [SerializeField] private GameObject buttonHomeE;

	[FormerlySerializedAs("animShower")] [SerializeField] private Animator animShowerR;
	[FormerlySerializedAs("animBathWater")] [SerializeField] private Animator animBathWaterR;

	[FormerlySerializedAs("psBabyWet1")] [SerializeField] private ParticleSystem psBabyWetT1;
	[FormerlySerializedAs("psBabyWet2")] [SerializeField] private ParticleSystem psBabyWetT2;
	[FormerlySerializedAs("psBabyWet3")] [SerializeField] private ParticleSystem psBabyWetT3;
	
	private int phaseE;

	[SerializeField] private GameObject shampooHeadHolder;

	private BubblesListHolder bubblesAnimHolderR;

	[SerializeField] private BabyControllerBs babyC;
	[SerializeField] private ParticleSystem psLevelCompleted;
	 

	private void Awake()
	{
		buttonNextT.SetActive(false);
		bubblesAnimHolderR = transform.GetComponent<BubblesListHolder>();
	}
	
	private IEnumerator Start () {
		CompletedActionNoN = 0;
		CleaningToolBS.OneToolEnabledNoN = 0;
		
		BlockClicksBs.Instance.SetBlockAllL(true);
		BlockClicksBs.Instance.SetBlockAllDelayY(.2f,false);
		
		yield return new WaitForSeconds(.1f);
		
		babyC.GetComponent<SetBabyAtlasBS>().SetBabyY(GameDataBS.GetSelectedBabyY());
		babyC.BabyBath();
		
		yield return new WaitForSeconds(1);
		LevelTransitionBS.Instance.ShowScene();
		 
		TutorialBS.Instance.ShowTutorial(0);
	}


	public void NextPhaseE( string phaseState )
	{
		StartCoroutine(nameof(WaitNextPhaseE),phaseState);
	}

	private IEnumerator WaitNextPhaseE( string phaseState )
	{
		if(phaseState == "ShowerTap" && phaseE == 0)
		{
			TutorialBS.Instance.StopTutor();
			phaseE = 1;
			CleaningToolBS.OneToolEnabledNoN = 0;

			animShowerR.Play("FillWater");
			animBathWaterR.gameObject.SetActive(true);
			animBathWaterR.Play("Rise");
			yield return new WaitForSeconds(.1f);
			if( SoundManagerBS.Instance!=null) 
			{
				SoundManagerBS.Instance.PlaySound( SoundManagerBS.Instance.ShowerR);
				SoundManagerBS.Instance.listStopSoundOnExit.Add( SoundManagerBS.Instance.ShowerR);
			}
			yield return new WaitForSeconds(2);
			CompletedActionN();

			yield return new WaitForSeconds(1.8f);
			if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.StopSound( SoundManagerBS.Instance.ShowerR); 
			phaseE = 2;
			
			topMenuBsU.ShowTopMenuU(); 

			yield return new WaitForSeconds(1);
			TutorialBS.Instance.ShowTutorial(1);
		}
		else if(phaseState == "GiveToy" && phaseE == 2)
		{
			phaseE = 3;
			CompletedActionN();
			CleaningToolBS.OneToolEnabledNoN = 1;
			babyC.BabySmile();
			yield return new WaitForSeconds(1.5f);
			babyC.BabyBath();
			TutorialBS.Instance.ShowTutorial(2);
		}
		else if(phaseState == "Soap" && phaseE == 3)
		{
			CleaningToolBS.ActiveToolNo = 0;
			CompletedActionN();

			yield return new WaitForSeconds(1);
			CleaningToolBS.OneToolEnabledNoN = 2;
			 
			yield return new WaitForSeconds(.3f);
			phaseE  = 4;
			babyC.BabyBath();
			TutorialBS.Instance.ShowTutorial(3);
		}
		else if(phaseState == "Shampoo") 
		{
			TutorialBS.Instance.ShowTutorial(4);
		}


		else if(phaseState == "ShampooHead" && phaseE == 4) 
		{
			CompletedActionN();
			CleaningToolBS.OneToolEnabledNoN = 0;
			yield return new WaitForSeconds(1);

	 
			phaseE = 5;
		 
			TutorialBS.Instance.ShowTutorial(5);

		}
		else if(phaseState == "ShowerTap" && phaseE == 5)
		{
			TutorialBS.Instance.StopTutor();
			if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.PlaySound( SoundManagerBS.Instance.ShowerR);
			phaseE = 6;
			animShowerR.Play("WashBabyStart");
			psBabyWetT1.gameObject.SetActive(true);
			psBabyWetT2.gameObject.SetActive(true);
			psBabyWetT1.enableEmission = true;
			psBabyWetT2.enableEmission = true;

			yield return new WaitForSeconds(1);
			bubblesAnimHolderR.WashBubbles();


			phaseE = 7;
			TutorialBS.Instance.ShowTutorial(6);
		}
		else if(phaseState == "ShowerTap" && phaseE == 7)
		{
			TutorialBS.Instance.StopTutor();
			if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.StopSound( SoundManagerBS.Instance.ShowerR);
			animShowerR.Play("WashBabyEnd");
			phaseE = 8;
			CompletedActionN();
			yield return new WaitForSeconds(1);
			phaseE = 9;

			CleaningToolBS.OneToolEnabledNoN = 3;
			TutorialBS.Instance.ShowTutorial(7);
		}

		else if(phaseState == "BathtubPlug" && phaseE == 9)
		{
			if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.PlaySound( SoundManagerBS.Instance.ShowerR);
			topMenuBsU.HideTopMenuU(); 
			CleaningToolBS.OneToolEnabledNoN = 0;
			phaseE = 10;
			animBathWaterR.Play("Empty");
			psBabyWetT3.gameObject.SetActive(true);
			psBabyWetT3.enableEmission = true;
			FloatInWaterBS.BEmptyWaterR =  true;
			yield return new WaitForSeconds(1);
			CompletedActionN();
			yield return new WaitForSeconds(2);
			if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.StopSound( SoundManagerBS.Instance.ShowerR);
			phaseE = 11;
			
			CleaningToolBS.OneToolEnabledNoN = 4;
			TutorialBS.Instance.ShowTutorial(8);
		}
 
		else if(phaseState == "Towel" && phaseE == 11)
		{
			CleaningToolBS.OneToolEnabledNoN = 0;

			psBabyWetT1.enableEmission = false;
			psBabyWetT2.enableEmission = false;
			psBabyWetT3.enableEmission = false;
			CompletedActionN();
 
		}
		yield return new WaitForEndOfFrame();

		 
	}

	public void HideWaterDropsTowelL()
	{
		psBabyWetT2.enableEmission = false;
		psBabyWetT3.enableEmission = false;
	}


	public void CompletedActionN()
	{
		CompletedActionNoN++;
		progressBarR.SetProgressBar(CompletedActionNoN/7f , true );
		
		if(CompletedActionNoN == 7)
		{
			babyC.BabySmile();
			if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.PlaySound( SoundManagerBS.Instance.MinigameCompleted);
			StartCoroutine(nameof(LevelCompletedD));
		}
		else 
		{
			if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.PlaySound( SoundManagerBS.Instance.ActionCompleted);
		}
	}
	
	private IEnumerator LevelCompletedD()
	{
		GameDataBS.BCompletedMiniGameE = true;
		psLevelCompleted.gameObject.SetActive(true);
		yield return new WaitForEndOfFrame();
		psLevelCompleted.Stop();
		psLevelCompleted.Play();
		
		yield return new WaitForSeconds(1);
		buttonNextT.SetActive(true);
		buttonHomeE.SetActive(false);
	}

	public void ButtonHomeClickedD( )
	{
		CompletedActionNoN = 0;
		if(SoundManagerBS.Instance!=null) SoundManagerBS.Instance.Play_ButtonClickK();
		StopAllCoroutines();
		GameDataBS.BCompletedMiniGameE = false;
		LevelTransitionBS.Instance.HideSceneAndLoadNext("SelectMinigame"); 
		
		BlockClicksBs.Instance.SetBlockAllL(true);
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
