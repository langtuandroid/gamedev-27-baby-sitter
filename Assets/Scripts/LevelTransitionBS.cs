using UnityEngine;
using System.Collections;
using TemplateScripts;

public class LevelTransitionBS : MonoBehaviour {
	
	[SerializeField] private Animator anim;
 
	public static LevelTransitionBS Instance;
	
	private static string _nextLevelName = "";
	private bool bLoadScene = false;  

	private void Start () 
	{
		DontDestroyOnLoad(this.gameObject);
		
		anim = GameObject.Find("TransitionImageC").GetComponent<Animator>();
		anim.gameObject.SetActive(false);
	}

	private void Awake()
	{
		if(_nextLevelName != "" &&  Application.loadedLevelName == "TransitionScene") 
		{
//			Debug.Log("Transition SCENE");
			Application.LoadLevel(_nextLevelName);
			 
			return;
		}
		else
		{
			if(Instance !=null && Instance != this ) GameObject.Destroy(gameObject);
			else Instance = this;
			 
		}
	}

	public void OnLevelWasLoaded(int level)
	{
		if(Application.loadedLevelName != "TransitionScene" )
		{
			anim = GameObject.Find("TransitionImageC").GetComponent<Animator>();
			anim.Play("DefaultClosed"); 
		}
	}
	
	private IEnumerator  WaitAndDestroy()
	{
		yield return new WaitForEndOfFrame();
		if(Instance.anim == null) Instance.anim = GameObject.Find("TransitionImageC").GetComponent<Animator>();
		Instance.anim.gameObject.SetActive(false);
		Destroy(gameObject);
	}


	public void HideSceneAndLoadNext(string levelName)
	{
		if(bLoadScene) return;
		bLoadScene = true;
		_nextLevelName = levelName;
		StopAllCoroutines();
		//StartCoroutine(SetBlockAll(0,true));
		BlockClicksBs.Instance.SetBlockAllL(true);

		anim.gameObject.SetActive(true);
		StartCoroutine(nameof(LoadScene) , levelName);
		
		if(anim != null)  anim.SetBool("bClose",true);

	}

	public void ShowScene()
	{
		if(anim == null) anim = GameObject.Find("TransitionImageC").GetComponent<Animator>();
		if(anim!=null)  anim.SetBool("bClose",false);

		BlockClicksBs.Instance.SetBlockAllL(true);
		BlockClicksBs.Instance.SetBlockAllDelayY(1f,false);
	}

	 
	private IEnumerator LoadScene (string levelName)
	{
		if(SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.StopActiveSoundsOnExitAndClearList();
		yield return new WaitForSeconds(1.2f);
		bLoadScene = false;

		Application.LoadLevel("TransitionScene");
		
	}

	public void HideAndShowSceneWithoutLoading( )
	{
		StopAllCoroutines();
		//StartCoroutine(SetBlockAll(0,true));
		BlockClicksBs.Instance.SetBlockAllL(true);
		 
		anim.gameObject.SetActive(true);
		anim.SetBool("bClose",true);
		StartCoroutine(nameof(WaitHideAndShowScene));

	}

	private IEnumerator WaitHideAndShowScene ( )
	{
		yield return new WaitForSeconds(2f);
		anim.SetBool("bClose",false);
		//StartCoroutine(SetBlockAll(1,false));
		BlockClicksBs.Instance.SetBlockAllL(true);
		BlockClicksBs.Instance.SetBlockAllDelayY(1f,false);
	}

	public void AnimEventHideSceneAnimStarted()
	{

	}

	public void AnimEventShowSceneAnimFinished()
	{
		anim.gameObject.SetActive(false);
	}
}

