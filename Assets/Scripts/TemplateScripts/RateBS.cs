using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TemplateScripts
{
	public class RateBS : MonoBehaviour {

		private string rateURL;
		
		[Header("Links for rate")]
		
		[SerializeField] private string rateUrlAndroid;
		[SerializeField] private string rateUrlIOS;
		[SerializeField] private string rateUrlWinPhone;
		[SerializeField] private string rateUrlWinStore;
		[SerializeField] private string rateUrlMAC;
		
		public static int AppStartedNumber,AlreadyRated;
		
		private bool rateClicked = false;
		
		private void Start () {

#if UNITY_ANDROID
			rateURL = rateUrlAndroid;
#elif UNITY_IOS
		rateURL = rateUrlIOS;
#elif (UNITY_WP8 || UNITY_WP8_1)
		rateURL = rateUrlWinPhone;
#elif (UNITY_WSA_8_0 || UNITY_WSA_8_1 || UNITY_WSA_10_0)
		rateURL = rateUrlWinStore;
#elif UNITY_STANDALONE_OSX
		rateURL = rateUrlMAC;
#endif
		}
		
		public void RateClicked(int number)
		{
			if(!rateClicked)
			{
				AlreadyRated = 1;
				PlayerPrefs.SetInt("alreadyRated",AlreadyRated);
				PlayerPrefs.Save();
				rateClicked=true;
				StartCoroutine(nameof(ActivateStars),number);
			}
		}

		private IEnumerator ActivateStars(int number)
		{
			switch(number)
			{
				case 1: case 2: case 3:
					for(int i=1;i<=number;i++)
					{
						GameObject.Find("PopUpRate/AnimationHolder/Body/ContentHolder/StarsHolder/StarBG"+i+"/Star"+i).GetComponent<Image>().enabled = true;
					}
					yield return new WaitForSeconds(0.5f);
					HideRateMenu(GameObject.Find("PopUpRate"));
					break;
				case 4: case 5:
					for(int i=1;i<=number;i++)
					{
						GameObject.Find("PopUpRate/AnimationHolder/Body/ContentHolder/StarsHolder/StarBG"+i+"/Star"+i).GetComponent<Image>().enabled = true;
					}
					Application.OpenURL(rateURL);
					yield return new WaitForSeconds(0.5f);
					HideRateMenu(GameObject.Find("PopUpRate"));
					yield return new WaitForSeconds(0.5f);

					break;
			}
			yield return null;
			AlreadyRated = 1;
			PlayerPrefs.SetInt("alreadyRated",AlreadyRated);
			PlayerPrefs.Save();

		}
		
		public void ShowRateMenu()
		{
			transform.GetComponent<Animator>().Play("Open");
		}
		
		public void HideRateMenu(GameObject menu)
		{
			GameObject.Find("Canvas").GetComponent<MenuManagerBS>().ClosePopUpMenu(menu);
		}
		
		public void NoThanks()
		{

			AlreadyRated = 1;
			PlayerPrefs.SetInt("alreadyRated",AlreadyRated);
			PlayerPrefs.Save();
			HideRateMenu(GameObject.Find("PopUpRate"));
		}
	}
}
