using UnityEngine;
using System.Collections;

public class PlayRecord : MonoBehaviour {

	[SerializeField] private bool bRotate = false; 
	private readonly float recordRotSpeed = -260;
	// Update is called once per frame
	void Update () {
		if(bRotate)
		{
			transform.Rotate(0,0,Time.deltaTime*recordRotSpeed);
		}
	}
}
