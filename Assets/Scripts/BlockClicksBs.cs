using UnityEngine;
using System.Collections;

public class BlockClicksBs : MonoBehaviour {

	public static BlockClicksBs Instance;

	private CanvasGroup blockAllL;
	 
	private void Awake () {
		Instance = this;
		blockAllL = GameObject.Find("BlockAll").GetComponent<CanvasGroup>();
	}
	
 
	private IEnumerator SetBlockAllL(float time, bool blockRays)
	{
		if(blockAllL == null) blockAllL = GameObject.Find("BlockAll").GetComponent<CanvasGroup>();
		yield return new WaitForSeconds(time);
		blockAllL.blocksRaycasts = blockRays;

	}

	public void SetBlockAllDelayY(float time, bool blockRays)
	{
		StopAllCoroutines();
		StartCoroutine( SetBlockAllL( time, blockRays));
	}

	public void SetBlockAllL(  bool blockRays)
	{
		if(blockAllL == null) blockAllL = GameObject.Find("BlockAll").GetComponent<CanvasGroup>();
		blockAllL.blocksRaycasts = blockRays;
	}
}
