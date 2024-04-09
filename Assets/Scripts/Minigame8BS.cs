using UnityEngine;
using System.Collections;
using TemplateScripts;
using UnityEngine.Serialization;

public class Minigame8BS : MonoBehaviour {

	[FormerlySerializedAs("topMenu")] [SerializeField] private TopMenuBS topMenuBs;
	[SerializeField] private ProgressBar progressBar;
	
	private static int _completedActionNo = 0;
	
	[FormerlySerializedAs("ButtonNext")] [SerializeField] private GameObject buttonNext;
	[FormerlySerializedAs("ButtonHome")] [SerializeField] private GameObject buttonHome;
	
	[SerializeField] private Animator animGramophone;
	[SerializeField] private GameObject psGramophone1;
	[SerializeField] private GameObject psGramophone2;
	
	private string playingRecordD = "";
 
	private int phaseE = 0;
	private int playingRecordNoN = 0;
	private float recordPlayingTimeE = 0;
	private bool bLampP;

	[FormerlySerializedAs("RecordsSounds")] [SerializeField] private AudioSource[] recordsSounds;
	[FormerlySerializedAs("InstrumentsSounds")] [SerializeField] private AudioSource[] instrumentsSounds;

	[SerializeField] private BabyControllerBs babyC;
	[SerializeField] private GameObject psSleeping;
	[SerializeField] private ParticleSystem psLevelCompleted;

	private void Awake()
	{
		buttonNext.SetActive(false);
	}
	
	private IEnumerator Start () 
	{
	 	topMenuBs.ShowTopMenuU();
		 
		_completedActionNo = 0;
	 

		 BlockClicksBs.Instance.SetBlockAllL(true);
		 BlockClicksBs.Instance.SetBlockAllDelayY(.2f,false);
		
		yield return new WaitForSeconds(.1f);
	
		babyC.GetComponent<SetBabyAtlasBS>().SetBabyY(GameDataBS.GetSelectedBabyY());
		babyC.BabyCryingIdle();

		yield return new WaitForSeconds(1);
		LevelTransitionBS.Instance.ShowScene();

		TutorialBS.Instance.ShowTutorial(0);
	}
	

	private void Update () {
		if(playingRecordNoN>0 )
		{
		   	recordPlayingTimeE +=Time.deltaTime;
			if(phaseE == 0)
			{
				babyC.BabySmile();
				phaseE++;
			}
			else 	if(phaseE == 1 && recordPlayingTimeE>3)
			{
				babyC.BabySleepy();
				phaseE++;
			}
			else 	if(phaseE == 2 && recordPlayingTimeE>8)
			{
				babyC.BabySleepy2();
				phaseE++;
			}


			progressBar.SetProgressBar(recordPlayingTimeE/15f ,false );
			if(recordPlayingTimeE >=15  && _completedActionNo== 0)
			{
				if(!psSleeping.activeSelf)
				{
					babyC.BabySleeping();
					psSleeping.SetActive(true);
				}
				_completedActionNo = 1;
				CompletedActionN();
			}
		}
	}

	
	public void NextPhaseE( string phaseState )
	{
		if(phaseState == "LampHandle") bLampP = true;
		CompletedActionN();
	}

	public void UndoPhaseE( string phaseState )
	{
		if(phaseState == "LampHandle") bLampP = false;
	 
	}

	public void CompletedActionN()
	{
		if(_completedActionNo == 1 && bLampP)
		{
			GameObject.Find("LampHandle").GetComponent<ItemActionBS>().bEnabled = false;
			StartCoroutine(nameof(LevelCompletedD));
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
	
	public void ButtonRattleClickedD()
	{
		_completedActionNo = 0;
		if(SoundManagerBS.Instance != null) SoundManagerBS.Instance.StopAndPlay_Sound( instrumentsSounds[0]);
	}

	public void ButtonChumesClickedD()
	{
		if(SoundManagerBS.Instance != null) SoundManagerBS.Instance.StopAndPlay_Sound( instrumentsSounds[1]);
	}

	public void ButtonTriangleClickedD()
	{
		if(SoundManagerBS.Instance != null) SoundManagerBS.Instance.StopAndPlay_Sound(instrumentsSounds[2]);
	}

	public void StopRecordD()
	{
		if(  SoundManagerBS.Instance!=null)
		{
			switch(playingRecordNoN)
			{
			case 1:
				SoundManagerBS.Instance.StopSound( recordsSounds[0]);
				break;
			case 2:
				SoundManagerBS.Instance.StopSound( recordsSounds[1]);
				break;
			case 3:
				SoundManagerBS.Instance.StopSound( recordsSounds[2]);
				break;
			case 4:
				SoundManagerBS.Instance.StopSound(recordsSounds[3]);
				break;
			case 5:
				SoundManagerBS.Instance.StopSound( recordsSounds[4]);
				break;
			}
		}
	}
	public void PlayRecordD(string name)
	{
		TutorialBS.Instance.StopTutor();
		if(!psGramophone1.activeSelf)
		{
			animGramophone.Play("playMusic");
			psGramophone1.SetActive(true);
			psGramophone2.SetActive(true);
		}
		if(  SoundManagerBS.Instance!=null)
		{

			SoundManagerBS.Instance.ChangeSoundVolume(SoundManagerBS.Instance.GameplayMusic,.3f,.0f);
			StopRecordD();
		

			switch(name)
			{
			case "M07_01":
				SoundManagerBS.Instance.PlaySound( recordsSounds[0]);
				playingRecordNoN = 1;
				break;
			case "M07_02":
				SoundManagerBS.Instance.PlaySound( recordsSounds[1]);
				playingRecordNoN = 2;
				break;
			case "M07_03":
				SoundManagerBS.Instance.PlaySound(recordsSounds[2]);
				playingRecordNoN = 3;
				break;
			case "M07_04":
				SoundManagerBS.Instance.PlaySound( recordsSounds[3]);
				playingRecordNoN = 4;
				break;
			case "M07_05":
				SoundManagerBS.Instance.PlaySound( recordsSounds[4]);
				playingRecordNoN = 5;
				break;
			}

			 
		}
	}

	public void ButtonHomeClickedD( )
	{
		psGramophone1.GetComponent<ParticleSystem>().enableEmission = false;
		psGramophone2.GetComponent<ParticleSystem>().enableEmission = false;
		psSleeping.GetComponent<ParticleSystem>().enableEmission = false;
		StopRecordD();
		SoundManagerBS.Instance.ChangeSoundVolume(SoundManagerBS.Instance.GameplayMusic,.3f,SoundManagerBS.Instance.OriginalMusicVolumeE);
		
		if(SoundManagerBS.Instance!=null) SoundManagerBS.Instance.Play_ButtonClickK();
		StopAllCoroutines();
		GameDataBS.BCompletedMiniGameE = false;
		LevelTransitionBS.Instance.HideSceneAndLoadNext("SelectMinigame"); 
		BlockClicksBs.Instance.SetBlockAllL(true);
		//Implementation.Instance.ShowInterstitial();
	}

	public void ButtonNextClickedD()
	{
		_completedActionNo = 0;
		psGramophone1.GetComponent<ParticleSystem>().enableEmission = false;
		psGramophone2.GetComponent<ParticleSystem>().enableEmission = false;
		psSleeping.GetComponent<ParticleSystem>().enableEmission = false;
		StopRecordD();
		SoundManagerBS.Instance.ChangeSoundVolume(SoundManagerBS.Instance.GameplayMusic,.3f,SoundManagerBS.Instance.OriginalMusicVolumeE);
		
		if(SoundManagerBS.Instance!=null) SoundManagerBS.Instance.Play_ButtonClickK();
		BlockClicksBs.Instance.SetBlockAllL(true);
		LevelTransitionBS.Instance.HideSceneAndLoadNext("SelectMinigame");
		StopAllCoroutines();
		//Implementation.Instance.ShowInterstitial();
	}

}
