using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TemplateScripts;
using UnityEngine.Serialization;

public class TopMenu : MonoBehaviour {
	private int activeMenu = 0;
	
	[SerializeField] private Transform scrollContent;
	[SerializeField] private ScrollRect scrollRect;

	[SerializeField] private Animator animTopMenu;

	[SerializeField] private Transform btnPref;

	[FormerlySerializedAs("ItemiSprites")] [SerializeField] private Sprite[] itemiSprites; 
	private Sprite[] itemSprites2 ;

	private readonly List<Transform> itemButtons = new List<Transform>();
	[FormerlySerializedAs("HidenButtonsHolder")] [SerializeField] private Transform hidenButtonsHolder;
	
	private readonly MenuItems menuItems = new MenuItems();

	private Image decoration1;
	private Image decoration2;
	
	[SerializeField] private Transform btnMenuItemPref;
	
	[FormerlySerializedAs("ActiveItemHolder")] [SerializeField] private Transform activeItemHolder;
	[FormerlySerializedAs("DragTargetPositions")] [SerializeField] private Transform[] dragTargetPositions;
	
	public void SetMenuItems (int subMenuNo) //mg1
	{
		activeMenu = subMenuNo;
		Dictionary <string,MenuItemData> m = menuItems.ReturmMenu(subMenuNo);
		
		//----------------------------------------------------
		menuItems.ReturnMenuImages(subMenuNo, out itemSprites2,m.Count);
		
		//----------------------------------------------------
		string tmp =  "M" + subMenuNo.ToString().PadLeft(2,'0') + "_";
		
		float scrollContentSize = 190;
		float btnWidth = 190;
		
		scrollContentSize = btnWidth * m.Count   ;  
		scrollContent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal  ,scrollContentSize  );
		
		
		//POPUNA ITEMA
		
		//sakrivanje ako ima vise u meniju
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
			TopMenuItem ti;
			//ako vec postoji kreirano dugme
			if(i<=itemButtons.Count)
			{
				btn = itemButtons[i-1];
				btn.SetParent(scrollContent);
				btn.name =   tmp + i.ToString().PadLeft(2,'0');   
				ti = btn.GetComponent<TopMenuItem>();
				ti.activeItemHolder = activeItemHolder;

				ti.targetPoint = new Transform[1];
				ti.targetPoint[0] = dragTargetPositions[activeMenu-1];
 
				//btn.GetComponent<Button>().onClick.RemoveAllListeners();
				//btn.GetComponent<Button>().onClick.AddListener(() =>ButtonItemClicked(btn.name));
				
				//sredi zakljucavanje itema
				//btn.GetComponent<Button>().enabled = !d.Locked;
			}
			else //kreira se novo dugme
			{
				btn = (Transform) GameObject.Instantiate(btnMenuItemPref);
				btn.SetParent(scrollContent);
				btn.name =   tmp + i.ToString().PadLeft(2,'0');  
				ti = btn.GetComponent<TopMenuItem>();
				ti.activeItemHolder = activeItemHolder;

				ti.targetPoint = new Transform[1];
				ti.targetPoint[0] = dragTargetPositions[activeMenu-1];


				//btn.GetComponent<Button>().onClick.RemoveAllListeners();
				//btn.GetComponent<Button>().onClick.AddListener(() =>ButtonItemClicked(btn.name));
				
				//sredi zakljucavanje itema
				//btn.GetComponent<Button>().enabled = !d.Locked;
				itemButtons.Add(btn);
			}

			//--------PODESAVANJE SLIKE----------------------------------------------
			btn.transform.localScale = Vector3.one;
			ti.activeMenu = activeMenu;
			if(d.ButtonImgName == "")
			{
				foreach( Sprite sp in  itemSprites2)
				{
					if( sp !=null &&     sp.name  == d.Name.Trim())
					{
						Image im = btn.GetChild(0).GetComponent<Image>();
						im.sprite = sp; 
						ti.imageSize = d.ImgeSize;
						ti.bChangeSpriteOnDrag = false;
						ti.dragSprite = null;
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
						ti.bChangeSpriteOnDrag = true;

						foreach( Sprite dragSprite in  itemSprites2)
						{
							if( dragSprite !=null &&  dragSprite.name == d.Name  )
							{
								ti.dragSprite  = dragSprite; 	
								break;
							}
						}
						 
						break;
					}
				}
			}
 

			//ti.bLocked = d.Locked;
			//ti.SetLockImage();
		}

		transform.SendMessage("MenuChanged");
		scrollRect.horizontalNormalizedPosition = 0;
	}

 

	//--------------------------------------------------
	public void ButtonItemClicked(string item)
	{

		if(MiniGame1.CompletedActionNo < activeMenu+1) return;


		//		Debug.Log(item);
		StopAllCoroutines();
		Sprite sprite = null;
		MenuItemData it =MenuItems.mitd[item];
		foreach( Sprite sp in  itemSprites2)
		{
			if( sp !=null &&  sp.name == it.Name  )
			{
				sprite = sp; 	break;
			}
		}
 
		decoration1 = GameObject.Find("Canvas/Baby/P"+activeMenu.ToString()+"/Decoration1").GetComponent<Image>();
		decoration2 = GameObject.Find("Canvas/Baby/P"+activeMenu.ToString()+"/Decoration2").GetComponent<Image>();
			 
		if(sprite != null) 
		{
			if(decoration1.sprite == null)
				StartCoroutine(  DecorationFadeIn(decoration1,sprite));
			else 
				StartCoroutine( DecorationCrossFadeAndSwap(decoration1,decoration2,sprite,true ));

			//GameObject.Find("CanvasBG/SceneGraphics/Baby/P"+activeMenu.ToString()+"/Particles").GetComponent<ParticleSystem>().Play();	
		}
		else
		{
			BlockClicks.Instance.SetBlockAll(true);
			BlockClicks.Instance.SetBlockAllDelay(.3f,false);
		}
 
		 
		if(SoundManager.Instance != null) SoundManager.Instance.StopAndPlay_Sound(SoundManager.Instance.ButtonClick);
		//SoundManager.Instance.StopAndPlay_Sound(SoundManager.Instance.ElementCompleted);
	}

	private IEnumerator DecorationFadeIn(Image img,   Sprite sp)
	{
		BlockClicks.Instance.SetBlockAll(true);
		BlockClicks.Instance.SetBlockAllDelay(.5f,false);
		
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

	private IEnumerator DecorationCrossFadeAndSwap(Image img1, Image img2,  Sprite sp, bool swapPositions = false)
	{
		BlockClicks.Instance.SetBlockAll(true);
		BlockClicks.Instance.SetBlockAllDelay(.5f,false);
		
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
	
	public void ShowTopMenu()
	{
		animTopMenu.CrossFade("ShowMenu",.1f);
	}

	public void HideTopMenu()
	{
		animTopMenu.CrossFade("HideMenu",.1f);
	}
}
