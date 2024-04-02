using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Serialization;

public class ProgressBar : MonoBehaviour {
	
	[FormerlySerializedAs("ProgressBarFill")] public Image progressBarFill;
	 
	private RectTransform rtBar;
	
	private float lastValue = 0; //0-1
	[FormerlySerializedAs("Value")] [SerializeField] private float value = 0;
 
	private void Awake () {
		progressBarFill.fillAmount = value;
		 
		SetProgress (0,false);

		//SetProgress(.5f,true);
	}

	/// <summary>
	/// Sets the progress.
	/// </summary>
	/// <param name="value">Value (0-1).</param>
	///  <param name="bSmoothChange">postepeno povecavanje ako je true</param>
	public void SetProgress(float value, bool bSmoothChange)
	{
		lastValue = progressBarFill.fillAmount;
		if(value>1) value = 1;
		if(value<0) value = 0; 

		 this.value = value;
 
		if( bSmoothChange  )
		{
			StopCoroutine(nameof(SmoothUpdateProgress));
			StartCoroutine(nameof(SmoothUpdateProgress));
		}
		else
			progressBarFill.fillAmount = this.value;
 
	}
 
	private IEnumerator SmoothUpdateProgress () 
	{
		if(progressBarFill.fillAmount < value)
		{
			while(progressBarFill.fillAmount < value)
			{
				yield return new WaitForFixedUpdate();
				progressBarFill.fillAmount +=Time.fixedDeltaTime*.25f;
			}
		}
		else
		{
			while(progressBarFill.fillAmount > value)
			{
				yield return new WaitForFixedUpdate();
				progressBarFill.fillAmount -=Time.fixedDeltaTime*.5f;
			}
		}

		progressBarFill.fillAmount = value;
 
	}

 
}
