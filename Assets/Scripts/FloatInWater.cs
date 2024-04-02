using UnityEngine;
using System.Collections;

public class FloatInWater : MonoBehaviour {
	private Vector3 startPos;
	private Vector3 endPos;
	private Vector3 tmpPos;
	
	public float f = 2f;
	public float a = 3f;
	public float f2 = .5f;
	public float a2= 2f;

	public static bool BEmptyWater;
	
	private float time;
	private float time2;
	
	private void Start () {
		BEmptyWater = false;
		startPos = transform.localPosition;
		tmpPos= startPos;
		endPos = new Vector3(0,-1000,0);
	}
	
	// Update is called once per frame
	private void Update () {
		if(BEmptyWater)
		{
			time2 += Time.deltaTime*.5f;
			tmpPos = Vector3.Lerp( startPos,endPos, time2);
//			Debug.Log(EndPos);
			if(time2>1) BEmptyWater = false;
		}

		time+=Time.deltaTime;
      	transform.localPosition = tmpPos + new Vector3( a2 * Mathf.Sin( f2 * time),  a * Mathf.Sin( f * time),0);
	}
}
