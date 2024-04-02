using UnityEngine;
using System.Collections;
using TemplateScripts;
using UnityEngine.UI;

/**
  * Scene:MainScene, GamePlayScene
  * Object:N/A
  * Description: Skripta koja zavisno od stanja PlayerPrefs-a podesava image-a u PopUpSettings meniju i isto tako sadrzi f-je koje registuju klikove i promenu image-a za zvuk i sound
  **/
public class SoundSettings : MonoBehaviour {

	// Use this for initialization
	void Start () {
			InitialiseSoundSettings();
	}

	public void InitialiseSoundSettings()
	{
		//if(PlayerPrefs.HasKey("SoundOn"))
	//	{
			SoundManager.MusicOn = PlayerPrefs.GetInt("MusicOn",1);
			SoundManager.SoundOn = PlayerPrefs.GetInt("SoundOn",1);
	//	}

		if(SoundManager.SoundOn == 0)
			GameObject.Find("SoundOnOff").GetComponent<Image>().enabled = true;
		else
			GameObject.Find("SoundOnOff").GetComponent<Image>().enabled = false;


		if(SoundManager.MusicOn == 0)
			GameObject.Find("MusicOnOff").GetComponent<Image>().enabled = true;
		else
			GameObject.Find("MusicOnOff").GetComponent<Image>().enabled = false;
	}

	public void SoundOnOff()
	{
		if(SoundManager.SoundOn == 1)
		{
			SoundManager.SoundOn = 0;
			SoundManager.Instance.MuteAllSounds();
			GameObject.Find("SoundOnOff").GetComponent<Image>().enabled = true;
		}
		else
		{
			SoundManager.SoundOn = 1;
			SoundManager.Instance.UnmuteAllSounds();
			SoundManager.Instance.Play_ButtonClick();
			GameObject.Find("SoundOnOff").GetComponent<Image>().enabled = false;
		}

		PlayerPrefs.SetInt("SoundOn",SoundManager.SoundOn);
		PlayerPrefs.SetInt("MusicOn",SoundManager.MusicOn);
		PlayerPrefs.Save();
	}

	public void MusicOnOff()
	{
		SoundManager.Instance.Play_ButtonClick();
		if(SoundManager.MusicOn == 1)
		{
 			SoundManager.Instance.Stop_Music();
			SoundManager.MusicOn = 0;
			GameObject.Find("MusicOnOff").GetComponent<Image>().enabled = true;
		}
		else
		{
			SoundManager.MusicOn = 1;
 			SoundManager.Instance.Play_Music();
			GameObject.Find("MusicOnOff").GetComponent<Image>().enabled = false;
		}
		PlayerPrefs.SetInt("SoundOn",SoundManager.SoundOn);
		PlayerPrefs.SetInt("MusicOn",SoundManager.MusicOn);
		PlayerPrefs.Save();
	}
	
}
