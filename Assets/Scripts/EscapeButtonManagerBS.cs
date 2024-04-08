using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TemplateScripts;
using UnityEngine.SceneManagement;

public class EscapeButtonManagerBS : MonoBehaviour {

	private bool bDisableEscE = false;
	public static readonly Stack<string> EscapeButtonFunctionStackK = new Stack<string>();

	private void Start () {
		DontDestroyOnLoad (gameObject);
	}

	private void OnEnable() {
		SceneManager.sceneLoaded += OnSceneLoadedD;
	}

	private void OnDisable() {
		SceneManager.sceneLoaded -= OnSceneLoadedD;
	}

	private void OnSceneLoadedD(Scene scene, LoadSceneMode mode) 
	{
		EscapeButtonFunctionStackK.Clear();
		bDisableEscE = false;
		if(  scene.name == "HomeScene") AddEscapeButtonFunction("ExitGame");
	}
	
	public static void AddEscapeButtonFunction( string functionName, string functionParam = "")
	{
		if(functionParam != "") functionName +="*"+functionParam;
		EscapeButtonFunctionStackK.Push(functionName);
	}
	
	private void Update()
	{
		if( !bDisableEscE  && Input.GetKeyDown(KeyCode.Escape) )
		{
			if(EscapeButtonFunctionStackK.Count>0)
			{
				bDisableEscE = true;

				if( EscapeButtonFunctionStackK.Peek().Contains("*") )
				{
					string[] funcAndParam = EscapeButtonFunctionStackK.Peek().Split('*');
					if(funcAndParam[0] == "ClosePopUpMenuEsc") 
					{
						GameObject.Find("Canvas").SendMessage(funcAndParam[0],funcAndParam[1], SendMessageOptions.DontRequireReceiver);
					}
					else
					{
						Camera.main.SendMessage(funcAndParam[0],funcAndParam[1], SendMessageOptions.DontRequireReceiver);
						EscapeButtonFunctionStackK.Pop();
					}
				}
				else
				{
					if( EscapeButtonFunctionStackK.Count == 1 && EscapeButtonFunctionStackK.Peek() == "btnPauseClick" ) 
						Camera.main.SendMessage("btnPauseClick", SendMessageOptions.DontRequireReceiver);
					else if(EscapeButtonFunctionStackK.Count >= 1 && EscapeButtonFunctionStackK.Peek() == "CloseDailyReward") 
					{
						GameObject.Find("PopUps/DailyReward").GetComponent <DailyRewardsBS>().CollectT();
					}
					else 
						Camera.main.SendMessage(EscapeButtonFunctionStackK.Pop(), SendMessageOptions.DontRequireReceiver);
				}
			} 
			StartCoroutine(nameof(DisableEscE));
		}
	}
	
	private IEnumerator DisableEscE()
	{
		yield return new WaitForSeconds(2);
		bDisableEscE = false;
	}


}



