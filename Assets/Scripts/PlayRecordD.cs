using UnityEngine;

public class PlayRecordD : MonoBehaviour {

	[SerializeField] private bool bRotate = false; 
	private readonly float recordRotSpeedD = -260;
	// Update is called once per frame
	private void Update () {
		if(bRotate)
		{
			transform.Rotate(0,0,Time.deltaTime*recordRotSpeedD);
		}
	}
}
