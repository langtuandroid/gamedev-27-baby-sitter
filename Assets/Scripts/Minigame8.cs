using UnityEngine;
using System.Collections;
using TemplateScripts;

public class Minigame8 : MonoBehaviour {

	public TopMenu topMenu;
	public ProgressBar progressBar;
	public static int CompletedActionNo = 0;
	public GameObject ButtonNext;
	public GameObject ButtonHome;
	
	public Animator animGramophone;
	public GameObject psGramophone1;
	public GameObject psGramophone2;
	private string playingRecord = "";
 
	int phase = 0;
	int  PlayingRecordNo = 0;
	float recordPlayingTime = 0;
	bool bLamp;

	public AudioSource[] RecordsSounds;
	public AudioSource[] InstrumentsSounds;

	public BabyController babyC;
	public GameObject psSleeping;
	public ParticleSystem psLevelCompleted;

	void Awake()
	{
		ButtonNext.SetActive(false);
	 
	}
	
	IEnumerator Start () 
	{
	 	topMenu.ShowTopMenu();
		 
		CompletedActionNo = 0;
	 

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
	

	void Update () {
		if(PlayingRecordNo>0 )
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
			if(recordPlayingTime >=15  && CompletedActionNo== 0)
			{
				if(!psSleeping.activeSelf)
				{
					babyC.BabySleeping();
					psSleeping.SetActive(true);
				}
				CompletedActionNo = 1;
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
	
	
	
//	IEnumerator WaitNextPhase( string phaseState )
//	{
//		Debug.Log(phaseState +"   @");
// 
//		yield return new WaitForEndOfFrame();
//	}
	
	
	
	
	
	public void CompletedAction()
	{
 
		if(CompletedActionNo == 1 && bLamp)
		{
			GameObject.Find("LampHandle").GetComponent<ItemAction>().bEnabled = false;
			//if( SoundManager.Instance!=null)  SoundManager.Instance.Play_Sound( SoundManager.Instance.MinigameCompleted);
			StartCoroutine("LevelCompleted");
		}
		else 
		{
			//if( SoundManager.Instance!=null)  SoundManager.Instance.Play_Sound( SoundManager.Instance.ActionCompleted);
		}
	}
	
	
	IEnumerator LevelCompleted()
	{
		//Debug.Log("Completed");
		GameData.BCompletedMiniGame = true;
		psLevelCompleted.gameObject.SetActive(true);
		yield return new WaitForEndOfFrame();
		psLevelCompleted.Stop();
		psLevelCompleted.Play();
		
		yield return new WaitForSeconds(1);
		ButtonNext.SetActive(true);
		ButtonHome.SetActive(false);
	}
	


	public void  ButtonRattleClicked()
	{
		CompletedActionNo = 0;
		if(SoundManager.Instance != null) SoundManager.Instance.StopAndPlay_Sound( InstrumentsSounds[0]);
	}

	public void  ButtonChymesClicked()
	{
		if(SoundManager.Instance != null) SoundManager.Instance.StopAndPlay_Sound( InstrumentsSounds[1]);
	}

	public void  ButtonTriangleClicked()
	{
		if(SoundManager.Instance != null) SoundManager.Instance.StopAndPlay_Sound(InstrumentsSounds[2]);
	}

	public void StopRecord()
	{
		if(  SoundManager.Instance!=null)
		{
			switch(PlayingRecordNo)
			{
			case 1:
				SoundManager.Instance.Stop_Sound( RecordsSounds[0]);
				break;
			case 2:
				SoundManager.Instance.Stop_Sound( RecordsSounds[1]);
				break;
			case 3:
				SoundManager.Instance.Stop_Sound( RecordsSounds[2]);
				break;
			case 4:
				SoundManager.Instance.Stop_Sound(RecordsSounds[3]);
				break;
			case 5:
				SoundManager.Instance.Stop_Sound( RecordsSounds[4]);
				break;
			}
		}
	}
	public void PlayRecord(string name)
	{
		Tutorial.Instance.StopTutorial();
		//Debug.Log("PLAY RECORD");
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
				SoundManager.Instance.Play_Sound( RecordsSounds[0]);
				PlayingRecordNo = 1;
				break;
			case "M07_02":
				SoundManager.Instance.Play_Sound( RecordsSounds[1]);
				PlayingRecordNo = 2;
				break;
			case "M07_03":
				SoundManager.Instance.Play_Sound(RecordsSounds[2]);
				PlayingRecordNo = 3;
				break;
			case "M07_04":
				SoundManager.Instance.Play_Sound( RecordsSounds[3]);
				PlayingRecordNo = 4;
				break;
			case "M07_05":
				SoundManager.Instance.Play_Sound( RecordsSounds[4]);
				PlayingRecordNo = 5;
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

	public void  ButtonNextClicked()
	{
		CompletedActionNo = 0;
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
