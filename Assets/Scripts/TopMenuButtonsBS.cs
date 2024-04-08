using UnityEngine;
using UnityEngine.UI;
using TemplateScripts;

public class TopMenuButtonsBS : MonoBehaviour {

	private bool bNextT = false;
	private bool bPrevV = false;
	
	[SerializeField] private ScrollRect scrollRect;
	[SerializeField] private RectTransform content;

	private float speedE = 1;
	
	private void Start () {
		MenuChangedDD();
	}
	
	 
	private void Update () {
		if(bPrevV && scrollRect.horizontalNormalizedPosition > 0 ) scrollRect.horizontalNormalizedPosition -= Time.deltaTime *speedE;
		else if(bNextT && scrollRect.horizontalNormalizedPosition < 1 ) scrollRect.horizontalNormalizedPosition += Time.deltaTime*speedE;
	}

	public void ButtonPrevDownN()
	{
		if(SoundManagerBS.Instance!=null) SoundManagerBS.Instance.Play_ButtonClickK();
		bNextT = false;
		bPrevV = true;
	}

	public void ButtonPrevUpP()
	{
		bNextT = false;
		bPrevV = false;
	}


	public void ButtonNextDownN()
	{
		if(SoundManagerBS.Instance!=null) SoundManagerBS.Instance.Play_ButtonClickK();
		bNextT = true;
		bPrevV = false;
	}

	public void ButtonNextUpP()
	{
		bNextT = false;
		bPrevV = false;
	}

	public void MenuChangedDD()
	{
		speedE =  .35f*  content.rect.width / (1+ (content.rect.width  - scrollRect.GetComponent<RectTransform>().rect.width ) )   ;
	}


}
