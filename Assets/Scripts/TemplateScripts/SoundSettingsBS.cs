using UnityEngine;
using UnityEngine.UI;

namespace TemplateScripts
{
	public class SoundSettingsBS : MonoBehaviour {
		
		private void Start () {
			InitialiseSettings();
		}

		public void InitialiseSettings()
		{
			SoundManagerBS.MusicOnN = PlayerPrefs.GetInt("MusicOn",1);
			SoundManagerBS.SoundOnN = PlayerPrefs.GetInt("SoundOn",1);

			if(SoundManagerBS.SoundOnN == 0)
				GameObject.Find("SoundOnOff").GetComponent<Image>().enabled = true;
			else
				GameObject.Find("SoundOnOff").GetComponent<Image>().enabled = false;


			if(SoundManagerBS.MusicOnN == 0)
				GameObject.Find("MusicOnOff").GetComponent<Image>().enabled = true;
			else
				GameObject.Find("MusicOnOff").GetComponent<Image>().enabled = false;
		}

		public void SoundOnOffP()
		{
			if(SoundManagerBS.SoundOnN == 1)
			{
				SoundManagerBS.SoundOnN = 0;
				SoundManagerBS.Instance.MuteAllSoundsS();
				GameObject.Find("SoundOnOff").GetComponent<Image>().enabled = true;
			}
			else
			{
				SoundManagerBS.SoundOnN = 1;
				SoundManagerBS.Instance.UnmuteAllSounds();
				SoundManagerBS.Instance.Play_ButtonClickK();
				GameObject.Find("SoundOnOff").GetComponent<Image>().enabled = false;
			}

			PlayerPrefs.SetInt("SoundOn",SoundManagerBS.SoundOnN);
			PlayerPrefs.SetInt("MusicOn",SoundManagerBS.MusicOnN);
			PlayerPrefs.Save();
		}

		public void MusicOnOffP()
		{
			SoundManagerBS.Instance.Play_ButtonClickK();
			if(SoundManagerBS.MusicOnN == 1)
			{
				SoundManagerBS.Instance.StopMusic();
				SoundManagerBS.MusicOnN = 0;
				GameObject.Find("MusicOnOff").GetComponent<Image>().enabled = true;
			}
			else
			{
				SoundManagerBS.MusicOnN = 1;
				SoundManagerBS.Instance.Play_Music();
				GameObject.Find("MusicOnOff").GetComponent<Image>().enabled = false;
			}
			PlayerPrefs.SetInt("SoundOn",SoundManagerBS.SoundOnN);
			PlayerPrefs.SetInt("MusicOn",SoundManagerBS.MusicOnN);
			PlayerPrefs.Save();
		}
	
	}
}
