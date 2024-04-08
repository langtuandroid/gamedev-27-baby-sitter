using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TemplateScripts
{
	public class DailyRewardsBS : MonoBehaviour {
		
		private static readonly int [] DailyRewardAmount = new int[]{  10, 20, 30, 40, 50,  60};
		private static int _levelRewardD;
		
		private bool rewardCompletedD = false;
		private int sixDayCount, typeOfSixReward; 
		
		public Text moneyText;
	 
		private  DateTime quitTimeE;
		private string lastPlayDateE,timeQuitStringG;
		
		public Image imgUnlockedDecoration;
		
		private bool bCollectedD = false;

		public bool TestDailyRewards()
		{
			bool bDailyReward = false ;
			
			DateTime currentTime = DateTime.Today;
		 
			if(PlayerPrefs.HasKey("LevelReward"))
			{
				_levelRewardD=PlayerPrefs.GetInt("LevelReward");
			}
			else
			{
				_levelRewardD=0;
				PlayerPrefs.SetInt("LevelReward",0);
			}
		
			if(PlayerPrefs.HasKey("VremeIzlaska"))
			{
				lastPlayDateE=PlayerPrefs.GetString("VremeIzlaska");
				DateTime dt = DateTime.Parse(lastPlayDateE) ;
				quitTimeE = new DateTime(dt.Year,dt.Month,dt.Day) ;

			}
			else
			{
				timeQuitStringG = DateTime.Now.ToString();
				PlayerPrefs.SetString("VremeIzlaska", timeQuitStringG);
				//PlayerPrefs.Save();
			}

			if(quitTimeE.AddDays(1) == currentTime)
			{
				_levelRewardD++;
				if(_levelRewardD ==7) _levelRewardD = 1;
				bDailyReward = true;
			}
			else if(quitTimeE.AddDays(1) < currentTime)
			{
				_levelRewardD = 1;
				bDailyReward = true;
			}
			else if(quitTimeE  != currentTime)
			{
				_levelRewardD = 0;
				PlayerPrefs.SetInt("LevelReward",0);
				timeQuitStringG = DateTime.Now.ToString();
				PlayerPrefs.SetString("VremeIzlaska", timeQuitStringG);
			}
			return bDailyReward;
		}




		private void OnApplicationPause(bool pauseStatus) { 
			if(pauseStatus)
			{
				timeQuitStringG = DateTime.Now.ToString();
				PlayerPrefs.SetString("VremeIzlaska", timeQuitStringG);
				PlayerPrefs.Save();
			
			}
		}
		
		public void ShowDailyRewardD ( )
		{
			int currentDayReward = _levelRewardD;
			gameObject.GetComponent<RectTransform>().anchoredPosition =  new Vector2(0,1970);// Vector2.zero;
			gameObject.GetComponent<Animator>().SetBool("IsOpen",true);
			MenuManagerBS.BPopUpVisible = true;
			MenuManagerBS.ActiveMenu = transform.name;
			EscapeButtonManagerBS.EscapeButtonFunctionStackK.Push("CloseDailyReward");



			for(int i = 1;i<=currentDayReward; i++)
			{
				GameObject.Find("Day" + i.ToString()).transform.GetComponent<Animator>().SetTrigger("EnableImage");

			}
 

			for(int i = 1;i<=6; i++)
			{
				GameObject.Find("Day" + i.ToString()+"/NumberText").transform.GetComponent<Text>().text = DailyRewardAmount[i-1].ToString() ;
			}
 
		  
		}
	
	 
		public IEnumerator MoneyCounterR(int kolicina)
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

			if(EscapeButtonManagerBS.EscapeButtonFunctionStackK.Count > 0 && EscapeButtonManagerBS.EscapeButtonFunctionStackK.Peek() == "CloseDailyReward") EscapeButtonManagerBS.EscapeButtonFunctionStackK.Pop ();
			MenuManagerBS.BPopUpVisible = false;
			MenuManagerBS.ActiveMenu = "";
		}

		private void SetActiveDay(int dayNumber)
		{
			GameObject.Find("Day"+dayNumber+"/Image").GetComponent<Image>().color = new Color(255,255,255,1);	 
		}

		private void OnApplicationQuit() {
			timeQuitStringG = DateTime.Now.ToString();
			PlayerPrefs.SetString("VremeIzlaska", timeQuitStringG);
			PlayerPrefs.Save();

		}

		public void TakeRewardD()
		{
			if(!rewardCompletedD)
			{
				{
					StartCoroutine(nameof(MoneyCounterR),DailyRewardAmount[_levelRewardD-1]);
				}

			 
				SoundManagerBS.Instance.PlaySound(SoundManagerBS.Instance.Coins);


				rewardCompletedD=true;

			}

		}
		
		public void CollectT()
		{
			if(bCollectedD) return;
			bCollectedD = true;
			SoundManagerBS.Instance.Play_ButtonClickK();

			timeQuitStringG = DateTime.Now.ToString();
			PlayerPrefs.SetString("VremeIzlaska", timeQuitStringG);
			PlayerPrefs.SetInt("LevelReward",_levelRewardD);
			PlayerPrefs.Save();

		 
			TakeRewardD();
		}


	 
	}
}
