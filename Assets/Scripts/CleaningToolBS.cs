using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TemplateScripts;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class CleaningToolBS : MonoBehaviour  , IBeginDragHandler, IDragHandler, IEndDragHandler
{
	[FormerlySerializedAs("toolType")] [SerializeField] private ToolType toolTypeE;
	
	[SerializeField] private Vector3 startPosition ;
	[SerializeField] private bool bIsKoriscen = false;
	[HideInInspector()]
	
	[SerializeField] private bool bDrag = false;
	 
	public static int OneToolEnabledNoN = 0;
	 
	[SerializeField] private float x;
	[SerializeField] private float y;
	
	[SerializeField] private Vector3 diffPos = new Vector3(0,0,0);
	[SerializeField] private float testDistance = .2f;// .5f;//1;// .25f;

	[SerializeField] private int toolNo = 0;
	 
	public static int ActiveToolNo = 0;
	public static GameObject ItemBeingDragged;
	
	public static bool BCleaningG = false;

	private readonly Vector3 offsetPosS = Vector3.zero;
	private Transform trPomM;

	private Transform testPointT;

	[SerializeField] private ToolBehavior toolBehavior; 
	private ParticleSystem psFinishCleaningAll;

	private PointerEventData pointerEventData;
	[SerializeField] private Quaternion startRotation;

	[SerializeField] private Animator animationChild; 
	
	private Transform parentOldD;
	private Transform dragItemParentT;
 
	[SerializeField] private GameObject BubblesPref;
	
	private int bubblesCountT;
	[FormerlySerializedAs("AssociatedTool")] [SerializeField] private CleaningToolBS associatedToolBs;

	[SerializeField] private bool snapToTarget = false;
	[FormerlySerializedAs("ShampooCover")] [SerializeField] private GameObject shampooCover;
	[FormerlySerializedAs("ShampooDrop")] [SerializeField] private GameObject shampooDrop;

	private readonly int cyclesToFinishH = 5;
	private int cyclesS = 0;

	private BubblesListHolder bubblesAnimHolderR;
	[FormerlySerializedAs("TowelImage")] [SerializeField] private Image towelImage;

	private GameObject topMovementLimitT;
	
	private bool bMovingBackK = false;

	public void Awake()
	{
		OneToolEnabledNoN = 0;  
		BCleaningG = false;
		bDrag = false;
		ActiveToolNo = 0;
	}

	private IEnumerator Start()
	{
		yield return new WaitForSeconds(0.1f);
		 
		dragItemParentT = GameObject.Find("ActiveItemHolder").transform;
 
		startPosition  = transform.position;
		 
		testPointT = transform.Find("TestPoint");
		parentOldD = transform.parent;
	 
		bIsKoriscen = false;

		if( toolTypeE == ToolType.soap  ) bubblesAnimHolderR = Camera.main.GetComponent<BubblesListHolder>();
	}
	 
	private void Update()
	{ 
		if(  bDrag )
		{
			x = Input.mousePosition.x;
			y = Input.mousePosition.y;

			Vector3 posM = Camera.main.ScreenToWorldPoint(new Vector3(x ,y,10f) ) - offsetPosS;
			if( topMovementLimitT!=null && posM.y >topMovementLimitT.transform.position.y) posM = new Vector3(posM.x, topMovementLimitT.transform.position.y  ,posM.z);
			transform.position =  Vector3.Lerp (transform.position, posM  , 10 * Time.deltaTime)  ;
		}
	}
	
	private void CreateBubbles()
	{
		if(toolBehavior == ToolBehavior.AnimateOnlyWhenMovingOverObject && !pointerEventData.IsPointerMoving()) return;
		Collider2D[] hitColliders = Physics2D.OverlapCircleAll(testPointT.position, testDistance  , 1 << LayerMask.NameToLayer("Tool"+toolNo.ToString()+"Interact")); //layermask to filter the varius colliders
		if(hitColliders.Length > 0  )
		{

			if( SoundManagerBS.Instance!=null) 
			{
				SoundManagerBS.Instance.BubbleE.pitch = Random.Range(.9f, 1.7f);
				SoundManagerBS.Instance.StopAndPlay_Sound( SoundManagerBS.Instance.BubbleE); 
			}
			bubblesCountT++;
			GameObject bp = GameObject.Instantiate(BubblesPref);
			bp.transform.SetParent ( hitColliders[0].transform.parent);//.parent.parent );
			bp.transform.position = transform.position;
			bubblesAnimHolderR.bubblesAnimM.Add(bp.GetComponent<Animator>() );

			if(bubblesCountT >= 50 )
			{
				CancelInvoke(nameof(CreateBubbles));
				ToolCleaningFinished();
			}
		}	
	}
 
	private void TestClean()
	{ 
		if(BCleaningG) return;
		if(toolBehavior == ToolBehavior.AnimateOnlyWhenMovingOverObject && !pointerEventData.IsPointerMoving()) return;
		Collider2D[] hitColliders;
	  
	 	hitColliders = Physics2D.OverlapCircleAll(testPointT.position, testDistance  , 1 << LayerMask.NameToLayer("Tool"+toolNo.ToString()+"Interact")); //layermask to filter the varius colliders
		if(hitColliders.Length > 0  )
		{
			trPomM = null;
			for (int i =0 ; i<hitColliders.Length; i++)    
			{
				trPomM = hitColliders[i].transform;
				BCleaningG = true;
				break;
			}


			if( toolTypeE == ToolType.soap  )
			{
				animationChild.SetTrigger("tUpDownClean");
				TutorialBS.Instance.StopTutor();
			}
			else if(toolTypeE == ToolType.shampoo  )
			{ 
				if(bDrag)
				{
					bDrag = false;
					animationChild.SetTrigger("tShampooBottle");
					StartCoroutine("SnapToTargetT",trPomM.transform);
					TutorialBS.Instance.StopTutor();
				}
			}
			if( toolTypeE == ToolType.towel  )
			{
				animationChild.SetTrigger("tUpDownClean");  
				 if( SoundManagerBS.Instance!=null )  SoundManagerBS.Instance.StopAndPlay_Sound(SoundManagerBS.Instance.TowelL);
				TutorialBS.Instance.StopTutor();
			}
			if( toolTypeE == ToolType.BathtubPlug  )
			{
				animationChild.SetTrigger("tPlug");
				TutorialBS.Instance.StopTutor();
			}
		}
		 
	}


	private void ToolCleaningFinished()
	{
		if(!bIsKoriscen)
		{
			string gamePhaseState = "";
			if(toolTypeE == ToolType.soap) gamePhaseState = "Soap";
			else if(toolTypeE == ToolType.shampoo) gamePhaseState = "Shampoo";
			else if(toolTypeE == ToolType.BathtubPlug) gamePhaseState = "BathtubPlug";
			else if(toolTypeE == ToolType.towel) gamePhaseState = "Towel";

			Camera.main.SendMessage("NextPhaseE", gamePhaseState, SendMessageOptions.DontRequireReceiver);

	 
			bIsKoriscen = true;
			bDrag = false;
			 

			if(!bMovingBackK)
			{
				StopAllCoroutines();
				StartMoveBackK();
			}

		}
	}


	public void ShampooBottle_Drop()
	{
		shampooDrop.SetActive(true);
	 
		shampooDrop.GetComponent<Animator>().Play("ShampooDrop",-1,0);
	}

	public void ShampooBottle_MoveBack()
	{
		BCleaningG = false;
		ToolCleaningFinished();
	}

	public void BathTubgPlug_MoveBack()
	{
		BCleaningG = false;
		ToolCleaningFinished();
	}

	 
	private IEnumerator SnapToTargetT( Transform target)
	{
		OneToolEnabledNoN = 0;
		bDrag = false;
		CancelInvoke(nameof(TestClean));
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

 	
	public void CleaningAnimation_Finished()
	{
		BCleaningG = false;
		cyclesS++;
		if( toolTypeE == ToolType.towel  && cyclesS ==(cyclesToFinishH-1)) 
		{
			Camera.main.SendMessage("HideWaterDropsTowelL",  SendMessageOptions.DontRequireReceiver);
		}
		if( toolTypeE != ToolType.soap  && cyclesS ==cyclesToFinishH) 
		{
			ToolCleaningFinished();
		}
		if( toolTypeE == ToolType.soap  )
		{ 
			//SoundManager.Instance.Stop_Sound(SoundManager.Instance.SoapSound);
		}
		else if( toolTypeE == ToolType.towel ) 
		{
			//SoundManager.Instance.Stop_Sound(SoundManager.Instance.SpraySound);
		}
 	
	}
	
	public void OnBeginDrag (PointerEventData eventData)
	{
		if(bMovingBackK) return;
		StopAllCoroutines(); 

		if(topMovementLimitT == null) topMovementLimitT = GameObject.Find("TopMovementLimit");

		pointerEventData = eventData;
		BCleaningG = false;
		if(OneToolEnabledNoN >-1 && toolNo != OneToolEnabledNoN)
		{
			bDrag = false;
			return;
		}
		
		if(  !bIsKoriscen   && !bDrag  )
		{
			if( toolTypeE != ToolType.BathtubPlug  ) transform.localScale = 1.4f*Vector3.one;
			animationChild.transform.parent.rotation = Quaternion.Euler(0,0,0);
			ActiveToolNo = toolNo;

			bDrag = true;
			diffPos =transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition)   ;
			diffPos = new Vector3(diffPos.x,diffPos.y,0);
			
			InvokeRepeating(nameof(TestClean),0f, .1f);
			
			transform.SetParent(dragItemParentT);
			if( toolTypeE == ToolType.soap  )
			{
				InvokeRepeating(nameof(CreateBubbles),0f, .1f);
			}
			else if( toolTypeE == ToolType.shampoo  )
			{
				shampooCover.SetActive(false);
			}

			else if( toolTypeE == ToolType.towel  )
			{
				towelImage.enabled = true;
				towelImage.transform.parent.GetComponent<Image>().enabled = false;
			}

			TutorialBS.Instance.StopTutor();
		}
	}

	
	public void OnDrag (PointerEventData eventData)
	{
		 
	}
 
	public void OnEndDrag (PointerEventData eventData)
	{
		if(toolTypeE == ToolType.soap) CancelInvoke(nameof(CreateBubbles));
		if(  !bIsKoriscen &&  bDrag 	)  
		{
			bDrag = false;
			CancelInvoke(nameof(TestClean));
			StartCoroutine(nameof(MoveBackK) );
			if(toolTypeE == ToolType.spray ) 
			{
				animationChild.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
			}
		}
	}
	
	private IEnumerator MoveBackK(  )
	{
		if(!bMovingBackK)
		{
			bMovingBackK = true;
			
			yield return new WaitForEndOfFrame( );

			float pom = 0;
			Vector3 positionS = transform.position;
			while(pom<1 )
			{ 
				pom+=Time.fixedDeltaTime*2;
				transform.position = Vector3.Lerp(positionS, startPosition,pom);
				if( toolTypeE != ToolType.BathtubPlug  ) 
				{
					if(transform.localScale.x >  1) transform.localScale =  (1.4f -  pom)*Vector3.one;
					else transform.localScale =  Vector3.one;
				}
				yield return new WaitForFixedUpdate( );
			}
		 
			transform.SetParent(parentOldD);
			transform.position = startPosition;

			if( toolTypeE == ToolType.shampoo  )
			{
				shampooCover.SetActive(true);
			}
			else if( toolTypeE == ToolType.towel  )
			{
				towelImage.enabled = false;
				towelImage.transform.parent.GetComponent<Image>().enabled = true;
			}
			
			bMovingBackK = false;
			if(bIsKoriscen) 
			{
				
			}
		}
	}

	public void StartMoveBackK()
	{
		CancelInvoke(nameof(TestClean));
		StartCoroutine(nameof(MoveBackK) );
	
	}

	private bool appFocus = true;
	
	private void OnApplicationFocus( bool hasFocus )
	{
		if(  !appFocus && hasFocus )
		{
			if(  !bIsKoriscen &&  bDrag )
			{
				bDrag = false;
				
				CancelInvoke(nameof(TestClean));
				StartCoroutine(nameof(MoveBackK) );
			}
		}
		appFocus = hasFocus;
		
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
	BathtubPlug,

	spray
}
