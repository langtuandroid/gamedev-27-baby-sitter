using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BlockClicks : MonoBehaviour {

	public static BlockClicks Instance;

	private CanvasGroup blockAll;
	 
	private void Awake () {
		Instance = this;
		blockAll = GameObject.Find("BlockAll").GetComponent<CanvasGroup>();
	}
	
 
	private IEnumerator _SetBlockAll(float time, bool blockRays)
	{
		if(blockAll == null) blockAll = GameObject.Find("BlockAll").GetComponent<CanvasGroup>();
		yield return new WaitForSeconds(time);
		blockAll.blocksRaycasts = blockRays;

	}

	public void SetBlockAllDelay(float time, bool blockRays)
	{
		StopAllCoroutines();
		StartCoroutine( _SetBlockAll( time, blockRays));
	}

	public void  SetBlockAll(  bool blockRays)
	{
		if(blockAll == null) blockAll = GameObject.Find("BlockAll").GetComponent<CanvasGroup>();
		blockAll.blocksRaycasts = blockRays;
	}
}
