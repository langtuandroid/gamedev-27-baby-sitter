using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TemplateScripts;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class CleaningTool : MonoBehaviour  , IBeginDragHandler, IDragHandler, IEndDragHandler
{
	[SerializeField] private ToolType toolType;
	
	[SerializeField] private Vector3 startPosition ;
	[SerializeField] private bool bIsKoriscen = false;
	[HideInInspector()]
	
	[SerializeField] private bool bDrag = false;
	 
	public static int OneToolEnabledNo = 0;
	 
	[SerializeField] private float x;
	[SerializeField] private float y;
	
	[SerializeField] private Vector3 diffPos = new Vector3(0,0,0);
	[SerializeField] private float testDistance = .2f;// .5f;//1;// .25f;

	[SerializeField] private int toolNo = 0;
	 
	public static int ActiveToolNo = 0;
	public static GameObject ItemBeingDragged;
	
	public static bool BCleaning = false;

	private readonly Vector3 offsetPos = Vector3.zero;
	private Transform trPom;

	private Transform testPoint;

	[SerializeField] private ToolBehavior toolBehavior; 
	private ParticleSystem psFinishCleaningAll;

	private PointerEventData pointerEventData;
	[SerializeField] private Quaternion startRotation;

	[SerializeField] private Animator animationChild; 
	
	private Transform parentOld;
	private Transform dragItemParent;
 
	[SerializeField] private GameObject BubblesPref;
	
	private int bubblesCount;
	[SerializeField] private CleaningTool AssociatedTool;

	[SerializeField] private bool snapToTarget = false;
	[FormerlySerializedAs("ShampooCover")] [SerializeField] private GameObject shampooCover;
	[FormerlySerializedAs("ShampooDrop")] [SerializeField] private GameObject shampooDrop;

	private readonly int cyclesToFinish = 5;
	private int cycles = 0;

	private BubblesListHolder bubblesAnimHolder;
	[FormerlySerializedAs("TowelImage")] [SerializeField] private Image towelImage;

	private GameObject topMovementLimit;
	
	private bool bMovingBack = false;

	public void Awake()
	{
		OneToolEnabledNo = 0;  
		BCleaning = false;
		bDrag = false;
		ActiveToolNo = 0;
	}

	private IEnumerator Start()
	{
		yield return new WaitForSeconds(0.1f);
		 
		dragItemParent = GameObject.Find("ActiveItemHolder").transform;
 
		startPosition  = transform.position;
		 
		testPoint = transform.Find("TestPoint");
		parentOld = transform.parent;
	 
		bIsKoriscen = false;

		if( toolType == ToolType.soap  ) bubblesAnimHolder = Camera.main.GetComponent<BubblesListHolder>();
	}
	 
	private void Update()
	{ 
		if(  bDrag )
		{
			x = Input.mousePosition.x;
			y = Input.mousePosition.y;

			Vector3 posM = Camera.main.ScreenToWorldPoint(new Vector3(x ,y,10f) ) - offsetPos;
			if( topMovementLimit!=null && posM.y >topMovementLimit.transform.position.y) posM = new Vector3(posM.x, topMovementLimit.transform.position.y  ,posM.z);
			transform.position =  Vector3.Lerp (transform.position, posM  , 10 * Time.deltaTime)  ;
		}
	}
	
	private void CreateBubbles()
	{
		if(toolBehavior == ToolBehavior.AnimateOnlyWhenMovingOverObject && !pointerEventData.IsPointerMoving()) return;
		Collider2D[] hitColliders = Physics2D.OverlapCircleAll(testPoint.position, testDistance  , 1 << LayerMask.NameToLayer("Tool"+toolNo.ToString()+"Interact")); //layermask to filter the varius colliders
		if(hitColliders.Length > 0  )
		{

			if( SoundManager.Instance!=null) 
			{
				SoundManager.Instance.Bubble.pitch = Random.Range(.9f, 1.7f);
				SoundManager.Instance.StopAndPlay_Sound( SoundManager.Instance.Bubble); 
			}
			bubblesCount++;
			GameObject bp = GameObject.Instantiate(BubblesPref);
			bp.transform.SetParent ( hitColliders[0].transform.parent);//.parent.parent );
			bp.transform.position = transform.position;
			bubblesAnimHolder.bubblesAnim.Add(bp.GetComponent<Animator>() );

			if(bubblesCount >= 50 )
			{
				CancelInvoke(nameof(CreateBubbles));
				ToolCleaningFinished();
			}
		}	
	}
 
	private void TestClean()
	{ 
		if(BCleaning) return;
		if(toolBehavior == ToolBehavior.AnimateOnlyWhenMovingOverObject && !pointerEventData.IsPointerMoving()) return;
		Collider2D[] hitColliders;
	  
	 	hitColliders = Physics2D.OverlapCircleAll(testPoint.position, testDistance  , 1 << LayerMask.NameToLayer("Tool"+toolNo.ToString()+"Interact")); //layermask to filter the varius colliders
		if(hitColliders.Length > 0  )
		{
			trPom = null;
			for (int i =0 ; i<hitColliders.Length; i++)    
			{
				trPom = hitColliders[i].transform;
				BCleaning = true;
				break;
			}


			if( toolType == ToolType.soap  )
			{
				animationChild.SetTrigger("tUpDownClean");
				Tutorial.Instance.StopTutorial();
			}
			else if(toolType == ToolType.shampoo  )
			{ 
				if(bDrag)
				{
					bDrag = false;
					animationChild.SetTrigger("tShampooBottle");
					StartCoroutine("SnapToTarget",trPom.transform);
					Tutorial.Instance.StopTutorial();
				}
			}
			if( toolType == ToolType.towel  )
			{
				animationChild.SetTrigger("tUpDownClean");  
				 if( SoundManager.Instance!=null )  SoundManager.Instance.StopAndPlay_Sound(SoundManager.Instance.Towel);
				Tutorial.Instance.StopTutorial();
			}
			if( toolType == ToolType.bathtub_plug  )
			{
				animationChild.SetTrigger("tPlug");
				Tutorial.Instance.StopTutorial();
			}
		}
		 
	}


	private void ToolCleaningFinished()
	{
		if(!bIsKoriscen)
		{
			string gamePhaseState = "";
			if(toolType == ToolType.soap) gamePhaseState = "Soap";
			else if(toolType == ToolType.shampoo) gamePhaseState = "Shampoo";
			else if(toolType == ToolType.bathtub_plug) gamePhaseState = "BathtubPlug";
			else if(toolType == ToolType.towel) gamePhaseState = "Towel";

			Camera.main.SendMessage("NextPhase", gamePhaseState, SendMessageOptions.DontRequireReceiver);

	 
			bIsKoriscen = true;
			bDrag = false;
			 

			if(!bMovingBack)
			{
				StopAllCoroutines();
				StartMoveBack();
			}

		}
	}


	public void ShampooBottleDrop()
	{
		shampooDrop.SetActive(true);
	 
		shampooDrop.GetComponent<Animator>().Play("ShampooDrop",-1,0);
	}

	public void ShampooBottleMoveBack()
	{
		BCleaning = false;
		ToolCleaningFinished();
	}

	public void  BathTubgPlugMoveBack()
	{
		BCleaning = false;
		ToolCleaningFinished();
	}

	 
	private IEnumerator SnapToTarget( Transform target)
	{
		OneToolEnabledNo = 0;
		bDrag = false;
		CancelInvoke("TestClean");
		yield return new WaitForEndOfFrame();
		float pom = 0;
		Vector3 sPos = transform.position;
		while(pom<1)
		{
			pom+=Time.fixedDeltaTime* 2;
			transform.position = Vector3.Lerp(sPos, target.position,pom);
			yield return new WaitForFixedUpdate();
		}
	}

 	
	public void CleaningAnimationFinished()
	{
		BCleaning = false;
		cycles++;
		if( toolType == ToolType.towel  && cycles ==(cyclesToFinish-1)) 
		{
			Camera.main.SendMessage("HideWaterDropsTowel",  SendMessageOptions.DontRequireReceiver);
		}
		if( toolType != ToolType.soap  && cycles ==cyclesToFinish) 
		{
			ToolCleaningFinished();
		}
		if( toolType == ToolType.soap  )
		{ 
			//SoundManager.Instance.Stop_Sound(SoundManager.Instance.SoapSound);
		}
		else if( toolType == ToolType.towel ) 
		{
			//SoundManager.Instance.Stop_Sound(SoundManager.Instance.SpraySound);
		}
 	
	}
	
	public void OnBeginDrag (PointerEventData eventData)
	{
		if(bMovingBack) return;
		StopAllCoroutines(); 

		if(topMovementLimit == null) topMovementLimit = GameObject.Find("TopMovementLimit");

		pointerEventData = eventData;
		BCleaning = false;
		if(OneToolEnabledNo >-1 && toolNo != OneToolEnabledNo)
		{
			bDrag = false;
			return;
		}
		
		if(  !bIsKoriscen   && !bDrag  )
		{
			if( toolType != ToolType.bathtub_plug  ) transform.localScale = 1.4f*Vector3.one;
			animationChild.transform.parent.rotation = Quaternion.Euler(0,0,0);
			ActiveToolNo = toolNo;

			bDrag = true;
			diffPos =transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition)   ;
			diffPos = new Vector3(diffPos.x,diffPos.y,0);
			
			InvokeRepeating(nameof(TestClean),0f, .1f);
			
			transform.SetParent(dragItemParent);
			if( toolType == ToolType.soap  )
			{
				InvokeRepeating(nameof(CreateBubbles),0f, .1f);
			}
			else if( toolType == ToolType.shampoo  )
			{
				shampooCover.SetActive(false);
			}

			else if( toolType == ToolType.towel  )
			{
				towelImage.enabled = true;
				towelImage.transform.parent.GetComponent<Image>().enabled = false;
			}

			Tutorial.Instance.StopTutorial();
		}
	}

	
	public void OnDrag (PointerEventData eventData)
	{
		 
	}
 
	public void OnEndDrag (PointerEventData eventData)
	{
		if(toolType == ToolType.soap) CancelInvoke(nameof(CreateBubbles));
		if(  !bIsKoriscen &&  bDrag 	)  
		{
			bDrag = false;
			CancelInvoke(nameof(TestClean));
			StartCoroutine(nameof(MoveBack) );
			if(toolType == ToolType.spray ) 
			{
				animationChild.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
			}
		}
	}
	
	private IEnumerator MoveBack(  )
	{
		if(!bMovingBack)
		{
			bMovingBack = true;
			
			yield return new WaitForEndOfFrame( );

			float pom = 0;
			Vector3 positionS = transform.position;
			while(pom<1 )
			{ 
				pom+=Time.fixedDeltaTime*2;
				transform.position = Vector3.Lerp(positionS, startPosition,pom);
				if( toolType != ToolType.bathtub_plug  ) 
				{
					if(transform.localScale.x >  1) transform.localScale =  (1.4f -  pom)*Vector3.one;
					else transform.localScale =  Vector3.one;
				}
				yield return new WaitForFixedUpdate( );
			}
		 
			transform.SetParent(parentOld);
			transform.position = startPosition;

			if( toolType == ToolType.shampoo  )
			{
				shampooCover.SetActive(true);
			}
			else if( toolType == ToolType.towel  )
			{
				towelImage.enabled = false;
				towelImage.transform.parent.GetComponent<Image>().enabled = true;
			}
			
			bMovingBack = false;
			if(bIsKoriscen) 
			{
				
			}
		}
	}

	public void StartMoveBack()
	{
		CancelInvoke(nameof(TestClean));
		StartCoroutine(nameof(MoveBack) );
	
	}

	bool appFoucs = true;
	private void OnApplicationFocus( bool hasFocus )
	{
		if(  !appFoucs && hasFocus )
		{
			if(  !bIsKoriscen &&  bDrag )
			{
				bDrag = false;
				
				CancelInvoke(nameof(TestClean));
				StartCoroutine(nameof(MoveBack) );
			}
		}
		appFoucs = hasFocus;
		
	}

	 
}

public enum ToolBehavior
{
	AnimateWhenHoveringOverObject,
	AnimateWhenDroppedOnObject,
	AnimateOnlyWhenMovingOverObject

}

public enum ToolType
{
	soap,
	shampoo,
	towel,
	bathtub_plug,

	spray
}
