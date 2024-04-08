using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TemplateScripts;
using UnityEngine.Serialization;

public class TopMenuBS : MonoBehaviour {
	private int activeMenuU = 0;
	
	[SerializeField] private Transform scrollContent;
	[SerializeField] private ScrollRect scrollRect;

	[SerializeField] private Animator animTopMenu;

	[SerializeField] private Transform btnPref;

	[FormerlySerializedAs("ItemiSprites")] [SerializeField] private Sprite[] itemiSprites; 
	private Sprite[] itemSpritesS2 ;

	private readonly List<Transform> itemButtons = new List<Transform>();
	[FormerlySerializedAs("HidenButtonsHolder")] [SerializeField] private Transform hidenButtonsHolder;
	
	private readonly MenuItemsBS menuItemsBsS = new MenuItemsBS();

	private Image decorationN1;
	private Image decorationN2;
	
	[SerializeField] private Transform btnMenuItemPref;
	
	[FormerlySerializedAs("ActiveItemHolder")] [SerializeField] private Transform activeItemHolder;
	[FormerlySerializedAs("DragTargetPositions")] [SerializeField] private Transform[] dragTargetPositions;
	
	public void SetMenuItemsS(int subMenuNo)
	{
		activeMenuU = subMenuNo;
		Dictionary <string,MenuItemData> m = menuItemsBsS.ReturmMenu(subMenuNo);
		
		menuItemsBsS.ReturnMenuImages(subMenuNo, out itemSpritesS2,m.Count);
	
		string tmp =  "M" + subMenuNo.ToString().PadLeft(2,'0') + "_";
		
		float scrollContentSize = 190;
		float btnWidth = 190;
		
		scrollContentSize = btnWidth * m.Count   ;  
		scrollContent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal  ,scrollContentSize  );
		
		if(m.Count<itemButtons.Count)
		{
			for(int i = m.Count; i<itemButtons.Count;i++)
			{
				itemButtons[i].SetParent(hidenButtonsHolder);
				itemButtons[i].GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			}
		}
		
		
		for(int i = 1; i<=m.Count;i++)
		{
			MenuItemData d = m[ tmp + i.ToString().PadLeft(2,'0')];
			Transform btn;
			TopMenuItemBS ti;
		
			if(i<=itemButtons.Count)
			{
				btn = itemButtons[i-1];
				btn.SetParent(scrollContent);
				btn.name =   tmp + i.ToString().PadLeft(2,'0');   
				ti = btn.GetComponent<TopMenuItemBS>();
				ti.activeItemHolder = activeItemHolder;

				ti.targetPoint = new Transform[1];
				ti.targetPoint[0] = dragTargetPositions[activeMenuU-1];
			}
			else 
			{
				btn = (Transform) GameObject.Instantiate(btnMenuItemPref);
				btn.SetParent(scrollContent);
				btn.name =   tmp + i.ToString().PadLeft(2,'0');  
				ti = btn.GetComponent<TopMenuItemBS>();
				ti.activeItemHolder = activeItemHolder;

				ti.targetPoint = new Transform[1];
				ti.targetPoint[0] = dragTargetPositions[activeMenuU-1];

				itemButtons.Add(btn);
			}
			
			btn.transform.localScale = Vector3.one;
			ti.activeMenu = activeMenuU;
			if(d.ButtonImgName == "")
			{
				foreach( Sprite sp in  itemSpritesS2)
				{
					if( sp !=null &&     sp.name  == d.Name.Trim())
					{
						Image im = btn.GetChild(0).GetComponent<Image>();
						im.sprite = sp; 
						ti.imageSize = d.ImgeSize;
						ti.bChangeSpriteOnDragG = false;
						ti.dragSpriteE = null;
						break;
					}
				}
			}
			else
			{
				foreach( Sprite sp in  itemiSprites)
				{
					if( ( sp.texture.name+"/"+ sp.name )  == d.ButtonImgName.Trim())
					{
						Image im = btn.GetChild(0).GetComponent<Image>();
						im.sprite = sp;
						ti.imageSize = d.ImgeSize;
						ti.bChangeSpriteOnDragG = true;

						foreach( Sprite dragSprite in  itemSpritesS2)
						{
							if( dragSprite !=null &&  dragSprite.name == d.Name  )
							{
								ti.dragSpriteE  = dragSprite; 	
								break;
							}
						}
						 
						break;
					}
				}
			}
		}

		transform.SendMessage("MenuChanged");
		scrollRect.horizontalNormalizedPosition = 0;
	}

	public void ButtonItemClickedD(string item)
	{
		if(MiniGame1BS.CompletedActionNoN < activeMenuU+1) return;
		
		StopAllCoroutines();
		Sprite sprite = null;
		MenuItemData it =MenuItemsBS.mitdD[item];
		foreach( Sprite sp in  itemSpritesS2)
		{
			if( sp !=null &&  sp.name == it.Name  )
			{
				sprite = sp; 	break;
			}
		}
 
		decorationN1 = GameObject.Find("Canvas/Baby/P"+activeMenuU.ToString()+"/Decoration1").GetComponent<Image>();
		decorationN2 = GameObject.Find("Canvas/Baby/P"+activeMenuU.ToString()+"/Decoration2").GetComponent<Image>();
			 
		if(sprite != null) 
		{
			if(decorationN1.sprite == null)
				StartCoroutine(  DecorationFadeInN(decorationN1,sprite));
			else 
				StartCoroutine( DecorationCrossFadeAndSwapP(decorationN1,decorationN2,sprite,true ));
		}
		else
		{
			BlockClicksBs.Instance.SetBlockAllL(true);
			BlockClicksBs.Instance.SetBlockAllDelayY(.3f,false);
		}
 
		 
		if(SoundManagerBS.Instance != null) SoundManagerBS.Instance.StopAndPlay_Sound(SoundManagerBS.Instance.ButtonClick);
	}

	private IEnumerator DecorationFadeInN(Image img,   Sprite sp)
	{
		BlockClicksBs.Instance.SetBlockAllL(true);
		BlockClicksBs.Instance.SetBlockAllDelayY(.5f,false);
		
		img.sprite = sp;
		img.color = new Color(1,1,1,0);
		yield return null;
		float fadeInTime = 0;
		while(fadeInTime<1)
		{
			fadeInTime+=Time.fixedDeltaTime*2;
			img.color = new Color(1,1,1,fadeInTime);
			yield return new WaitForFixedUpdate();
		}
		img.color = new Color(1,1,1,1);
		Camera.main.SendMessage("CompletedAction");
	}

	private IEnumerator DecorationCrossFadeAndSwapP(Image img1, Image img2,  Sprite sp, bool swapPositions = false)
	{
		BlockClicksBs.Instance.SetBlockAllL(true);
		BlockClicksBs.Instance.SetBlockAllDelayY(.5f,false);
		
		img2.sprite = sp;
		img2.color = new Color(1,1,1,0);
		yield return null;
		float fadeInTime = 0;
		while(fadeInTime<1)
		{
			fadeInTime+=Time.fixedDeltaTime*2;
			img1.color = new Color(1,1,1,1-fadeInTime);
			img2.color = new Color(1,1,1, fadeInTime);
			yield return new WaitForFixedUpdate();
		}
		img1.color = new Color(1,1,1,1);
		img1.sprite = sp;

		img2.color = new Color(1,1,1,0);
		img2.sprite = null;
		img1.rectTransform.sizeDelta = img2.rectTransform.sizeDelta; 
		
		if(swapPositions)
		{
			(img1.transform.position, img2.transform.position) = (img2.transform.position, img1.transform.position);
		}
	}
	
	public void ShowTopMenuU()
	{
		animTopMenu.CrossFade("ShowMenu",.1f);
	}

	public void HideTopMenuU()
	{
		animTopMenu.CrossFade("HideMenu",.1f);
	}
}
