using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class FloatInWaterBS : MonoBehaviour {
	private Vector3 startPosS;
	private Vector3 endPosS;
	private Vector3 tmpPosS;
	
	[FormerlySerializedAs("f")] public float fF = 2f;
	[FormerlySerializedAs("a")] public float aA = 3f;
	[FormerlySerializedAs("f2")] public float fF2 = .5f;
	[FormerlySerializedAs("a2")] public float aA2= 2f;

	public static bool BEmptyWaterR;
	
	private float timeE;
	private float timeE2;
	
	private void Start () {
		BEmptyWaterR = false;
		startPosS = transform.localPosition;
		tmpPosS= startPosS;
		endPosS = new Vector3(0,-1000,0);
	}
	
	// Update is called once per frame
	private void Update () {
		if(BEmptyWaterR)
		{
			timeE2 += Time.deltaTime*.5f;
			tmpPosS = Vector3.Lerp( startPosS,endPosS, timeE2);
//			Debug.Log(EndPos);
			if(timeE2>1) BEmptyWaterR = false;
		}

		timeE+=Time.deltaTime;
      	transform.localPosition = tmpPosS + new Vector3( aA2 * Mathf.Sin( fF2 * timeE),  aA * Mathf.Sin( fF * timeE),0);
	}
}
