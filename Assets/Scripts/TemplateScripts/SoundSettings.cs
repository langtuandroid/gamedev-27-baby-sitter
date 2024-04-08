using UnityEngine;
using UnityEngine.UI;

namespace TemplateScripts
{
	public class SoundSettings : MonoBehaviour {
		
		private void Start () {
			InitialiseSoundSettings();
		}

		public void InitialiseSoundSettings()
		{
			SoundManager.MusicOn = PlayerPrefs.GetInt("MusicOn",1);
			SoundManager.SoundOn = PlayerPrefs.GetInt("SoundOn",1);

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
}
