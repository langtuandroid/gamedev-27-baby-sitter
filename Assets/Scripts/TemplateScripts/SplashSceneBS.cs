using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TemplateScripts
{
	public class SplashSceneBS : MonoBehaviour {
	
		private int appStartedNumberR;
		private AsyncOperation progressS;
		private Image progressBarR;
		private float myProgressS;
		private string sceneToLoadD;
 
		private void Start ()
		{
			GameDataBS.Init();
			sceneToLoadD =  "HomeScene";

			progressBarR = GameObject.Find("ProgressBar").GetComponent<Image>();
			if(PlayerPrefs.HasKey("appStartedNumber"))
			{
				appStartedNumberR = PlayerPrefs.GetInt("appStartedNumber");
			}
			else
			{
				appStartedNumberR = 0;
			}
			appStartedNumberR++;
			PlayerPrefs.SetInt("appStartedNumber",appStartedNumberR);
			
			StartCoroutine(LoadScene());
		}
		
		private IEnumerator LoadScene()
		{
			while(myProgressS < 0.99)
			{
				myProgressS += 0.012f;
				progressBarR.fillAmount=myProgressS;
				yield return new WaitForSeconds(0.05f);
			}

			SceneManager.LoadScene(sceneToLoadD);
		
		}

	}
}
