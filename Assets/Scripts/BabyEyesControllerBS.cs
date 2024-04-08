using UnityEngine;
using System.Collections;

public class BabyEyesControllerBS : MonoBehaviour {
	
	private Transform eyeLeftT;
	private Transform eyeRightT;

	private Vector3 targetPosLeftT;
	private Vector3 targetPosRightT;

	private float timeLastMbb = 0;
	// Use this for initialization
	private void Start () {
		Input.multiTouchEnabled = false;
		eyeLeftT = transform.Find("EyeLeft/Eye_Left/EyeWhite/EyeBallHolder/EyeBall");
		eyeRightT = transform.Find("EyeRight/Eye_Rigth/EyeWhite/EyeBallHolder/EyeBall");
		 
	}
	
	// Update is called once per frame
	private void Update () {

		if( Input.GetMouseButton(0))
		{
			targetPosLeftT =   (  eyeLeftT.parent.position  - Camera.main.ScreenToWorldPoint(Input.mousePosition));
			targetPosLeftT = new Vector3(targetPosLeftT.x,-targetPosLeftT.y,0).normalized  *10 ;
			eyeLeftT.localPosition = Vector3.Lerp(eyeLeftT.localPosition, targetPosLeftT,Time.deltaTime*4);

			targetPosRightT =   (  eyeRightT.parent.position  - Camera.main.ScreenToWorldPoint(Input.mousePosition));
			targetPosRightT = new Vector3(-targetPosRightT.x,-targetPosRightT.y,0).normalized  *10 ;
			eyeRightT.localPosition = Vector3.Lerp(eyeRightT.localPosition, targetPosRightT,Time.deltaTime*4);

			timeLastMbb = 0;
		}
		else if(timeLastMbb<2)
		{
			eyeLeftT.localPosition = Vector3.Lerp(eyeLeftT.localPosition, Vector3.zero,Time.deltaTime*4);
			eyeRightT.localPosition = Vector3.Lerp(eyeRightT.localPosition, Vector3.zero,Time.deltaTime*4);
			timeLastMbb +=Time.deltaTime;
		}


	}
}
