using UnityEngine;
using System.Collections;
using TemplateScripts;
using UnityEngine.Serialization;

public class Minigame3 : MonoBehaviour {

	[SerializeField] private TopMenu topMenu;
	[SerializeField] private ProgressBar progressBar;

	private static int _completedActionNo = 0;
	
	[FormerlySerializedAs("ButtonNext")] [SerializeField] private GameObject buttonNext;
	[FormerlySerializedAs("ButtonHome")] [SerializeField] private GameObject buttonHome;

	private int phase = 0;

	[FormerlySerializedAs("Blanket")] [SerializeField] private ItemAction blanket;
	[FormerlySerializedAs("Lamp")] [SerializeField] private ItemAction lamp;
	[FormerlySerializedAs("Cradle")] [SerializeField] private ItemAction cradle;

	private bool bToy = false;
	private bool bPacifier = false;
	private bool bLamp = false;
	private bool bBlanket = false;
	
	[SerializeField] private BabyController babyC;
	[SerializeField] private GameObject psSleeping;
	[SerializeField] private ParticleSystem psLevelCompleted;
	
	private void Awake()
	{
		buttonNext.SetActive(false);
	}
	
	private IEnumerator Start () {
		 topMenu.ShowTopMenu();
		_completedActionNo = 0;
		CleaningTool.OneToolEnabledNo = 0;
		
		BlockClicks.Instance.SetBlockAll(true);
		BlockClicks.Instance.SetBlockAllDelay(.2f,false);
		
		
		yield return new WaitForSeconds(.1f);

		
		//podesi bebicu 
		//Debug.Log("Selektovana bebica:  " +  (GameData.selectedMinigameIndex+1) );
		babyC.GetComponent<SetBabyAtlas>().SetBaby(GameData.GetSelectedBaby());
		babyC.BabyCryingIdle();

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
		 
		if(phaseState == "GiveToy" && !bToy)
		{
			bToy = true;
			//Debug.Log("GiveToy");
			phase ++;
			CompletedAction();
		}
		else if(phaseState == "GivePacifier" && !bPacifier)
		{
			bPacifier = true;
			//Debug.Log("GivePacifier");
			phase ++;
			babyC.BabyIdle();
			CompletedAction();
		}
		else  if(phaseState == "Blanket" && !bBlanket)
		{
			bBlanket = true;
			phase ++;
			CompletedAction();
			 
		}
		else  if(phaseState == "LampHandle" && !bLamp )
		{
			bLamp = true;
			phase ++;
			CompletedAction();
			 
		}
		else if(phaseState == "Cradle" && phase==4 )
		{
			phase ++;
			CompletedAction();
			cradle.bCradleCountEnabled = false;
		}

		if(phase == 4) cradle.bCradleCountEnabled = true;
		else cradle.bCradleCountEnabled = false;
	 
		yield return new WaitForEndOfFrame();
	}

	public void UndoPhase( string phaseState )
	{
		if(phaseState == "GiveToy" && bToy)
		{
			bToy = false;
			//Debug.Log("GiveToy");
			phase --;
			_completedActionNo-=2;
			CompletedAction(false);
		}
		else if(phaseState == "GivePacifier" && bPacifier)
		{
			bPacifier = false;
			phase --;
			_completedActionNo-=2;
			CompletedAction(false);

		}
		else  if(phaseState == "Blanket" && bBlanket)
		{
			bBlanket = false;
			phase--;
			_completedActionNo-=2;
			CompletedAction( false);
			 
		}
		else  if(phaseState == "LampHandle" && bLamp )
		{
			bLamp = false;
			phase--;
			_completedActionNo-=2;
			CompletedAction(false);
 
		}

		if(phase == 4) cradle.bCradleCountEnabled = true;
		else cradle.bCradleCountEnabled = false;

		ShowTut();
	}
	
	public void CompletedAction(bool playSound  )
	{
		_completedActionNo++;
		progressBar.SetProgress(_completedActionNo/5f , true );
		
		if(_completedActionNo == 5)
		{
			if(playSound && SoundManager.Instance!=null)  SoundManager.Instance.Play_Sound( SoundManager.Instance.MinigameCompleted);
			StartCoroutine(nameof(LevelCompleted));
		}
		else 
		{
			if(playSound && SoundManager.Instance!=null)  SoundManager.Instance.Play_Sound( SoundManager.Instance.ActionCompleted);
		}
	}

	public void BabySleep(int state)
	{
		if(state == 1)
			babyC.BabySleepy();
		if(state == 2)
			babyC.BabySleepy2();
	}


	private void ShowTut()
	{
		if(!bToy )Tutorial.Instance.ShowTutorial(0);
		else if(!bPacifier )Tutorial.Instance.ShowTutorial(1);
		else if(!bBlanket )Tutorial.Instance.ShowTutorial(2);
		else if(!bLamp )Tutorial.Instance.ShowTutorial(3);
		else  Tutorial.Instance.ShowTutorial(4);
	}

	public void CompletedAction( )
	{
		_completedActionNo++;
		progressBar.SetProgress(_completedActionNo/5f , true );



		if(_completedActionNo == 5)
		{
			babyC.BabySleeping();
			psSleeping.SetActive(true);
			if( SoundManager.Instance!=null)SoundManager.Instance.Play_Sound( SoundManager.Instance.MinigameCompleted);
			StartCoroutine("LevelCompleted");
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
		_completedActionNo = 0;
		psSleeping.GetComponent<ParticleSystem>().enableEmission = false;
		if(SoundManager.Instance!=null) SoundManager.Instance.Play_ButtonClick();
		StopAllCoroutines();
		GameData.BCompletedMiniGame = false;
		LevelTransition.Instance.HideSceneAndLoadNext("SelectMinigame"); 
		BlockClicks.Instance.SetBlockAll(true);
		//Implementation.Instance.ShowInterstitial();
	}

	public void ButtonNextClicked()
	{
		_completedActionNo = 0;
		psSleeping.GetComponent<ParticleSystem>().enableEmission = false;
		BlockClicks.Instance.SetBlockAll(true);
		if(SoundManager.Instance!=null) SoundManager.Instance.Play_ButtonClick();
		LevelTransition.Instance.HideSceneAndLoadNext("SelectMinigame");
		StopAllCoroutines();
		//Implementation.Instance.ShowInterstitial();
	}

}
