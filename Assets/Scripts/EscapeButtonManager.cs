using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TemplateScripts;
using UnityEngine.SceneManagement;

public class EscapeButtonManager : MonoBehaviour {

	private bool bDisableEsc = false;
	public static readonly Stack<string> EscapeButtonFunctionStack = new Stack<string>();

	private void Start () {
		DontDestroyOnLoad (gameObject);
	}

	private void OnEnable() {
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnDisable() {
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
	{
		EscapeButtonFunctionStack.Clear();
		bDisableEsc = false;
		if(  scene.name == "HomeScene") AddEscapeButonFunction("ExitGame");
	}
	
	public static void  AddEscapeButonFunction( string functionName, string functionParam = "")
	{
		if(functionParam != "") functionName +="*"+functionParam;
		EscapeButtonFunctionStack.Push(functionName);
	}
	
	private void Update()
	{
		if( !bDisableEsc  && Input.GetKeyDown(KeyCode.Escape) )
		{
			if(EscapeButtonFunctionStack.Count>0)
			{
				bDisableEsc = true;

				if( EscapeButtonFunctionStack.Peek().Contains("*") )
				{
					string[] funcAndParam = EscapeButtonFunctionStack.Peek().Split('*');
					if(funcAndParam[0] == "ClosePopUpMenuEsc") 
					{
						GameObject.Find("Canvas").SendMessage(funcAndParam[0],funcAndParam[1], SendMessageOptions.DontRequireReceiver);
					}
					else
					{
						Camera.main.SendMessage(funcAndParam[0],funcAndParam[1], SendMessageOptions.DontRequireReceiver);
						EscapeButtonFunctionStack.Pop();
					}
				}
				else
				{
					if( EscapeButtonFunctionStack.Count == 1 && EscapeButtonFunctionStack.Peek() == "btnPauseClick" ) 
						Camera.main.SendMessage("btnPauseClick", SendMessageOptions.DontRequireReceiver);
					else if(EscapeButtonFunctionStack.Count >= 1 && EscapeButtonFunctionStack.Peek() == "CloseDailyReward") 
					{
						GameObject.Find("PopUps/DailyReward").GetComponent <DailyRewards>().Collect();
					}
					else 
						Camera.main.SendMessage(EscapeButtonFunctionStack.Pop(), SendMessageOptions.DontRequireReceiver);
				}
			} 
			StartCoroutine(nameof(DisableEsc));
		}
	}
	
	private IEnumerator DisableEsc()
	{
		yield return new WaitForSeconds(2);
		bDisableEsc = false;
	}


}



