using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class BubblesListHolder : MonoBehaviour {

	[FormerlySerializedAs("bubblesAnim")] public List< Animator> bubblesAnimM= new  List<Animator>();

	public void WashBubbles()
	{
		for (int j = bubblesAnimM.Count-1; j >=0; j--) 
		{
			bubblesAnimM[j].SetTrigger("tHide");
			Destroy(bubblesAnimM[j].gameObject,2f);
		}	 
		bubblesAnimM.Clear();
	}
}
