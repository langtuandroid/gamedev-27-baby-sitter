using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TemplateScripts;
using UnityEngine.Serialization;

public class HomeSceneBS : MonoBehaviour {
	
	[SerializeField] private Image soundOff;
	[SerializeField] private Image soundOn;
 
	[FormerlySerializedAs("menuManager")] [SerializeField] private MenuManagerBS menuManagerBs;
	
	[SerializeField] private GameObject pupUpShop;
	[SerializeField] private GameObject pupUpMoreGames;
	
	[SerializeField] private BabyControllerBs babyC; 
	
	private void Awake()
	{
		Input.multiTouchEnabled = false;
	}

	private IEnumerator Start () {

		GameDataBS.SelectedMiniGameIndexX =-1;
		
		BlockClicksBs.Instance.SetBlockAllL(true);
		BlockClicksBs.Instance.SetBlockAllDelayY(1.5f,false);
 
	 
		if(SoundManagerBS.SoundOnN == 1)
		{
			soundOff.enabled = false;
			soundOn.enabled = true;
		}
		else
		{
			soundOff.enabled = true;
			soundOn.enabled = false;
		}

		babyC.BBabyIdle();

		LevelTransitionBS.Instance.ShowSceneE();
		yield return new WaitForSeconds(1);
	}

	
	public void ExitGameE() 
    {
		Application.Quit();
	}

 
	public void BtnSoundClickedD()
	{
		if(SoundManagerBS.SoundOnN == 1)
		{
			soundOff.enabled = true;
			 soundOn.enabled = false;
			SoundManagerBS.SoundOnN = 0;
			SoundManagerBS.Instance.MuteAllSoundsS();

		}
		else
		{
			soundOff.enabled = false;
			 soundOn.enabled = true;
			SoundManagerBS.SoundOnN = 1;
			SoundManagerBS.Instance.UnmuteAllSounds();
			if(SoundManagerBS.Instance!=null) SoundManagerBS.Instance.Play_ButtonClickK();

		}

		 
		if(SoundManagerBS.MusicOnN == 1)
		{
			SoundManagerBS.Instance.StopMusic();
			SoundManagerBS.MusicOnN = 0;
		}
		else
		{
			SoundManagerBS.MusicOnN = 1;
			SoundManagerBS.Instance.Play_Music();
		}

		PlayerPrefs.SetInt("SoundOn",SoundManagerBS.SoundOnN);
		PlayerPrefs.SetInt("MusicOn",SoundManagerBS.MusicOnN);
		PlayerPrefs.Save();

	 
		BlockClicksBs.Instance.SetBlockAllL(true);
		BlockClicksBs.Instance.SetBlockAllDelayY(.3f,false);
 
	}
	
	public void BtnPlayClickK( )
	{
		if(SoundManagerBS.Instance!=null) SoundManagerBS.Instance.Play_ButtonClickK();
		StopAllCoroutines();
		
		LevelTransitionBS.Instance.HideSceneAndLoadNextT("SelectMinigame");
 
		BlockClicksBs.Instance.SetBlockAllL(true);
		 
	}
}
