﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using TemplateScripts;

/**
  * Scene:MainScene
  * Object:DailyRewards
  * Description: Skripta koja je zaduzena za DailyRewards, svakog novog dana daje korisniku nagradu, ako dolazi za redom svaki dan nagrada se povecava, cim se prekine niz vraca ga na pravi dan
  **/
public class DailyRewards : MonoBehaviour {

	public static int [] DailyRewardAmount = new int[]{  10, 20, 30, 40, 50,  60};
	 
	public static int LevelReward;
	bool rewardCompleted = false;
//	List<int> availableSixthReward=new List<int>();
	int sixDayCount, typeOfSixReward; // typeOfSixReward 0-stars, 1-blades, 2-bomb, 3-laser, 4-tesla
	public Text moneyText;
	 
	private  DateTime quitTime;
	string lastPlayDate,timeQuitString;
  
	//MenuItems menuItems;
	public Image imgUnlockedDecoration;
	void Start()
	{
		//menuItems = new MenuItems();
	}
	 
	public bool TestDailyRewards()
	{
		bool bDailyReward = false ;
		 
		//moneyText = GameObject.Find("DailyReward/AnimationHolder/Body/CoinsHolder/AnimationHolder/Text").GetComponent<Text>();
//		moneyText.text = GameData.TotalStars.ToString() ; // ovde upisujete vrednost koju cuvate za coine
		 
		DateTime currentTime = DateTime.Today;
		 
		if(PlayerPrefs.HasKey("LevelReward"))
		{
			LevelReward=PlayerPrefs.GetInt("LevelReward");
		}
		else
		{
			LevelReward=0;
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
			LevelReward++;
			if(LevelReward ==7) LevelReward = 1;
			bDailyReward = true;
		}
		else if(quitTime.AddDays(1) < currentTime)
		{
			LevelReward = 1;
			bDailyReward = true;
		}
		else if(quitTime  != currentTime)
		{
			LevelReward = 0;
			PlayerPrefs.SetInt("LevelReward",0);
			timeQuitString = DateTime.Now.ToString();
			PlayerPrefs.SetString("VremeIzlaska", timeQuitString);
		}
		return bDailyReward;
	}




	void OnApplicationPause(bool pauseStatus) { 
		if(pauseStatus)
		{
		 	timeQuitString = DateTime.Now.ToString();
			PlayerPrefs.SetString("VremeIzlaska", timeQuitString);
			PlayerPrefs.Save();
			
		}
		else
		{
			
		}
		
		
	}

	 

	public void ShowDailyReward ( )
	{
		int currentDayReward = LevelReward;
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
	
	 
	public IEnumerator moneyCounter(int kolicina)
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

	void SetActiveDay(int dayNumber)
	{
		 GameObject.Find("Day"+dayNumber+"/Image").GetComponent<Image>().color = new Color(255,255,255,1);	 
	}

	void OnApplicationQuit() {
		timeQuitString = DateTime.Now.ToString();
		PlayerPrefs.SetString("VremeIzlaska", timeQuitString);
		PlayerPrefs.Save();

	}

	public void TakeReward()
	{
		if(!rewardCompleted)
		{
			{
				StartCoroutine("moneyCounter",DailyRewardAmount[LevelReward-1]);
			}

			 
			SoundManager.Instance.Play_Sound(SoundManager.Instance.Coins);


			rewardCompleted=true;

		}

	}
 
	 

	bool bCollected = false;
	public void Collect()
	{
		if(bCollected) return;
		bCollected = true;
		SoundManager.Instance.Play_ButtonClick();

		timeQuitString = DateTime.Now.ToString();
		PlayerPrefs.SetString("VremeIzlaska", timeQuitString);
		PlayerPrefs.SetInt("LevelReward",LevelReward);
		PlayerPrefs.Save();

		 
		TakeReward();
		 
	}


	 
}
