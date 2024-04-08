using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TemplateScripts
{
	public class DailyRewards : MonoBehaviour {
		
		private static readonly int [] DailyRewardAmount = new int[]{  10, 20, 30, 40, 50,  60};
		private static int _levelReward;
		
		private bool rewardCompleted = false;
		private int sixDayCount, typeOfSixReward; 
		
		public Text moneyText;
	 
		private  DateTime quitTime;
		string lastPlayDate,timeQuitString;
		
		public Image imgUnlockedDecoration;
		
		private bool bCollected = false;

		public bool TestDailyRewards()
		{
			bool bDailyReward = false ;
			
			DateTime currentTime = DateTime.Today;
		 
			if(PlayerPrefs.HasKey("LevelReward"))
			{
				_levelReward=PlayerPrefs.GetInt("LevelReward");
			}
			else
			{
				_levelReward=0;
				PlayerPrefs.SetInt("LevelReward",0);
			}
		
			if(PlayerPrefs.HasKey("VremeIzlaska"))
			{
				lastPlayDate=PlayerPrefs.GetString("VremeIzlaska");
				DateTime dt = DateTime.Parse(lastPlayDate) ;
				quitTime = new DateTime(dt.Year,dt.Month,dt.Day) ;

			}
			else
			{
				timeQuitString = DateTime.Now.ToString();
				PlayerPrefs.SetString("VremeIzlaska", timeQuitString);
				//PlayerPrefs.Save();
			}

			if(quitTime.AddDays(1) == currentTime)
			{
				_levelReward++;
				if(_levelReward ==7) _levelReward = 1;
				bDailyReward = true;
			}
			else if(quitTime.AddDays(1) < currentTime)
			{
				_levelReward = 1;
				bDailyReward = true;
			}
			else if(quitTime  != currentTime)
			{
				_levelReward = 0;
				PlayerPrefs.SetInt("LevelReward",0);
				timeQuitString = DateTime.Now.ToString();
				PlayerPrefs.SetString("VremeIzlaska", timeQuitString);
			}
			return bDailyReward;
		}




		private void OnApplicationPause(bool pauseStatus) { 
			if(pauseStatus)
			{
				timeQuitString = DateTime.Now.ToString();
				PlayerPrefs.SetString("VremeIzlaska", timeQuitString);
				PlayerPrefs.Save();
			
			}
		}
		
		public void ShowDailyReward ( )
		{
			int currentDayReward = _levelReward;
			gameObject.GetComponent<RectTransform>().anchoredPosition =  new Vector2(0,1970);// Vector2.zero;
			gameObject.GetComponent<Animator>().SetBool("IsOpen",true);
			MenuManager.BPopUpVisible = true;
			MenuManager.ActiveMenu = transform.name;
			EscapeButtonManager.EscapeButtonFunctionStack.Push("CloseDailyReward");



			for(int i = 1;i<=currentDayReward; i++)
			{
				GameObject.Find("Day" + i.ToString()).transform.GetComponent<Animator>().SetTrigger("EnableImage");

			}
 

			for(int i = 1;i<=6; i++)
			{
				GameObject.Find("Day" + i.ToString()+"/NumberText").transform.GetComponent<Text>().text = DailyRewardAmount[i-1].ToString() ;
			}
 
		  
		}
	
	 
		public IEnumerator MoneyCounter(int kolicina)
		{
			int current = int.Parse(moneyText.text);
			int suma = current + kolicina;
			int korak = (suma - current)/10;
			while(current != suma)
			{
				current += korak;
				moneyText.text = current.ToString();
				yield return new WaitForSeconds(0.07f);
			}

			yield return new WaitForSeconds(0.2f);

			gameObject.GetComponent<Animator>().SetBool("IsOpen",false);

			if(EscapeButtonManager.EscapeButtonFunctionStack.Count > 0 && EscapeButtonManager.EscapeButtonFunctionStack.Peek() == "CloseDailyReward") EscapeButtonManager.EscapeButtonFunctionStack.Pop ();
			MenuManager.BPopUpVisible = false;
			MenuManager.ActiveMenu = "";
		}

		private void SetActiveDay(int dayNumber)
		{
			GameObject.Find("Day"+dayNumber+"/Image").GetComponent<Image>().color = new Color(255,255,255,1);	 
		}

		private void OnApplicationQuit() {
			timeQuitString = DateTime.Now.ToString();
			PlayerPrefs.SetString("VremeIzlaska", timeQuitString);
			PlayerPrefs.Save();

		}

		public void TakeReward()
		{
			if(!rewardCompleted)
			{
				{
					StartCoroutine(nameof(MoneyCounter),DailyRewardAmount[_levelReward-1]);
				}

			 
				SoundManager.Instance.Play_Sound(SoundManager.Instance.Coins);


				rewardCompleted=true;

			}

		}
		
		public void Collect()
		{
			if(bCollected) return;
			bCollected = true;
			SoundManager.Instance.Play_ButtonClick();

			timeQuitString = DateTime.Now.ToString();
			PlayerPrefs.SetString("VremeIzlaska", timeQuitString);
			PlayerPrefs.SetInt("LevelReward",_levelReward);
			PlayerPrefs.Save();

		 
			TakeReward();
		}


	 
	}
}
