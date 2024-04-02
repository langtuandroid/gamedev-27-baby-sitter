using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TemplateScripts
{
	/**
  * Scene:Splash
  * Object:Main Camera
  **/
	public class SplashScene : MonoBehaviour {
	
		private int appStartedNumber;
		private AsyncOperation progress;
		private Image progressBar;
		private float myProgress;
		private string sceneToLoad;
 
		private void Start ()
		{
			GameData.Init();
			sceneToLoad =  "HomeScene";

			progressBar = GameObject.Find("ProgressBar").GetComponent<Image>();
			if(PlayerPrefs.HasKey("appStartedNumber"))
			{
				appStartedNumber = PlayerPrefs.GetInt("appStartedNumber");
			}
			else
			{
				appStartedNumber = 0;
			}
			appStartedNumber++;
			PlayerPrefs.SetInt("appStartedNumber",appStartedNumber);
			
			StartCoroutine(LoadScene());
		}
		
		private IEnumerator LoadScene()
		{
			while(myProgress < 0.99)
			{
				myProgress += 0.012f;
				progressBar.fillAmount=myProgress;
				yield return new WaitForSeconds(0.05f);
			}

			SceneManager.LoadScene(sceneToLoad);
		
		}

	}
}
