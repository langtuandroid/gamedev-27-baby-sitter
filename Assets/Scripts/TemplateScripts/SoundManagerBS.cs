using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TemplateScripts
{
	public class SoundManagerBS : MonoBehaviour {

		public static int MusicOnN = 1;
		public static int SoundOnN = 1;
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
		[field:SerializeField] public AudioSource MoquitoSmackK { private set; get; }

		[field:SerializeField] public AudioSource CreamTubeE { private set; get; }
		[field:SerializeField] public AudioSource[] BabyY { private set; get; }
	 
		[field:SerializeField] public AudioSource DropInWaterR { private set; get; }
		[field:SerializeField] public AudioSource BubbleE { private set; get; }
		[field:SerializeField] public AudioSource ShowerR { private set; get; }
		[field:SerializeField] public AudioSource BathtubPlugG { private set; get; }
		[field:SerializeField] public AudioSource TowelL { private set; get; }
		[field:SerializeField] public AudioSource NoseCleanN { private set; get; }
		[field:SerializeField] public AudioSource TeethH { private set; get; }
		[field:SerializeField] public AudioSource BlenderR { private set; get; }
		[field:SerializeField] public AudioSource BabyChewW { private set; get; }
		[field:SerializeField] public AudioSource  LiquidD { private set; get; }
		[field:SerializeField] public AudioSource CerealL { private set; get; }
		[field:SerializeField] public AudioSource  BabyCryY { private set; get; }
		[field:SerializeField] public AudioSource  BlanketT { private set; get; }
		[field:SerializeField] public AudioSource  LightSwitchH { private set; get; }
		[field:SerializeField] public AudioSource  CradleSwingG { private set; get; }
		[field:SerializeField] public AudioSource  ShowItemM { private set; get; }
		[field:SerializeField] public AudioSource  ChimesS { private set; get; }
		[field:SerializeField] public AudioSource  RattleToyY { private set; get; }
		[field:SerializeField] public AudioSource  MixingFoodD { private set; get; }

		[field:SerializeField] public float OriginalMusicVolumeE { private set; get; }

		public static SoundManagerBS instance;
		
		public List<AudioSource> listStopSoundOnExit = new  List<AudioSource>(); 


		public static SoundManagerBS Instance
		{
			get
			{
				if(instance == null)
				{
					instance = GameObject.FindObjectOfType(typeof(SoundManagerBS)) as SoundManagerBS;
				}

				return instance;
			}
		}

		private void Start () 
		{

			OriginalMusicVolumeE = GameplayMusic.volume;
			DontDestroyOnLoad(this.gameObject);

			if(PlayerPrefs.HasKey("SoundOn"))
			{
				SoundOnN = PlayerPrefs.GetInt("SoundOn",1);
				if(SoundManagerBS.SoundOnN == 0) MuteAllSoundsS();
				else UnmuteAllSounds();
			}
			else
			{
				SetSoundD(true);
			}

			MusicOnN = PlayerPrefs.GetInt("MusicOn",1);
			if(MusicOnN == 1)Play_Music();
			else StopMusic();

			Screen.sleepTimeout = SleepTimeout.NeverSleep; 

		}

		public void SetSoundD(bool bEnabled)
		{
			if(bEnabled)
			{
				PlayerPrefs.SetInt("SoundOn", 1);
				UnmuteAllSounds();
			}
			else
			{
				PlayerPrefs.SetInt("SoundOn", 0);
				MuteAllSoundsS();
			}

			SoundOnN = PlayerPrefs.GetInt("SoundOn");
		}

		public void Play_ButtonClickK()
		{
			if(ButtonClick.clip != null && SoundOnN == 1)
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
			if(GameplayMusic.clip != null && MusicOnN == 1 && !GameplayMusic.isPlaying)
			{
				GameplayMusic.volume = OriginalMusicVolumeE;
				GameplayMusic.Play();
			}
		}

		public void StopMusic()
		{
			if(GameplayMusic.clip != null && MusicOnN == 1)
			{
				StartCoroutine(FadeOut(GameplayMusic, 0.1f));
			}
		}

//	public void Play_TaskCompleted()
//	{
//		if(ElementCompleted.clip != null&& soundOn == 1)
//			ElementCompleted.Play();
//	}

		public void PlayPopUpShow(float time = 0)
		{
			if(PopUpShow.clip != null && SoundOnN == 1)
				StartCoroutine(PlayClip(PopUpShow,time));
			 
		}

		public void PlayPopUpHide(float time = 0)
		{
			if(PopUpHide.clip != null && SoundOnN == 1)
				StartCoroutine(PlayClip(PopUpHide,time));
		
		}

		private IEnumerator PlayClip(AudioSource Clip, float time)
		{
			yield return new WaitForSeconds(time);
			Clip.Play();
		}
		
		private IEnumerator FadeOut(AudioSource sound, float time)
		{
			float originalVolume = sound.volume;

			if(sound.name == GameplayMusic.name) originalVolume = OriginalMusicVolumeE;


			while(sound.volume > 0.05f)
			{
				sound.volume = Mathf.MoveTowards(sound.volume, 0, time);
				yield return null;
			}
			sound.Stop();
			sound.volume = originalVolume;
		}
		
		public void MuteAllSoundsS()
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

		public void	PlaySound(AudioSource sound)
		{
			if(!sound.isPlaying  && SoundOnN == 1) 
				sound.Play();
		}

		public void	StopAndPlay_Sound(AudioSource sound)
		{
			if(sound.isPlaying)
				sound.Stop();

			if( SoundOnN == 1) 
				sound.Play();
		}
	
		public void	StopSound(AudioSource sound)
		{
		
			if(sound.isPlaying)
				sound.Stop();
		}


		public void ChangeSoundVolume(AudioSource sound, float time, float value)
		{
			if(value>1) value = 1;
			if(value<0) value = 0;
			if( (MusicOnN == 1 && sound.name == GameplayMusic.name ) || (SoundOnN == 1 &&  sound.name != GameplayMusic.name )) 
			{
				StartCoroutine( ChangeVolume( sound, time, value));
			}
		 
		}

		private IEnumerator ChangeVolume(AudioSource sound, float time,float value)
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
			if(  SoundOnN == 1) StartCoroutine(StopAndPlaySoundDly(sound,time));
		}

		private IEnumerator StopAndPlaySoundDly(AudioSource sound, float time)
		{
			yield return new WaitForSeconds(time);
			if(sound.isPlaying)
				sound.Stop();
		
			if( SoundOnN == 1) 
				sound.Play();
		}

		public void	PlaySound(AudioSource sound, float time)
		{
			if(  SoundOnN == 1) StartCoroutine(PlaySoundDly(sound,time));
		}

		private IEnumerator PlaySoundDly(AudioSource sound, float time)
		{
			yield return new WaitForSeconds(time);
			if(!sound.isPlaying  && SoundOnN == 1) 
				sound.Play();
		}

	}
}
