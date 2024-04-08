using UnityEngine;
using System.Collections;
using TemplateScripts;

public class LevelTransitionBS : MonoBehaviour {
	
	[SerializeField] private Animator anim;
 
	public static LevelTransitionBS Instance;
	
	private static string _nextLevelNameE = "";
	private bool bLoadSceneE = false;  

	private void Start () 
	{
		DontDestroyOnLoad(this.gameObject);
		
		anim = GameObject.Find("TransitionImageC").GetComponent<Animator>();
		anim.gameObject.SetActive(false);
	}

	private void Awake()
	{
		if(_nextLevelNameE != "" &&  Application.loadedLevelName == "TransitionScene") 
		{
			Application.LoadLevel(_nextLevelNameE);
		}
		else
		{
			if(Instance !=null && Instance != this ) GameObject.Destroy(gameObject);
			else Instance = this;
			 
		}
	}

	public void OnLevelWasLoadedD(int level)
	{
		if(Application.loadedLevelName != "TransitionScene" )
		{
			anim = GameObject.Find("TransitionImageC").GetComponent<Animator>();
			anim.Play("DefaultClosed"); 
		}
	}
	
	private IEnumerator WaitAndDestroy()
	{
		yield return new WaitForEndOfFrame();
		if(Instance.anim == null) Instance.anim = GameObject.Find("TransitionImageC").GetComponent<Animator>();
		Instance.anim.gameObject.SetActive(false);
		Destroy(gameObject);
	}


	public void HideSceneAndLoadNextT(string levelName)
	{
		if(bLoadSceneE) return;
		bLoadSceneE = true;
		_nextLevelNameE = levelName;
		StopAllCoroutines();
		//StartCoroutine(SetBlockAll(0,true));
		BlockClicksBs.Instance.SetBlockAllL(true);

		anim.gameObject.SetActive(true);
		StartCoroutine(nameof(LoadSceneE) , levelName);
		
		if(anim != null)  anim.SetBool("bClose",true);

	}

	public void ShowSceneE()
	{
		if(anim == null) anim = GameObject.Find("TransitionImageC").GetComponent<Animator>();
		if(anim!=null)  anim.SetBool("bClose",false);

		BlockClicksBs.Instance.SetBlockAllL(true);
		BlockClicksBs.Instance.SetBlockAllDelayY(1f,false);
	}

	 
	private IEnumerator LoadSceneE (string levelName)
	{
		if(SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.StopActiveSoundsOnExitAndClearList();
		yield return new WaitForSeconds(1.2f);
		bLoadSceneE = false;

		Application.LoadLevel("TransitionScene");
		
	}

	public void HideAndShowSceneWithoutLoadingG( )
	{
		StopAllCoroutines();
		//StartCoroutine(SetBlockAll(0,true));
		BlockClicksBs.Instance.SetBlockAllL(true);
		 
		anim.gameObject.SetActive(true);
		anim.SetBool("bClose",true);
		StartCoroutine(nameof(WaitHideAndShowSceneE));

	}

	private IEnumerator WaitHideAndShowSceneE ( )
	{
		yield return new WaitForSeconds(2f);
		anim.SetBool("bClose",false);
		//StartCoroutine(SetBlockAll(1,false));
		BlockClicksBs.Instance.SetBlockAllL(true);
		BlockClicksBs.Instance.SetBlockAllDelayY(1f,false);
	}

	public void AnimEventHideSceneAnimStartedD()
	{

	}

	public void AnimEventShowSceneAnimFinishedD()
	{
		anim.gameObject.SetActive(false);
	}
	
}

