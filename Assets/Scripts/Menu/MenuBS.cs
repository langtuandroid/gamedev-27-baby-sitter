using UnityEngine;

public class MenuBS : MonoBehaviour {


	private Animator animator;
	private CanvasGroup blockAll;

	
	public void Awake () 
	{
		animator = GetComponent<Animator> ();

		var rect = GetComponent<RectTransform> ();
		rect.offsetMax = rect.offsetMin = new Vector2 (0, 0);
	}

	public void Start()
	{
		blockAll = GameObject.Find("Canvas/BlockAll").GetComponent<CanvasGroup>();
	}

	public void ResetObjectT()
	{
		gameObject.SetActive (false);
	}
	
	public void DisableObject(string gameObjectName)
	{
		GameObject gameObject= GameObject.Find (gameObjectName);
		if (gameObject != null) 
		{
			if (gameObject.activeSelf) 
			{
				gameObject.SetActive (false);
			}
		}
	}

	public void OpenMenuU()
	{
	 
		if(blockAll == null)   blockAll = GameObject.Find("BlockAll").transform.GetComponent<CanvasGroup>();
		animator.SetTrigger("tOpen");
		animator.ResetTrigger("tClose");
		blockAll.blocksRaycasts = true;
	}

	public void CloseMenuU()
	{
		if(blockAll == null)   blockAll = GameObject.Find("BlockAll").transform.GetComponent<CanvasGroup>();
		animator.SetTrigger("tClose");
		animator.ResetTrigger("tOpen");
		blockAll.blocksRaycasts = true;
	}

	public void MenuClosedD()
	{
		if(blockAll == null)   blockAll = GameObject.Find("BlockAll").GetComponent<CanvasGroup>();
//		Debug.Log("Menu closed");
		blockAll.blocksRaycasts = false;
		ResetObjectT();
	}

	public void MenuOpenedD()
	{
		if(blockAll == null)   blockAll = GameObject.Find("BlockAll").GetComponent<CanvasGroup>();
	//	Debug.Log("Menu opened");
		blockAll.blocksRaycasts = false;
	}
	
}
