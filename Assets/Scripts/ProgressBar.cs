using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Serialization;

public class ProgressBar : MonoBehaviour {
	
	[FormerlySerializedAs("ProgressBarFill")] public Image progressBarFill;
	 
	private RectTransform rtBarR;
	
	private float lastValueE = 0; //0-1
	[FormerlySerializedAs("Value")] [SerializeField] private float value = 0;
 
	private void Awake () {
		
		progressBarFill.fillAmount = value;
		SetProgressBar (0,false);
	}

	public void SetProgressBar(float value, bool bSmoothChange)
	{
		lastValueE = progressBarFill.fillAmount;
		if(value>1) value = 1;
		if(value<0) value = 0; 

		 this.value = value;
 
		if( bSmoothChange  )
		{
			StopCoroutine(nameof(SmoothUpdateProgressBar));
			StartCoroutine(nameof(SmoothUpdateProgressBar));
		}
		else
			progressBarFill.fillAmount = this.value;
 
	}
 
	private IEnumerator SmoothUpdateProgressBar () 
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
