using UnityEngine;
using System.Collections;

public class BabyEyesController : MonoBehaviour {
	
	private Transform eyeLeft;
	private Transform eyeRight;

	private Vector3 targetPosLeft;
	private Vector3 targetPosRight;

	private float timeLastMb = 0;
	// Use this for initialization
	private void Start () {
		Input.multiTouchEnabled = false;
		eyeLeft = transform.Find("EyeLeft/Eye_Left/EyeWhite/EyeBallHolder/EyeBall");
		eyeRight = transform.Find("EyeRight/Eye_Rigth/EyeWhite/EyeBallHolder/EyeBall");
		 
	}
	
	// Update is called once per frame
	private void Update () {

		if( Input.GetMouseButton(0))
		{
			targetPosLeft =   (  eyeLeft.parent.position  - Camera.main.ScreenToWorldPoint(Input.mousePosition));
			targetPosLeft = new Vector3(targetPosLeft.x,-targetPosLeft.y,0).normalized  *10 ;
			eyeLeft.localPosition = Vector3.Lerp(eyeLeft.localPosition, targetPosLeft,Time.deltaTime*4);

			targetPosRight =   (  eyeRight.parent.position  - Camera.main.ScreenToWorldPoint(Input.mousePosition));
			targetPosRight = new Vector3(-targetPosRight.x,-targetPosRight.y,0).normalized  *10 ;
			eyeRight.localPosition = Vector3.Lerp(eyeRight.localPosition, targetPosRight,Time.deltaTime*4);

			timeLastMb = 0;
		}
		else if(timeLastMb<2)
		{
			eyeLeft.localPosition = Vector3.Lerp(eyeLeft.localPosition, Vector3.zero,Time.deltaTime*4);
			eyeRight.localPosition = Vector3.Lerp(eyeRight.localPosition, Vector3.zero,Time.deltaTime*4);
			timeLastMb +=Time.deltaTime;
		}


	}
}
