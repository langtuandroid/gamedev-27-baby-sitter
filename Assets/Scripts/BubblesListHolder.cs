using UnityEngine;
using System.Collections.Generic;

public class BubblesListHolder : MonoBehaviour {

	public List< Animator> bubblesAnim= new  List<Animator>();

	public void WashBubbles()
	{
		for (int j = bubblesAnim.Count-1; j >=0; j--) 
		{
			bubblesAnim[j].SetTrigger("tHide");
			Destroy(bubblesAnim[j].gameObject,2f);
		}	 
		bubblesAnim.Clear();
	}
}
