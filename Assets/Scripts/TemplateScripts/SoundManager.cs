using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TemplateScripts
{
	public class SoundManager : MonoBehaviour {

		public static int MusicOn = 1;
		public static int SoundOn = 1;
		public static bool ForceTurnOff = false;
		
		[field:SerializeField] public AudioSource GameplayMusic { private set; get; }
		[field:SerializeField] public AudioSource ButtonClick { private set; get; }
		
		[field:SerializeField] public AudioSource ButtonClick2 { private set; get; }
		[field:SerializeField] public AudioSource PopUpShow { private set; get; }
		[field:SerializeField] public AudioSource PopUpHide { private set; get; }
  
		[field:SerializeField] public AudioSource ActionCompleted { private set; get; }
		[field:SerializeField] public AudioSource MinigameCompleted { private set; get; }

		[field:SerializeField] public AudioSource Coins { private set; get; }
			/*
		public AudioSource RecordSound1;
		public AudioSource RecordSound2;
		public AudioSource RecordSound3;
		public AudioSource RecordSound4;
		public AudioSource RecordSound5;

		public AudioSource Instrument1;
		public AudioSource Instrument2;
		public AudioSource Instrument3;
	*/
		[field:SerializeField] public AudioSource MoquitoSmack { private set; get; }

		[field:SerializeField] public AudioSource CreamTube { private set; get; }
		[field:SerializeField] public AudioSource[] Baby { private set; get; }
	 
		[field:SerializeField] public AudioSource DropInWater { private set; get; }
		[field:SerializeField] public AudioSource Bubble { private set; get; }
		[field:SerializeField] public AudioSource Shower { private set; get; }
		[field:SerializeField] public AudioSource BathtubPlug { private set; get; }
		[field:SerializeField] public AudioSource Towel { private set; get; }
		[field:SerializeField] public AudioSource NoseClean { private set; get; }
		[field:SerializeField] public AudioSource Teeth { private set; get; }
		[field:SerializeField] public AudioSource Blender { private set; get; }
		[field:SerializeField] public AudioSource BabyChew { private set; get; }
		[field:SerializeField] public AudioSource  Liquid { private set; get; }
		[field:SerializeField] public AudioSource Cereal { private set; get; }
		[field:SerializeField] public AudioSource  BabyCry { private set; get; }
		[field:SerializeField] public AudioSource  Blanket { private set; get; }
		[field:SerializeField] public AudioSource  LightSwitch { private set; get; }
		[field:SerializeField] public AudioSource  CradleSwing { private set; get; }
		[field:SerializeField] public AudioSource  ShowItem { private set; get; }
		[field:SerializeField] public AudioSource  Chimes { private set; get; }
		[field:SerializeField] public AudioSource  RattleToy { private set; get; }
		[field:SerializeField] public AudioSource  MixingFood { private set; get; }

		[field:SerializeField] public float OriginalMusicVolume { private set; get; }

		public static SoundManager instance;
		
		public List<AudioSource> listStopSoundOnExit = new  List<AudioSource>(); 


		public static SoundManager Instance
		{
			get
			{
				if(instance == null)
				{
					instance = GameObject.FindObjectOfType(typeof(SoundManager)) as SoundManager;
				}

				return instance;
			}
		}

		private void Start () 
		{

			OriginalMusicVolume = GameplayMusic.volume;
			DontDestroyOnLoad(this.gameObject);

			if(PlayerPrefs.HasKey("SoundOn"))
			{
				SoundOn = PlayerPrefs.GetInt("SoundOn",1);
				if(SoundManager.SoundOn == 0) MuteAllSounds();
				else UnmuteAllSounds();
			}
			else
			{
				SetSound(true);
			}

			MusicOn = PlayerPrefs.GetInt("MusicOn",1);
			if(MusicOn == 1)Play_Music();
			else Stop_Music();

			Screen.sleepTimeout = SleepTimeout.NeverSleep; 

		}

		public void SetSound(bool bEnabled)
		{
			if(bEnabled)
			{
				PlayerPrefs.SetInt("SoundOn", 1);
				UnmuteAllSounds();
			}
			else
			{
				PlayerPrefs.SetInt("SoundOn", 0);
				MuteAllSounds();
			}

			SoundOn = PlayerPrefs.GetInt("SoundOn");
		}

		public void Play_ButtonClick()
		{
			if(ButtonClick.clip != null && SoundOn == 1)
				ButtonClick.Play();
		}

//	public void Play_MenuMusic()
//	{
//		if(menuMusic.clip != null && musicOn == 1)
//			menuMusic.Play();
//	}
//
//	public void Stop_MenuMusic()
//	{
//		if(menuMusic.clip != null && musicOn == 1)
//			menuMusic.Stop();
//	}

		public void Play_Music()
		{
			if(GameplayMusic.clip != null && MusicOn == 1 && !GameplayMusic.isPlaying)
			{
				GameplayMusic.volume = OriginalMusicVolume;
				GameplayMusic.Play();
			}
		}

		public void Stop_Music()
		{
			if(GameplayMusic.clip != null && MusicOn == 1)
			{
				StartCoroutine(FadeOut(GameplayMusic, 0.1f));
			}
		}

//	public void Play_TaskCompleted()
//	{
//		if(ElementCompleted.clip != null&& soundOn == 1)
//			ElementCompleted.Play();
//	}

		public void Play_PopUpShow(float time = 0)
		{
			if(PopUpShow.clip != null && SoundOn == 1)
				StartCoroutine(PlayClip(PopUpShow,time));
			 
		}

		public void Play_PopUpHide(float time = 0)
		{
			if(PopUpHide.clip != null && SoundOn == 1)
				StartCoroutine(PlayClip(PopUpHide,time));
		
		}

		IEnumerator PlayClip(AudioSource Clip, float time)
		{
			yield return new WaitForSeconds(time);
			Clip.Play();
		}

		/// <summary>
		/// Corutine-a koja za odredjeni AudioSource, kroz prosledjeno vreme, utisava AudioSource do 0, gasi taj AudioSource, a zatim vraca pocetni Volume na pocetan kako bi AudioSource mogao opet da se koristi
		/// </summary>
		/// <param name="sound">AudioSource koji treba smanjiti/param>
		/// <param name="time">Vreme za koje treba smanjiti Volume/param>
		IEnumerator FadeOut(AudioSource sound, float time)
		{
			float originalVolume = sound.volume;

			if(sound.name == GameplayMusic.name) originalVolume = OriginalMusicVolume;


			while(sound.volume > 0.05f)
			{
				sound.volume = Mathf.MoveTowards(sound.volume, 0, time);
				yield return null;
			}
			sound.Stop();
			sound.volume = originalVolume;
		}

		/// <summary>
		/// F-ja koja Mute-uje sve zvuke koja su deca SoundManager-a
		/// </summary>
		public void MuteAllSounds()
		{
			foreach (Transform t in transform)
			{
				t.GetComponent<AudioSource>().mute = true;
			}
		}

		/// <summary>
		/// F-ja koja Unmute-uje sve zvuke koja su deca SoundManager-a
		/// </summary>
		public void UnmuteAllSounds()
		{
			foreach (Transform t in transform)
			{
				t.GetComponent<AudioSource>().mute = false;
			}
		}

		public void	Play_Sound(AudioSource sound)
		{
			if(!sound.isPlaying  && SoundOn == 1) 
				sound.Play();
		}

		public void	StopAndPlay_Sound(AudioSource sound)
		{
			if(sound.isPlaying)
				sound.Stop();

			if( SoundOn == 1) 
				sound.Play();
		}
	
		public void	Stop_Sound(AudioSource sound)
		{
		
			if(sound.isPlaying)
				sound.Stop();
		}


		public void ChangeSoundVolume(AudioSource sound, float time, float value)
		{
			if(value>1) value = 1;
			if(value<0) value = 0;
			if( (MusicOn == 1 && sound.name == GameplayMusic.name ) || (SoundOn == 1 &&  sound.name != GameplayMusic.name )) 
			{
				StartCoroutine( _ChangeVolume( sound, time, value));
			}
		 
		}

		private IEnumerator _ChangeVolume(AudioSource sound, float time,float value)
		{
			float _time = 0;
			yield return new WaitForFixedUpdate();
			while( _time <1)
			{
				_time+= Time.fixedDeltaTime/time;
				sound.volume = Mathf.Lerp(sound.volume, value, _time );
				yield return new WaitForFixedUpdate();
			}
		 
		}
	 
		public void StopActiveSoundsOnExitAndClearList()
		{
			foreach (AudioSource a in listStopSoundOnExit)
			{
				a.Stop();
			}
			listStopSoundOnExit.Clear();
		}
		
		public void	StopAndPlay_Sound(AudioSource sound, float time)
		{
			if(  SoundOn == 1) StartCoroutine(StopAndPlay_SoundDly(sound,time));
		}

		private IEnumerator StopAndPlay_SoundDly(AudioSource sound, float time)
		{
			yield return new WaitForSeconds(time);
			if(sound.isPlaying)
				sound.Stop();
		
			if( SoundOn == 1) 
				sound.Play();
		}

		public void	Play_Sound(AudioSource sound, float time)
		{
			if(  SoundOn == 1) StartCoroutine(Play_SoundDly(sound,time));
		}

		private IEnumerator Play_SoundDly(AudioSource sound, float time)
		{
			yield return new WaitForSeconds(time);
			if(!sound.isPlaying  && SoundOn == 1) 
				sound.Play();
		}

	}
}
