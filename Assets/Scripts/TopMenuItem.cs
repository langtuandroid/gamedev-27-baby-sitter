using UnityEngine;
using System.Collections;
using TemplateScripts;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;


public class TopMenuItem : MonoBehaviour,  IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
	private static TopMenuItem Instance { set; get; }
	private bool bLocked = false;

	private bool bDrag = false;
	private readonly bool bEnableDrag = true;
	 
	private bool bSnapToTarget = false;
	
	private float x;
	private float y;
	
	private Vector3 dragOffset = new Vector3(0,0,0);
	
	private Vector3 startPosition;
	private Transform activeItem;
	[FormerlySerializedAs("ActiveItemHolder")] public Transform activeItemHolder;
	private readonly int mouseFollowSpeed = 10; 
	
	[FormerlySerializedAs("TargetPoint")] public Transform[] targetPoint;
	
	public GameObject prefabDragItem;
	
	private readonly float disposeItemDistance = 5f;
	
	private readonly int targetPointIndex = 0;

	public Sprite dragSprite;
	public bool bChangeSpriteOnDrag = false;
	public bool bChangeIconSpriteOnDrag = false;
	public Sprite emptyIconSprite;
	
	[FormerlySerializedAs("imgeSize")] [FormerlySerializedAs("ImgeSize")] public Vector2 imageSize = Vector2.zero;
	[SerializeField] public int activeMenu = 0;

	private static Image _selectedRecord = null;
	private static Sprite _selectedRecordSprite = null;

	private bool bSelectable = true;

	public bool bUseSnapDistance = false;
	[FormerlySerializedAs("SnapDistance")] public float snapDistance = 1f;
	public Vector3 testPointOffset;

	public GameObject TestTopMovementLimit;
	public GameObject TopMovementLimit;
	
	private bool appFocus = true;

	private void Start ()
	{
		Instance = this;
		_selectedRecord = null;
		_selectedRecordSprite = null;
		startPosition = transform.position;
		//SetLockImage();
	}
	 

	public void SetLockImage(  )
	{
		bLocked = MenuItems.mitd[transform.name].Locked;
		transform.GetChild(1).gameObject.SetActive(bLocked);
	}

	public void Unlock()
	{
		MenuItems.mitd[transform.name].Locked = false;
		bLocked = false;
		transform.GetChild(1).gameObject.SetActive(false);
		GameData.SaveUnlockedItemsToPp(transform.name.Remove(0,2));
	}
 
	private void Update () 
	{
		if( bDrag  && bEnableDrag)  
		{
			x = Input.mousePosition.x;
			y = Input.mousePosition.y;
			
			Vector3 posM = Camera.main.ScreenToWorldPoint(new Vector3(x ,y,10.0f) );

			if(TestTopMovementLimit!=null && TopMovementLimit!=null)
			{
				float diffTL = TestTopMovementLimit.transform.position.y-activeItem.position.y;
				if(posM.y+ diffTL>TopMovementLimit.transform.position.y) posM = new Vector3(posM.x, TopMovementLimit.transform.position.y - diffTL ,posM.z);
			}
			activeItem.position =  Vector3.Lerp (activeItem.position, posM  , mouseFollowSpeed * Time.deltaTime)  ;
		}
	}
	
	public void OnBeginDrag (PointerEventData eventData)
	{

		if(  !bEnableDrag || !bSelectable || bLocked) return;

		if( Application.loadedLevelName == "Minigame 1"  && MiniGame1.CompletedActionNo < activeMenu+1) return;



		bSnapToTarget = false;
		bDrag = true;

		activeItem = GameObject.Instantiate(prefabDragItem).transform;

		if(bChangeSpriteOnDrag && dragSprite !=null ) activeItem.GetComponent<Image>().sprite = dragSprite;
		else 	activeItem.GetComponent<Image>().sprite = transform.GetChild(0).GetComponent<Image>().sprite;

		if(bChangeIconSpriteOnDrag && emptyIconSprite!=null)
		{
			bSelectable = false;
			transform.GetChild(0).GetComponent<Image>() .sprite = emptyIconSprite;
		}

		activeItem.name = transform.name;
		activeItem.SetParent(activeItemHolder);
		activeItem.position = transform.position;
		activeItem.localScale = Vector3.one;
		if(imageSize != Vector2.zero) activeItem.GetComponent<RectTransform>().sizeDelta = imageSize;


		dragOffset =  Camera.main.ScreenToWorldPoint(Input.mousePosition) - activeItem.position; 
		dragOffset = new Vector3(dragOffset.x,dragOffset.y,0);
 
		// InvokeRepeating("TestDistance",0, .2f);
		if(  SoundManager.Instance!=null)  	SoundManager.Instance.StopAndPlay_Sound( SoundManager.Instance.ButtonClick2);

		Tutorial.Instance.PauseTutorial("TopMenu " + activeMenu);

		if(TopMovementLimit == null) TopMovementLimit = GameObject.Find("TopMovementLimit");

		if(TestTopMovementLimit != null && TopMovementLimit!=null)  GameObject.DestroyImmediate(TestTopMovementLimit);

		if(TestTopMovementLimit == null && TopMovementLimit!=null) 
		{
			TestTopMovementLimit = new GameObject("TestTopMovementLimit");
			TestTopMovementLimit.transform.SetParent(activeItem);
			//TestTopMovementLimit.transform.position = new Vector3(ActiveItem.transform.position.x,TopMovementLimit.transform.position.y,0);
			TestTopMovementLimit.name = "TTML";
			if(imageSize != Vector2.zero) TestTopMovementLimit.transform.localPosition = new Vector3(0, imageSize.y/2f, 0);
			else 
			{
				Vector2 ImageSize2 = activeItem.GetComponent<RectTransform>().sizeDelta;
				TestTopMovementLimit.transform.localPosition = new Vector3(0, ImageSize2.y/2f, 0);
			}
		}
	 
	}
	
	public void  OnDrag(PointerEventData eventData)
	{

	}

	public void OnEndDrag(PointerEventData eventData)
	{
		//return;
		if (!bEnableDrag || bLocked) return;
		if (bDrag && !bSnapToTarget)
		{
			StartCoroutine(nameof(TestEndDrag));
		}
	}

	private IEnumerator SnapToTarget()
	{
		if(!bSnapToTarget  ) 
		{
			bSnapToTarget = true;
		 
			bDrag = false;

			float timeMove = 0;
 
			Vector3 endPos = targetPoint[targetPointIndex].position ;
		
			while  (timeMove  <1 )
			{
				yield return new WaitForFixedUpdate();
				activeItem.position = Vector3.Lerp( activeItem.position , endPos , timeMove *2)  ;
 
				timeMove += Time.fixedDeltaTime;
 
			}
		}
		yield return new WaitForFixedUpdate();
	}
	
	private IEnumerator TestEndDrag(  )
	{
		bDrag = false;
		 
		yield return new WaitForFixedUpdate( );

		if(!bSnapToTarget )
		{
			if( (bUseSnapDistance && ( Vector2.Distance((activeItem.position + testPointOffset),targetPoint[0].position) < snapDistance  )) 
			    || ( !bUseSnapDistance && (startPosition.y - activeItem.position.y) > disposeItemDistance ))
			{
				bSnapToTarget = true;
				bDrag = false;
			 
				int closestPoint= 0;
				float distance = 100;
				float distance2 = 0;

				for(int i= 0;i<targetPoint.Length; i++)
				{
					distance2 = Vector2.Distance(activeItem.position,targetPoint[i].position);
					if(distance2 < distance )
					{
						closestPoint = i;
						distance = distance2;
					}
				}

				if(activeMenu == 1) 
				{
					StartCoroutine(nameof(FadeFlower));
				}
 				

				if( Application.loadedLevelName == "Minigame 2")
				{ 
				 	 if( SoundManager.Instance!=null)  SoundManager.Instance.Play_Sound( SoundManager.Instance.DropInWater,.2f);
				}

				float timeMove = 0;
				
				Vector3 endPos= targetPoint[closestPoint].position;
				while  (timeMove  <1 )
				{
					yield return new WaitForFixedUpdate();
					activeItem.position = Vector3.Lerp( activeItem.position , endPos , timeMove )  ;
					timeMove += Time.fixedDeltaTime*12;
				}
				activeItem.position =  endPos ;

				if(bChangeIconSpriteOnDrag)
				{
				 
					if(_selectedRecord!=null && _selectedRecordSprite != null)
					{
						_selectedRecord.sprite = _selectedRecordSprite;
						_selectedRecord.transform.parent.GetComponent<TopMenuItem>().bSelectable = true;
					}

					_selectedRecord =  transform.GetChild(0).GetComponent<Image>() ;
					_selectedRecordSprite = activeItem.GetComponent<Image>().sprite;

					 Camera.main.SendMessage("PlayRecord" , transform.name, SendMessageOptions.DontRequireReceiver);
				}

				int cc=targetPoint[closestPoint].childCount;
				for(int j = 0; j < cc; j++)  { GameObject.Destroy( targetPoint[closestPoint].GetChild(j).gameObject ); }

				activeItem.SetParent(targetPoint[closestPoint]); ;
				 
				if( Application.loadedLevelName == "Minigame 1"     )  
				{
					if( MiniGame1.CompletedActionNo == (activeMenu+1) ) Camera.main.SendMessage("CompletedAction");
					if(  SoundManager.Instance!=null)  	SoundManager.Instance.StopAndPlay_Sound( SoundManager.Instance.ButtonClick2);
				}

				else if( Application.loadedLevelName == "Minigame 2")
				{
					Camera.main.SendMessage("NextPhase","GiveToy");
					 //if( SoundManager.Instance!=null)  SoundManager.Instance.Play_Sound( SoundManager.Instance.DropInWater);
				}

				else if( Application.loadedLevelName == "Minigame 3" && activeItem.parent.name == "Cucla")
				{
					Camera.main.SendMessage("NextPhase","GivePacifier");
					if(  SoundManager.Instance!=null)  	SoundManager.Instance.StopAndPlay_Sound( SoundManager.Instance.ButtonClick2);
				}
				else  if( Application.loadedLevelName == "Minigame 3" && activeItem.parent.name != "Cucla")
				{
					Camera.main.SendMessage("NextPhase","GiveToy");
					if(  SoundManager.Instance!=null)  	SoundManager.Instance.StopAndPlay_Sound( SoundManager.Instance.ButtonClick2);
				}

				if( Application.loadedLevelName == "Minigame 4"     )  
				{
					if( Minigame4.CompletedActionNo == 1 ) Camera.main.SendMessage("CompletedAction");
					if(  SoundManager.Instance!=null)  	SoundManager.Instance.StopAndPlay_Sound( SoundManager.Instance.ButtonClick2);
				}

			}
			else
			{
				if(bChangeIconSpriteOnDrag && emptyIconSprite!=null)
				{
					transform.GetChild(0).GetComponent<Image>() .sprite  =  activeItem.GetComponent<Image>().sprite; 
					bSelectable = true;
				}

				CanvasGroup cg= activeItem.GetComponent<CanvasGroup>();
				while(cg.alpha >0.05f)
				{
					yield return new WaitForFixedUpdate( );
					cg.alpha -=Time.fixedTime*2;
				}

				Destroy(activeItem.gameObject);
			}
		}
	}

	private IEnumerator FadeFlower()
	{
		yield return new WaitForFixedUpdate();
		Image Flower =	GameObject.Find("Flower").GetComponent<Image>();
		if(Flower.color.a > 0)
		{
			float a = Flower.color.a;
			while(Flower.color.a >0)
			{
				a-=Time.fixedDeltaTime*4;
				Flower.color = new Color(1,1,1,a);
				yield return new WaitForFixedUpdate();
			}
			Flower.color = new Color(1,1,1,0);
		}
		
	}

	public void OnPointerClick (PointerEventData eventData)
	{
		if( bLocked ) 
		{
            AdsManager.AdsManager.Instance.videoRewarded.RemoveAllListeners();
            AdsManager.AdsManager.Instance.videoRewarded.AddListener(delegate()
                {
                    TopMenuItem item=this;
                    item.Unlock();
                });
            AdsManager.AdsManager.Instance.ShowVideoReward();
		}
	}
	
	private void OnApplicationFocus( bool hasFocus )
	{
		if(  !appFocus && hasFocus )
		{
			if(!bSnapToTarget &&  bDrag )
			{
				StopAllCoroutines();
				StartCoroutine(nameof(TestEndDrag) );
			}
		}
		appFocus = hasFocus;
		
	}
 

}
