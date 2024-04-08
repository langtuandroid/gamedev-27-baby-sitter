using UnityEngine;
using System.Collections;
using TemplateScripts;
using UnityEngine.Serialization;

public class Minigame3BS : MonoBehaviour {

	[FormerlySerializedAs("topMenu")] [SerializeField] private TopMenuBS topMenuBs;
	[SerializeField] private ProgressBar progressBar;

	private static int _completedActionNoN = 0;
	
	[FormerlySerializedAs("ButtonNext")] [SerializeField] private GameObject buttonNext;
	[FormerlySerializedAs("ButtonHome")] [SerializeField] private GameObject buttonHome;

	private int phaseE = 0;

	[FormerlySerializedAs("Blanket")] [SerializeField] private ItemActionBS blanket;
	[FormerlySerializedAs("Lamp")] [SerializeField] private ItemActionBS lamp;
	[FormerlySerializedAs("Cradle")] [SerializeField] private ItemActionBS cradle;

	private bool bToyY = false;
	private bool bPacifierR = false;
	private bool bLampP = false;
	private bool bBlanketT = false;
	
	[SerializeField] private BabyControllerBs babyC;
	[SerializeField] private GameObject psSleeping;
	[SerializeField] private ParticleSystem psLevelCompleted;
	
	private void Awake()
	{
		buttonNext.SetActive(false);
	}
	
	private IEnumerator Start () {
		 topMenuBs.ShowTopMenuU();
		_completedActionNoN = 0;
		CleaningToolBS.OneToolEnabledNoN = 0;
		
		BlockClicksBs.Instance.SetBlockAllL(true);
		BlockClicksBs.Instance.SetBlockAllDelayY(.2f,false);
		
		yield return new WaitForSeconds(.1f);
		
		babyC.GetComponent<SetBabyAtlasBS>().SetBabyY(GameDataBS.GetSelectedBabyY());
		babyC.BBabyCryingIdle();

		yield return new WaitForSeconds(1);
		LevelTransitionBS.Instance.ShowSceneE();

		TutorialBS.Instance.ShowTutorial(0);
	}

	public void NextPhase( string phaseState )
	{
		StartCoroutine(nameof(WaitNextPhase),phaseState);
	}

	private IEnumerator WaitNextPhase( string phaseState )
	{
		 
		if(phaseState == "GiveToy" && !bToyY)
		{
			bToyY = true;

			phaseE ++;
			CompletedAction();
		}
		else if(phaseState == "GivePacifier" && !bPacifierR)
		{
			bPacifierR = true;

			phaseE ++;
			babyC.BBabyIdle();
			CompletedAction();
		}
		else  if(phaseState == "Blanket" && !bBlanketT)
		{
			bBlanketT = true;
			phaseE ++;
			CompletedAction();
			 
		}
		else  if(phaseState == "LampHandle" && !bLampP )
		{
			bLampP = true;
			phaseE ++;
			CompletedAction();
			 
		}
		else if(phaseState == "Cradle" && phaseE==4 )
		{
			phaseE ++;
			CompletedAction();
			cradle.bCradleCountEnabled = false;
		}

		if(phaseE == 4) cradle.bCradleCountEnabled = true;
		else cradle.bCradleCountEnabled = false;
	 
		yield return new WaitForEndOfFrame();
	}

	public void UndoPhase( string phaseState )
	{
		if(phaseState == "GiveToy" && bToyY)
		{
			bToyY = false;
			
			phaseE --;
			_completedActionNoN-=2;
			CompletedAction(false);
		}
		else if(phaseState == "GivePacifier" && bPacifierR)
		{
			bPacifierR = false;
			phaseE --;
			_completedActionNoN-=2;
			CompletedAction(false);

		}
		else  if(phaseState == "Blanket" && bBlanketT)
		{
			bBlanketT = false;
			phaseE--;
			_completedActionNoN-=2;
			CompletedAction( false);
			 
		}
		else  if(phaseState == "LampHandle" && bLampP )
		{
			bLampP = false;
			phaseE--;
			_completedActionNoN-=2;
			CompletedAction(false);
 
		}

		if(phaseE == 4) cradle.bCradleCountEnabled = true;
		else cradle.bCradleCountEnabled = false;

		ShowTutorial();
	}
	
	public void CompletedAction(bool playSound  )
	{
		_completedActionNoN++;
		progressBar.SetProgressBar(_completedActionNoN/5f , true );
		
		if(_completedActionNoN == 5)
		{
			if(playSound && SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.PlaySound( SoundManagerBS.Instance.MinigameCompleted);
			StartCoroutine(nameof(LevelCompletedD));
		}
		else 
		{
			if(playSound && SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.PlaySound( SoundManagerBS.Instance.ActionCompleted);
		}
	}

	public void BabySleepP(int state)
	{
		if(state == 1)
			babyC.BBabySleepy();
		if(state == 2)
			babyC.BBabySleepy2();
	}


	private void ShowTutorial()
	{
		if(!bToyY )TutorialBS.Instance.ShowTutorial(0);
		else if(!bPacifierR )TutorialBS.Instance.ShowTutorial(1);
		else if(!bBlanketT )TutorialBS.Instance.ShowTutorial(2);
		else if(!bLampP )TutorialBS.Instance.ShowTutorial(3);
		else  TutorialBS.Instance.ShowTutorial(4);
	}

	public void CompletedAction( )
	{
		_completedActionNoN++;
		progressBar.SetProgressBar(_completedActionNoN/5f , true );



		if(_completedActionNoN == 5)
		{
			babyC.BBabySleeping();
			psSleeping.SetActive(true);
			if( SoundManagerBS.Instance!=null)SoundManagerBS.Instance.PlaySound( SoundManagerBS.Instance.MinigameCompleted);
			StartCoroutine(nameof(LevelCompletedD));
		}
		else 
		{
			ShowTutorial();
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
		buttonNext.SetActive(true);
		buttonHome.SetActive(false);
	}
	
	public void ButtonHomeClickedD( )
	{
		_completedActionNoN = 0;
		psSleeping.GetComponent<ParticleSystem>().enableEmission = false;
		if(SoundManagerBS.Instance!=null) SoundManagerBS.Instance.Play_ButtonClickK();
		StopAllCoroutines();
		GameDataBS.BCompletedMiniGameE = false;
		LevelTransitionBS.Instance.HideSceneAndLoadNextT("SelectMinigame"); 
		BlockClicksBs.Instance.SetBlockAllL(true);
	}

	public void ButtonNextClickedD()
	{
		_completedActionNoN = 0;
		psSleeping.GetComponent<ParticleSystem>().enableEmission = false;
		BlockClicksBs.Instance.SetBlockAllL(true);
		if(SoundManagerBS.Instance!=null) SoundManagerBS.Instance.Play_ButtonClickK();
		LevelTransitionBS.Instance.HideSceneAndLoadNextT("SelectMinigame");
		StopAllCoroutines();
	}

}
