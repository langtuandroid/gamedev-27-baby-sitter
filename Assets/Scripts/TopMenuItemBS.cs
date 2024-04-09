using UnityEngine;
using System.Collections;
using TemplateScripts;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;


public class TopMenuItemBS : MonoBehaviour,  IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
	private static TopMenuItemBS Instance { set; get; }
	private bool bLockedD = false;

	private bool bDragG = false;
	private readonly bool bEnableDragG = true;
	 
	private bool bSnapToTargetT = false;
	
	private float x;
	private float y;
	
	private Vector3 dragOffsetT = new Vector3(0,0,0);
	
	private Vector3 startPositionN;
	private Transform activeItemM;
	[FormerlySerializedAs("ActiveItemHolder")] public Transform activeItemHolder;
	private readonly int mouseFollowSpeedD = 10; 
	
	[FormerlySerializedAs("TargetPoint")] public Transform[] targetPoint;
	
	public GameObject prefabDragItem;
	
	private readonly float disposeItemDistanceE = 5f;
	
	private readonly int targetPointIndexX = 0;

	[FormerlySerializedAs("dragSprite")] public Sprite dragSpriteE;
	[FormerlySerializedAs("bChangeSpriteOnDrag")] public bool bChangeSpriteOnDragG = false;
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
		startPositionN = transform.position;
		//SetLockImage();
	}
	 

	public void SetLockImageE(  )
	{
		bLockedD = MenuItemsBS.mitdD[transform.name].Locked;
		transform.GetChild(1).gameObject.SetActive(bLockedD);
	}

	public void UnlockK()
	{
		MenuItemsBS.mitdD[transform.name].Locked = false;
		bLockedD = false;
		transform.GetChild(1).gameObject.SetActive(false);
		GameDataBS.SaveUnlockedItemsToPpP(transform.name.Remove(0,2));
	}
 
	private void Update () 
	{
		if( bDragG  && bEnableDragG)  
		{
			x = Input.mousePosition.x;
			y = Input.mousePosition.y;
			
			Vector3 posM = Camera.main.ScreenToWorldPoint(new Vector3(x ,y,10.0f) );

			if(TestTopMovementLimit!=null && TopMovementLimit!=null)
			{
				float diffTL = TestTopMovementLimit.transform.position.y-activeItemM.position.y;
				if(posM.y+ diffTL>TopMovementLimit.transform.position.y) posM = new Vector3(posM.x, TopMovementLimit.transform.position.y - diffTL ,posM.z);
			}
			activeItemM.position =  Vector3.Lerp (activeItemM.position, posM  , mouseFollowSpeedD * Time.deltaTime)  ;
		}
	}
	
	public void OnBeginDrag (PointerEventData eventData)
	{

		if(  !bEnableDragG || !bSelectable || bLockedD) return;

		if( Application.loadedLevelName == "Minigame 1"  && MiniGame1BS.CompletedActionNoN < activeMenu+1) return;



		bSnapToTargetT = false;
		bDragG = true;

		activeItemM = GameObject.Instantiate(prefabDragItem).transform;

		if(bChangeSpriteOnDragG && dragSpriteE !=null ) activeItemM.GetComponent<Image>().sprite = dragSpriteE;
		else 	activeItemM.GetComponent<Image>().sprite = transform.GetChild(0).GetComponent<Image>().sprite;

		if(bChangeIconSpriteOnDrag && emptyIconSprite!=null)
		{
			bSelectable = false;
			transform.GetChild(0).GetComponent<Image>() .sprite = emptyIconSprite;
		}

		activeItemM.name = transform.name;
		activeItemM.SetParent(activeItemHolder);
		activeItemM.position = transform.position;
		activeItemM.localScale = Vector3.one;
		if(imageSize != Vector2.zero) activeItemM.GetComponent<RectTransform>().sizeDelta = imageSize;


		dragOffsetT =  Camera.main.ScreenToWorldPoint(Input.mousePosition) - activeItemM.position; 
		dragOffsetT = new Vector3(dragOffsetT.x,dragOffsetT.y,0);
 
		// InvokeRepeating("TestDistance",0, .2f);
		if(  SoundManagerBS.Instance!=null)  	SoundManagerBS.Instance.StopAndPlay_Sound( SoundManagerBS.Instance.ButtonClick2);

		TutorialBS.Instance.PauseTutorialL("TopMenu " + activeMenu);

		if(TopMovementLimit == null) TopMovementLimit = GameObject.Find("TopMovementLimit");

		if(TestTopMovementLimit != null && TopMovementLimit!=null)  GameObject.DestroyImmediate(TestTopMovementLimit);

		if(TestTopMovementLimit == null && TopMovementLimit!=null) 
		{
			TestTopMovementLimit = new GameObject("TestTopMovementLimit");
			TestTopMovementLimit.transform.SetParent(activeItemM);
			//TestTopMovementLimit.transform.position = new Vector3(ActiveItem.transform.position.x,TopMovementLimit.transform.position.y,0);
			TestTopMovementLimit.name = "TTML";
			if(imageSize != Vector2.zero) TestTopMovementLimit.transform.localPosition = new Vector3(0, imageSize.y/2f, 0);
			else 
			{
				Vector2 ImageSize2 = activeItemM.GetComponent<RectTransform>().sizeDelta;
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
		if (!bEnableDragG || bLockedD) return;
		if (bDragG && !bSnapToTargetT)
		{
			StartCoroutine(nameof(TestEndDrag));
		}
	}

	private IEnumerator SnapToTarget()
	{
		if(!bSnapToTargetT  ) 
		{
			bSnapToTargetT = true;
		 
			bDragG = false;

			float timeMove = 0;
 
			Vector3 endPos = targetPoint[targetPointIndexX].position ;
		
			while  (timeMove  <1 )
			{
				yield return new WaitForFixedUpdate();
				activeItemM.position = Vector3.Lerp( activeItemM.position , endPos , timeMove *2)  ;
 
				timeMove += Time.fixedDeltaTime;
 
			}
		}
		yield return new WaitForFixedUpdate();
	}
	
	private IEnumerator TestEndDrag(  )
	{
		bDragG = false;
		 
		yield return new WaitForFixedUpdate( );

		if(!bSnapToTargetT )
		{
			if( (bUseSnapDistance && ( Vector2.Distance((activeItemM.position + testPointOffset),targetPoint[0].position) < snapDistance  )) 
			    || ( !bUseSnapDistance && (startPositionN.y - activeItemM.position.y) > disposeItemDistanceE ))
			{
				bSnapToTargetT = true;
				bDragG = false;
			 
				int closestPoint= 0;
				float distance = 100;
				float distance2 = 0;

				for(int i= 0;i<targetPoint.Length; i++)
				{
					distance2 = Vector2.Distance(activeItemM.position,targetPoint[i].position);
					if(distance2 < distance )
					{
						closestPoint = i;
						distance = distance2;
					}
				}

				if(activeMenu == 1) 
				{
					StartCoroutine(nameof(FadeFlowerR));
				}
 				

				if( Application.loadedLevelName == "Minigame 2")
				{ 
				 	 if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.PlaySound( SoundManagerBS.Instance.DropInWaterR,.2f);
				}

				float timeMove = 0;
				
				Vector3 endPos= targetPoint[closestPoint].position;
				while  (timeMove  <1 )
				{
					yield return new WaitForFixedUpdate();
					activeItemM.position = Vector3.Lerp( activeItemM.position , endPos , timeMove )  ;
					timeMove += Time.fixedDeltaTime*12;
				}
				activeItemM.position =  endPos ;

				if(bChangeIconSpriteOnDrag)
				{
				 
					if(_selectedRecord!=null && _selectedRecordSprite != null)
					{
						_selectedRecord.sprite = _selectedRecordSprite;
						_selectedRecord.transform.parent.GetComponent<TopMenuItemBS>().bSelectable = true;
					}

					_selectedRecord =  transform.GetChild(0).GetComponent<Image>() ;
					_selectedRecordSprite = activeItemM.GetComponent<Image>().sprite;

					 Camera.main.SendMessage("PlayRecordD" , transform.name, SendMessageOptions.DontRequireReceiver);
				}

				int cc=targetPoint[closestPoint].childCount;
				for(int j = 0; j < cc; j++)  { GameObject.Destroy( targetPoint[closestPoint].GetChild(j).gameObject ); }

				activeItemM.SetParent(targetPoint[closestPoint]); ;
				 
				if( Application.loadedLevelName == "Minigame 1"     )  
				{
					if( MiniGame1BS.CompletedActionNoN == (activeMenu+1) ) Camera.main.SendMessage("CompletedActionN");
					if(  SoundManagerBS.Instance!=null)  	SoundManagerBS.Instance.StopAndPlay_Sound( SoundManagerBS.Instance.ButtonClick2);
				}

				else if( Application.loadedLevelName == "Minigame 2")
				{
					Camera.main.SendMessage("NextPhaseE","GiveToy");
					 //if( SoundManager.Instance!=null)  SoundManager.Instance.Play_Sound( SoundManager.Instance.DropInWater);
				}

				else if( Application.loadedLevelName == "Minigame 3" && activeItemM.parent.name == "Cucla")
				{
					Camera.main.SendMessage("NextPhaseE","GivePacifier");
					if(  SoundManagerBS.Instance!=null)  	SoundManagerBS.Instance.StopAndPlay_Sound( SoundManagerBS.Instance.ButtonClick2);
				}
				else  if( Application.loadedLevelName == "Minigame 3" && activeItemM.parent.name != "Cucla")
				{
					Camera.main.SendMessage("NextPhaseE","GiveToy");
					if(  SoundManagerBS.Instance!=null)  	SoundManagerBS.Instance.StopAndPlay_Sound( SoundManagerBS.Instance.ButtonClick2);
				}

				if( Application.loadedLevelName == "Minigame 4"     )  
				{
					if( Minigame4BS.CompletedActionNoN == 1 ) Camera.main.SendMessage("CompletedActionN");
					if(  SoundManagerBS.Instance!=null)  	SoundManagerBS.Instance.StopAndPlay_Sound( SoundManagerBS.Instance.ButtonClick2);
				}

			}
			else
			{
				if(bChangeIconSpriteOnDrag && emptyIconSprite!=null)
				{
					transform.GetChild(0).GetComponent<Image>() .sprite  =  activeItemM.GetComponent<Image>().sprite; 
					bSelectable = true;
				}

				CanvasGroup cg= activeItemM.GetComponent<CanvasGroup>();
				while(cg.alpha >0.05f)
				{
					yield return new WaitForFixedUpdate( );
					cg.alpha -=Time.fixedTime*2;
				}

				Destroy(activeItemM.gameObject);
			}
		}
	}

	private IEnumerator FadeFlowerR()
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
		if( bLockedD ) 
		{
            AdsManager.AdsManagerBS.Instance.videoRewarded.RemoveAllListeners();
            AdsManager.AdsManagerBS.Instance.videoRewarded.AddListener(delegate()
                {
                    TopMenuItemBS itemBs=this;
                    itemBs.UnlockK();
                });
            AdsManager.AdsManagerBS.Instance.ShowVideoReward();
		}
	}
	
	private void OnApplicationFocus( bool hasFocus )
	{
		if(  !appFocus && hasFocus )
		{
			if(!bSnapToTargetT &&  bDragG )
			{
				StopAllCoroutines();
				StartCoroutine(nameof(TestEndDrag) );
			}
		}
		appFocus = hasFocus;
		
	}
 

}
