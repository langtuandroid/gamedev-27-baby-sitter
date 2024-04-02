using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TemplateScripts;

public class TopMenuButtons : MonoBehaviour {

	private bool bNext = false;
	private bool bPrev = false;
	
	[SerializeField] private ScrollRect scrollRect;
	[SerializeField] private RectTransform content;

	private float speed = 1;
	
	private void Start () {
		MenuChanged();
	}
	
	 
	private void Update () {
		if(bPrev && scrollRect.horizontalNormalizedPosition > 0 ) scrollRect.horizontalNormalizedPosition -= Time.deltaTime *speed;
		else if(bNext && scrollRect.horizontalNormalizedPosition < 1 ) scrollRect.horizontalNormalizedPosition += Time.deltaTime*speed;
	}

	public void ButtonPrevDown()
	{
		if(SoundManager.Instance!=null) SoundManager.Instance.Play_ButtonClick();
		bNext = false;
		bPrev = true;
	}

	public void ButtonPrevUp()
	{
		bNext = false;
		bPrev = false;
	}


	public void ButtonNextDown()
	{
		if(SoundManager.Instance!=null) SoundManager.Instance.Play_ButtonClick();
		bNext = true;
		bPrev = false;
	}

	public void ButtonNextUp()
	{
		bNext = false;
		bPrev = false;
	}

	public void MenuChanged()
	{
		speed =  .35f*  content.rect.width / (1+ (content.rect.width  - scrollRect.GetComponent<RectTransform>().rect.width ) )   ;

		//speed =  0.01f*  (content.rect.width  - scrollRect.GetComponent<RectTransform>().rect.width  ) * scrollRect.GetComponent<RectTransform>().rect.width  / content.rect.width  ;
//		Debug.Log(content.rect.width  +   "           " + scrollRect.GetComponent<RectTransform>().rect.width );
	}


}
