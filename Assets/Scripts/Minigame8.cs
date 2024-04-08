using UnityEngine;
using System.Collections;
using TemplateScripts;
using UnityEngine.Serialization;

public class Minigame8 : MonoBehaviour {

	[SerializeField] private TopMenu topMenu;
	[SerializeField] private ProgressBar progressBar;
	
	private static int _completedActionNo = 0;
	
	[FormerlySerializedAs("ButtonNext")] [SerializeField] private GameObject buttonNext;
	[FormerlySerializedAs("ButtonHome")] [SerializeField] private GameObject buttonHome;
	
	[SerializeField] private Animator animGramophone;
	[SerializeField] private GameObject psGramophone1;
	[SerializeField] private GameObject psGramophone2;
	
	private string playingRecord = "";
 
	private int phase = 0;
	private int playingRecordNo = 0;
	private float recordPlayingTime = 0;
	private bool bLamp;

	[FormerlySerializedAs("RecordsSounds")] [SerializeField] private AudioSource[] recordsSounds;
	[FormerlySerializedAs("InstrumentsSounds")] [SerializeField] private AudioSource[] instrumentsSounds;

	[SerializeField] private BabyController babyC;
	[SerializeField] private GameObject psSleeping;
	[SerializeField] private ParticleSystem psLevelCompleted;

	private void Awake()
	{
		buttonNext.SetActive(false);
	}
	
	private IEnumerator Start () 
	{
	 	topMenu.ShowTopMenu();
		 
		_completedActionNo = 0;
	 

		 BlockClicks.Instance.SetBlockAll(true);
		 BlockClicks.Instance.SetBlockAllDelay(.2f,false);
		
		yield return new WaitForSeconds(.1f);
	
		babyC.GetComponent<SetBabyAtlas>().SetBaby(GameData.GetSelectedBaby());
		babyC.BabyCryingIdle();

		yield return new WaitForSeconds(1);
		LevelTransition.Instance.ShowScene();

		Tutorial.Instance.ShowTutorial(0);
	}
	

	private void Update () {
		if(playingRecordNo>0 )
		{
		   	recordPlayingTime +=Time.deltaTime;
			if(phase == 0)
			{
				babyC.BabySmile();
				phase++;
			}
			else 	if(phase == 1 && recordPlayingTime>3)
			{
				babyC.BabySleepy();
				phase++;
			}
			else 	if(phase == 2 && recordPlayingTime>8)
			{
				babyC.BabySleepy2();
				phase++;
			}


			progressBar.SetProgress(recordPlayingTime/15f ,false );
			if(recordPlayingTime >=15  && _completedActionNo== 0)
			{
				if(!psSleeping.activeSelf)
				{
					babyC.BabySleeping();
					psSleeping.SetActive(true);
				}
				_completedActionNo = 1;
				CompletedAction();
			}
		}
	}

	
	public void NextPhase( string phaseState )
	{
		if(phaseState == "LampHandle") bLamp = true;
		CompletedAction();
	}

	public void UndoPhase( string phaseState )
	{
		if(phaseState == "LampHandle") bLamp = false;
	 
	}

	public void CompletedAction()
	{
		if(_completedActionNo == 1 && bLamp)
		{
			GameObject.Find("LampHandle").GetComponent<ItemAction>().bEnabled = false;
			StartCoroutine(nameof(LevelCompleted));
		}
	
	}
	
	
	private IEnumerator LevelCompleted()
	{
		GameData.BCompletedMiniGame = true;
		psLevelCompleted.gameObject.SetActive(true);
		yield return new WaitForEndOfFrame();
		psLevelCompleted.Stop();
		psLevelCompleted.Play();
		
		yield return new WaitForSeconds(1);
		buttonNext.SetActive(true);
		buttonHome.SetActive(false);
	}
	


	public void ButtonRattleClicked()
	{
		_completedActionNo = 0;
		if(SoundManager.Instance != null) SoundManager.Instance.StopAndPlay_Sound( instrumentsSounds[0]);
	}

	public void ButtonChymesClicked()
	{
		if(SoundManager.Instance != null) SoundManager.Instance.StopAndPlay_Sound( instrumentsSounds[1]);
	}

	public void ButtonTriangleClicked()
	{
		if(SoundManager.Instance != null) SoundManager.Instance.StopAndPlay_Sound(instrumentsSounds[2]);
	}

	public void StopRecord()
	{
		if(  SoundManager.Instance!=null)
		{
			switch(playingRecordNo)
			{
			case 1:
				SoundManager.Instance.Stop_Sound( recordsSounds[0]);
				break;
			case 2:
				SoundManager.Instance.Stop_Sound( recordsSounds[1]);
				break;
			case 3:
				SoundManager.Instance.Stop_Sound( recordsSounds[2]);
				break;
			case 4:
				SoundManager.Instance.Stop_Sound(recordsSounds[3]);
				break;
			case 5:
				SoundManager.Instance.Stop_Sound( recordsSounds[4]);
				break;
			}
		}
	}
	public void PlayRecord(string name)
	{
		Tutorial.Instance.StopTutorial();
		if(!psGramophone1.activeSelf)
		{
			animGramophone.Play("playMusic");
			psGramophone1.SetActive(true);
			psGramophone2.SetActive(true);
		}
		if(  SoundManager.Instance!=null)
		{

			SoundManager.Instance.ChangeSoundVolume(SoundManager.Instance.GameplayMusic,.3f,.0f);
			StopRecord();
		

			switch(name)
			{
			case "M07_01":
				SoundManager.Instance.Play_Sound( recordsSounds[0]);
				playingRecordNo = 1;
				break;
			case "M07_02":
				SoundManager.Instance.Play_Sound( recordsSounds[1]);
				playingRecordNo = 2;
				break;
			case "M07_03":
				SoundManager.Instance.Play_Sound(recordsSounds[2]);
				playingRecordNo = 3;
				break;
			case "M07_04":
				SoundManager.Instance.Play_Sound( recordsSounds[3]);
				playingRecordNo = 4;
				break;
			case "M07_05":
				SoundManager.Instance.Play_Sound( recordsSounds[4]);
				playingRecordNo = 5;
				break;
			}

			 
		}
	}

	public void ButtonHomeClicked( )
	{
		psGramophone1.GetComponent<ParticleSystem>().enableEmission = false;
		psGramophone2.GetComponent<ParticleSystem>().enableEmission = false;
		psSleeping.GetComponent<ParticleSystem>().enableEmission = false;
		StopRecord();
		SoundManager.Instance.ChangeSoundVolume(SoundManager.Instance.GameplayMusic,.3f,SoundManager.Instance.OriginalMusicVolume);
		
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
		psGramophone1.GetComponent<ParticleSystem>().enableEmission = false;
		psGramophone2.GetComponent<ParticleSystem>().enableEmission = false;
		psSleeping.GetComponent<ParticleSystem>().enableEmission = false;
		StopRecord();
		SoundManager.Instance.ChangeSoundVolume(SoundManager.Instance.GameplayMusic,.3f,SoundManager.Instance.OriginalMusicVolume);
		
		if(SoundManager.Instance!=null) SoundManager.Instance.Play_ButtonClick();
		BlockClicks.Instance.SetBlockAll(true);
		LevelTransition.Instance.HideSceneAndLoadNext("SelectMinigame");
		StopAllCoroutines();
		//Implementation.Instance.ShowInterstitial();
	}

}
