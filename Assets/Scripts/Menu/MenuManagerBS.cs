using UnityEngine;
using System.Collections;
using TemplateScripts;
using UnityEngine.Serialization;
using UnityEngine.UI;
 
	/**
  * Scene:All
  * Object:Canvas
  * Description: Skripta zaduzena za hendlovanje(prikaz i sklanjanje svih Menu-ja,njihovo paljenje i gasenje, itd...)
  **/
public class MenuManagerBS : MonoBehaviour 
{
	[FormerlySerializedAs("currentMenu")] public MenuBS currentMenuBs;
	private MenuBS currentPopUpMenuBs;
	public static string ActiveMenu = "";
	
	public GameObject[] disabledObjects;
    private GameObject ratePopUpP;

    private static bool _bFirstLoadMainSceneE = true;
	 
	private DailyRewardsBS dailyRewardBs = null;
	
	public static bool BPopUpVisible = false;
	
	private CanvasGroup blockAllL;
	private GameObject popUpSpecialOfferR;
	private void Start () 
	{
		ActiveMenu = "";
		if(Application.loadedLevelName=="HomeScene")
		{
			ratePopUpP = GameObject.Find("PopUps/PopUpRate");

			 
			if(GameObject.Find("PopUps/DailyReward") != null ) dailyRewardBs = GameObject.Find("PopUps/DailyReward").GetComponent <DailyRewardsBS>();
		 
		}

		if (disabledObjects!=null) {
			for(int i=0;i<disabledObjects.Length;i++)
				disabledObjects[i].SetActive(false);
		}
	 
		if(Application.loadedLevelName=="HomeScene" && _bFirstLoadMainSceneE)
		{
			StartCoroutine(DelayStartMainScene());
		}

	}
	
	private IEnumerator DelayStartMainScene()
	{
		 
		if(blockAllL == null) blockAllL = GameObject.Find("BlockAll").GetComponent<CanvasGroup>();
		blockAllL.blocksRaycasts = true;
		
		yield return new WaitForSeconds(0.5f);
		if(PlayerPrefs.HasKey("alreadyRated"))
		{
			RateBS.AlreadyRated = PlayerPrefs.GetInt("alreadyRated");
		}
		else
		{
			RateBS.AlreadyRated = 0;
		}

		RateBS.AppStartedNumber = PlayerPrefs.GetInt("appStartedNumber",0);

		if(_bFirstLoadMainSceneE)
		{
			if(RateBS.AlreadyRated==0 && RateBS.AppStartedNumber>=6 )
			{
				RateBS.AppStartedNumber=0;
				PlayerPrefs.SetInt("appStartedNumber",RateBS.AppStartedNumber);
				PlayerPrefs.Save();

				ShowPopUpMenu(ratePopUpP);
			}
			else
			{
					if( _bFirstLoadMainSceneE && dailyRewardBs !=null &&  dailyRewardBs.TestDailyRewards())
					{	
						dailyRewardBs.ShowDailyRewardD();
					}
					else
					{	
						if(dailyRewardBs !=null ) dailyRewardBs.gameObject.SetActive(false);
					}
			}
		}

		_bFirstLoadMainSceneE = false;
		
		yield return new WaitForSeconds(1.1f);
		blockAllL.blocksRaycasts = false;
	}

	/// <summary>
	/// Funkcija koja pali(aktivira) objekat
	/// </summary>
	/// /// <param name="gameObject">Game object koji se prosledjuje i koji treba da se upali</param>
	public void EnableObject(GameObject gameObject)
	{
		
		if (gameObject != null) 
		{
			if (!gameObject.activeSelf) 
			{
				gameObject.SetActive (true);
			}
		}
	}

	/// <summary>
	/// Funkcija koja gasi objekat
	/// </summary>
	/// /// <param name="gameObject">Game object koji se prosledjuje i koji treba da se ugasi</param>
	public void DisableObject(GameObject gameObject)
	{
		
		if (gameObject != null) 
		{
			if (gameObject.activeSelf) 
			{
				gameObject.SetActive (false);
			}
		}
	}
	
	/// <summary>
	/// F-ja koji poziva ucitavanje Scene
	/// </summary>
	/// <param name="levelName">Level name.</param>
	public void LoadScene(string levelName )
	{
		if (levelName != "") {
			try {
				Application.LoadLevel(levelName);
			} catch (System.Exception e) {
				Debug.Log ("Can't load scene: " + e.Message);
			}
		} else {
			Debug.Log ("Can't load scene: Level name to set");
		}
	}
	
	/// <summary>
	/// F-ja koji poziva asihrono ucitavanje Scene
	/// </summary>
	/// <param name="levelName">Level name.</param>
	public void LoadSceneAsync(string levelName )
	{
		if (levelName != "") {
			try {
				Application.LoadLevelAsync (levelName);
			} catch (System.Exception e) {
				Debug.Log ("Can't load scene: " + e.Message);
			}
		} else {
			Debug.Log ("Can't load scene: Level name to set");
		}
	}

	 
 

	/// <summary>
	/// Funkcija za prikaz Menu-ja koji je pozvan kao PopUp-a
	/// </summary>
	/// /// <param name="menu">Game object koji se prosledjuje za prikaz, mora imati na sebi skriptu Menu.</param>
	public void ShowPopUpMenu(GameObject menu)
	{		 
		menu.gameObject.SetActive (true);
		currentPopUpMenuBs = menu.GetComponent<MenuBS> ();
		currentPopUpMenuBs.OpenMenuU();
		ActiveMenu = menu.name; 
		BPopUpVisible = true;
		if(SoundManagerBS.Instance!=null) 
		{
			//if(Time.timeScale>0 ) SoundManager.Instance.Play_PopUpShow(.05f);
			//else 
				SoundManagerBS.Instance.PlaySound(SoundManagerBS.Instance.PopUpShow);
		}
		if(Application.loadedLevelName != "Room") EscapeButtonManagerBS.AddEscapeButtonFunction("ClosePopUpMenuEsc",menu.name);
		 
		 
	}

	/// <summary>
	/// Funkcija za zatvaranje Menu-ja koji je pozvan kao PopUp-a, poziva inace coroutine-u, ima delay zbog animacije odlaska Menu-ja
	/// </summary>
	/// /// <param name="menu">Game object koji se prosledjuje i treba da se skloni, mora imati na sebi skriptu Menu.</param>
	public void ClosePopUpMenu(GameObject menu)
	{
		//StartCoroutine("HidePopUp",menu);
		menu.GetComponent<MenuBS> ().CloseMenuU();
		if(ActiveMenu == menu.name) ActiveMenu = "";
		if(SoundManagerBS.Instance !=null ) SoundManagerBS.Instance.PlayPopUpHide(.4f);
		BPopUpVisible = false;
		if(EscapeButtonManagerBS.EscapeButtonFunctionStackK.Count>=1) EscapeButtonManagerBS.EscapeButtonFunctionStackK.Pop();


	}

	public void ClosePopUpMenuEsc(string menuName)
	{
		if(GameObject.Find(menuName)!=null)
			ClosePopUpMenu(GameObject.Find(menuName));
	}


	/// <summary>
	/// Couorutine-a za zatvaranje Menu-ja koji je pozvan kao PopUp-a
	/// </summary>
	/// /// <param name="menu">Game object koji se prosledjuje, mora imati na sebi skriptu Menu.</param>
	IEnumerator HidePopUp(GameObject menu)
	{
		yield return null;
		//menu.GetComponent<Menu> ().IsOpen = false;
		menu.GetComponent<MenuBS> ().CloseMenuU();
		//yield return new WaitForSeconds(1.2f);
		//menu.SetActive (false);
	}

	/// <summary>
	/// Funkcija za prikaz poruke preko Log-a, prilikom klika na dugme
	/// </summary>
	/// /// <param name="message">poruka koju treba prikazati.</param>
	public void ShowMessage(string message)
	{
		Debug.Log(message);
	}
        

	/// <summary>
	/// Funkcija koja podesava naslov dialoga kao i poruku u dialogu i ova f-ja se poziva iz skripte
	/// </summary>
	/// <param name="messageTitleText">naslov koji treba prikazati.</param>
	/// <param name="messageText">custom poruka koju treba prikazati.</param>
	public void ShowPopUpMessage(string messageTitleText, string messageText)
	{
		transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/HeaderHolder/TextHeader").GetComponent<Text>().text=messageTitleText;
		transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/ContentHolder/TextBG/TextMessage").GetComponent<Text>().text=messageText;
		ShowPopUpMenu(transform.Find("PopUps/PopUpMessage").gameObject);

	}

	/// <summary>
	/// Funkcija koja podesava naslov CustomMessage-a, i ova f-ja se poziva preko button-a zajedno za f-jom ShowPopUpMessageCustomMessageText u redosledu: 1-ShowPopUpMessageTitleText 2-ShowPopUpMessageCustomMessageText
	/// </summary>
	/// <param name="messageTitleText">naslov koji treba prikazati.</param>
	public void ShowPopUpMessageTitleText(string messageTitleText)
	{
		transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/HeaderHolder/TextHeader").GetComponent<Text>().text=messageTitleText;
	}

	/// <summary>
	/// Funkcija koja podesava poruku CustomMessage, i poziva meni u vidu pop-upa, ova f-ja se poziva preko button-a zajedno za f-jom ShowPopUpMessageTitleText u redosledu: 1-ShowPopUpMessageTitleText 2-ShowPopUpMessageCustomMessageText
	/// </summary>
	/// <param name="messageText">custom poruka koju treba prikazati.</param>
	public void ShowPopUpMessageCustomMessageText(string messageText)
	{
		transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/ContentHolder/TextBG/TextMessage").GetComponent<Text>().text=messageText;		
		ShowPopUpMenu(transform.Find("PopUps/PopUpMessage").gameObject);
	}

	/// <summary>
	/// Funkcija koja podesava naslov dialoga kao i poruku u dialogu i ova f-ja se poziva iz skripte
	/// </summary>
	/// <param name="dialogTitleText">naslov koji treba prikazati.</param>
	/// <param name="dialogMessageText">custom poruka koju treba prikazati.</param>
	public void ShowPopUpDialog(string dialogTitleText, string dialogMessageText)
	{
		transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/HeaderHolder/TextHeader").GetComponent<Text>().text=dialogTitleText;
		transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/ContentHolder/TextBG/TextMessage").GetComponent<Text>().text=dialogMessageText;
		ShowPopUpMenu(transform.Find("PopUps/PopUpMessage").gameObject);
	}

	/// <summary>
	/// Funkcija koja podesava naslov dialoga, ova f-ja se poziva preko button-a zajedno za f-jom ShowPopUpDialogCustomMessageText u redosledu: 1-ShowPopUpDialogTitleText 2-ShowPopUpDialogCustomMessageText
	/// </summary>
	/// <param name="dialogTitleText">naslov koji treba prikazati.</param>
	public void ShowPopUpDialogTitleText(string dialogTitleText)
	{
		transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/HeaderHolder/TextHeader").GetComponent<Text>().text=dialogTitleText;
	}

	/// <summary>
	/// Funkcija koja podesava poruku dialoga i poziva meni u vidu pop-upa, ova f-ja se poziva preko button-a zajedno za f-jom ShowPopUpDialogTitleText u redosledu: 1-ShowPopUpDialogTitleText 2-ShowPopUpDialogCustomMessageText
	/// </summary>
	/// <param name="dialogMessageText">custom poruka koju treba prikazati.</param>
	public void ShowPopUpDialogCustomMessageText(string dialogMessageText)
	{
		transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/ContentHolder/TextBG/TextMessage").GetComponent<Text>().text=dialogMessageText;		
		ShowPopUpMenu(transform.Find("PopUps/PopUpMessage").gameObject);
	}
	
	public void btnClicked_PlaySound()
	{
		if(SoundManagerBS.Instance !=null)SoundManagerBS.Instance.Play_ButtonClickK();
	}


    public void OpenPrivacyPolicyLink()
    {
        Application.OpenURL(AdsManager.AdsManagerBS.Instance.privacyPolicyLink);
    }


}
