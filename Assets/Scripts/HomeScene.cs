using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TemplateScripts;
using UnityEngine.Serialization;


public class HomeScene : MonoBehaviour {
	
	[SerializeField] private Image soundOff;
	[SerializeField] private Image soundOn;
 
	[SerializeField] private MenuManager menuManager;
	
	[SerializeField] private GameObject pupUpShop;
	[SerializeField] private GameObject pupUpMoreGames;
	
	[SerializeField] private BabyController babyC; 
	
	private void Awake()
	{
		Input.multiTouchEnabled = false;
	}

	private IEnumerator Start () {

		GameData.SelectedMiniGameIndex =-1;
		
		BlockClicks.Instance.SetBlockAll(true);
		BlockClicks.Instance.SetBlockAllDelay(1.5f,false);
 
	 
		if(SoundManager.SoundOn == 1)
		{
			soundOff.enabled = false;
			soundOn.enabled = true;
		}
		else
		{
			soundOff.enabled = true;
			soundOn.enabled = false;
		}

		babyC.BabyIdle();

		LevelTransition.Instance.ShowScene();
		yield return new WaitForSeconds(1);
	}

	
	public void ExitGame () 
    {
		Application.Quit();
	}

 
	public void BtnSoundClicked()
	{
		if(SoundManager.SoundOn == 1)
		{
			soundOff.enabled = true;
			 soundOn.enabled = false;
			SoundManager.SoundOn = 0;
			SoundManager.Instance.MuteAllSounds();

		}
		else
		{
			soundOff.enabled = false;
			 soundOn.enabled = true;
			SoundManager.SoundOn = 1;
			SoundManager.Instance.UnmuteAllSounds();
			if(SoundManager.Instance!=null) SoundManager.Instance.Play_ButtonClick();

		}

		 
		if(SoundManager.MusicOn == 1)
		{
			SoundManager.Instance.Stop_Music();
			SoundManager.MusicOn = 0;
		}
		else
		{
			SoundManager.MusicOn = 1;
			SoundManager.Instance.Play_Music();
		}

		PlayerPrefs.SetInt("SoundOn",SoundManager.SoundOn);
		PlayerPrefs.SetInt("MusicOn",SoundManager.MusicOn);
		PlayerPrefs.Save();

	 
		BlockClicks.Instance.SetBlockAll(true);
		BlockClicks.Instance.SetBlockAllDelay(.3f,false);
 
	}
	
	public void BtnPlayClick( )
	{
		 
		if(SoundManager.Instance!=null) SoundManager.Instance.Play_ButtonClick();
		StopAllCoroutines();
		
		LevelTransition.Instance.HideSceneAndLoadNext("SelectMinigame");
 
		BlockClicks.Instance.SetBlockAll(true);
		 
	}

	//********************************************

 
}
