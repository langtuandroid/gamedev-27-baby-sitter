using UnityEngine;


///<summary>
///<para>Scene:All/NameOfScene/NameOfScene1,NameOfScene2,NameOfScene3...</para>
///<para>Object:N/A</para>
///<para>Description: Sample Description </para>
///</summary>

public class IntroLoading : MonoBehaviour {
 
	public void LoadSplashScreen()
	{
		Application.LoadLevel("Splash");
	}
}
