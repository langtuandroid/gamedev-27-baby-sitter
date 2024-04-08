using UnityEngine;
using System.Collections;
using TemplateScripts;
using UnityEngine.UI;
 
	/**
  * Scene:All
  * Object:Canvas
  * Description: Skripta zaduzena za hendlovanje(prikaz i sklanjanje svih Menu-ja,njihovo paljenje i gasenje, itd...)
  **/
public class MenuManager : MonoBehaviour 
{
	public Menu currentMenu;
	private Menu currentPopUpMenu;
	public static string ActiveMenu = "";
	
	public GameObject[] disabledObjects;
    private GameObject ratePopUp;

    private static bool _bFirstLoadMainScene = true;
	 
	private DailyRewards dailyReward = null;
	
	public static bool BPopUpVisible = false;
	
	private CanvasGroup blockAll;
	private GameObject popUpSpecialOffer;
	private void Start () 
	{
		ActiveMenu = "";
		if(Application.loadedLevelName=="HomeScene")
		{
			ratePopUp = GameObject.Find("PopUps/PopUpRate");

			 
			if(GameObject.Find("PopUps/DailyReward") != null ) dailyReward = GameObject.Find("PopUps/DailyReward").GetComponent <DailyRewards>();
		 
		}

		if (disabledObjects!=null) {
			for(int i=0;i<disabledObjects.Length;i++)
				disabledObjects[i].SetActive(false);
		}
	 
		if(Application.loadedLevelName=="HomeScene" && _bFirstLoadMainScene)
		{
			StartCoroutine(DelayStartMainScene());
		}

	}
	
	private IEnumerator DelayStartMainScene()
	{
		 
		if(blockAll == null) blockAll = GameObject.Find("BlockAll").GetComponent<CanvasGroup>();
		blockAll.blocksRaycasts = true;
		
		yield return new WaitForSeconds(0.5f);
		if(PlayerPrefs.HasKey("alreadyRated"))
		{
			Rate.AlreadyRated = PlayerPrefs.GetInt("alreadyRated");
		}
		else
		{
			Rate.AlreadyRated = 0;
		}

		Rate.AppStartedNumber = PlayerPrefs.GetInt("appStartedNumber",0);

		if(_bFirstLoadMainScene)
		{
			if(Rate.AlreadyRated==0 && Rate.AppStartedNumber>=6 )
			{
				Rate.AppStartedNumber=0;
				PlayerPrefs.SetInt("appStartedNumber",Rate.AppStartedNumber);
				PlayerPrefs.Save();

				ShowPopUpMenu(ratePopUp);
			}
			else
			{
					if( _bFirstLoadMainScene && dailyReward !=null &&  dailyReward.TestDailyRewards())
					{	
						dailyReward.ShowDailyReward();
					}
					else
					{	
						if(dailyReward !=null ) dailyReward.gameObject.SetActive(false);
					}
			}
		}

		_bFirstLoadMainScene = false;
		
		yield return new WaitForSeconds(1.1f);
		blockAll.blocksRaycasts = false;
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
		currentPopUpMenu = menu.GetComponent<Menu> ();
		currentPopUpMenu.OpenMenu();
		ActiveMenu = menu.name; 
		BPopUpVisible = true;
		if(SoundManager.Instance!=null) 
		{
			//if(Time.timeScale>0 ) SoundManager.Instance.Play_PopUpShow(.05f);
			//else 
				SoundManager.Instance.Play_Sound(SoundManager.Instance.PopUpShow);
		}
		if(Application.loadedLevelName != "Room") EscapeButtonManager.AddEscapeButonFunction("ClosePopUpMenuEsc",menu.name);
		 
		 
	}

	/// <summary>
	/// Funkcija za zatvaranje Menu-ja koji je pozvan kao PopUp-a, poziva inace coroutine-u, ima delay zbog animacije odlaska Menu-ja
	/// </summary>
	/// /// <param name="menu">Game object koji se prosledjuje i treba da se skloni, mora imati na sebi skriptu Menu.</param>
	public void ClosePopUpMenu(GameObject menu)
	{
		//StartCoroutine("HidePopUp",menu);
		menu.GetComponent<Menu> ().CloseMenu();
		if(ActiveMenu == menu.name) ActiveMenu = "";
		if(SoundManager.Instance !=null ) SoundManager.Instance.Play_PopUpHide(.4f);
		BPopUpVisible = false;
		if(EscapeButtonManager.EscapeButtonFunctionStack.Count>=1) EscapeButtonManager.EscapeButtonFunctionStack.Pop();


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
		menu.GetComponent<Menu> ().CloseMenu();
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
		if(SoundManager.Instance !=null)SoundManager.Instance.Play_ButtonClick();
	}


    public void OpenPrivacyPolicyLink()
    {
        Application.OpenURL(AdsManager.AdsManager.Instance.privacyPolicyLink);
    }


}
