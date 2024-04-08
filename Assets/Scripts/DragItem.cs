using UnityEngine;
using System.Collections;
using TemplateScripts;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	private Vector3 startPosition ;
	private Vector3 startScale;

	[FormerlySerializedAs("EndScaleFactor")] [SerializeField] public float endScaleFactor;
 
	public bool snapToTarget = false;
	private bool bIsKoriScene = false;
	
	[SerializeField] private bool bDrag = false;
	
	public static int OneItemEnabledNo = -1; //-1 dozvoljeni svi, 0 zabranjeni svi, 1,2,3.. dozvoljen samo odgovarajuci item
	[SerializeField] private int ItemNo = 0;
	
	private float x;
	private float y;
	private Vector3 diffPos = new Vector3(0,0,0);
	
	[SerializeField] private float testDistance =  2.5f;//1; // .25f;
	
	private ParticleSystem psFinishAction;
	private PointerEventData pointerEventData;
 
	[SerializeField] private Animator animator; 
	private bool bAnimationActive = false;
	[SerializeField] private string animationType = "";
 
	private Transform parentOld;
	private Transform dragItemParent;

	[FormerlySerializedAs("TestPoint")] [SerializeField] private Transform testPoint;
	[FormerlySerializedAs("TargetPoint")] [SerializeField] private Transform[] targetPoint;
	private int targetPointIndex = -1;

	[SerializeField] private bool bTestOnlyOnEndDrag = true;
	[FormerlySerializedAs("TopMovementLimit")] [SerializeField] private GameObject topMovementLimit;
	
	[SerializeField] private bool bMovingBack = false;

	private bool appFocus = true;
	
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(0.1f);
		
		dragItemParent = GameObject.Find("ActiveItemHolder").transform;
		
		startPosition  = transform.position;
		
		testPoint = transform.Find("TestPoint");
		parentOld = transform.parent;
		
		bIsKoriScene = false;
		
 
	}
	
	private void Update()
	{ 
		if(bDrag)
		{
			x = Input.mousePosition.x;
			y = Input.mousePosition.y;
			
			Vector3 posM = Camera.main.ScreenToWorldPoint(new Vector3(x ,y,10f) ) + diffPos;
			if( topMovementLimit!=null && posM.y >topMovementLimit.transform.position.y) posM = new Vector3(posM.x, topMovementLimit.transform.position.y  ,posM.z);
			transform.position =  Vector3.Lerp (transform.position  , posM  , 10 * Time.deltaTime)   ;
		}
	}


	private void TestTarget()
	{ 
		if(bAnimationActive || bIsKoriScene) return;

		int closestPoint= -1;
		float distance = 1000;
		float distance2 = 0;
		 
		for(int i= 0;i<targetPoint.Length; i++)
		{
			distance2 = Vector2.Distance(testPoint.position,targetPoint[i].position);
			if(distance2< testDistance &&  distance2 < distance )//&& TargetPoint[i].childCount==0)
			{
				closestPoint = i;
				distance = distance2;
			}
		}
		targetPointIndex = closestPoint;
		 
		if(targetPointIndex >-1)
		{
			StartCoroutine(nameof(SnapToTarget),targetPoint[targetPointIndex]);
		}
		else if( bTestOnlyOnEndDrag) 
		{
			StartCoroutine(nameof(MoveBack) );
		}

	}

	private IEnumerator SnapToTarget( Transform target)
	{
		OneItemEnabledNo = 0;
		bDrag = false;
		CancelInvoke(nameof(TestTarget));
 
		yield return new WaitForEndOfFrame();
		if(animationType =="BlenderTop" && animator != null) 
		{
			bIsKoriScene = true;
		 	float pom = 0;
			Vector3 sPos = transform.position;
			while(pom<1)
			{
				pom+=Time.fixedDeltaTime* 2;
				transform.position = Vector3.Lerp(sPos, target.position,pom);
				yield return new WaitForFixedUpdate();
			}

			animator.enabled = true;
			animator.Play("PourMash",-1,0);
			yield return new WaitForSeconds(0.1f);
			Camera.main.SendMessage("NextPhase", "PourMash", SendMessageOptions.DontRequireReceiver);
			yield return new WaitForSeconds(1.3f);
			animator.enabled = false;
			StartCoroutine(nameof(MoveBack));
 

		}
 
		else if(animationType =="FruitBlender" && animator != null) 
		{
			if( SoundManager.Instance!=null)  SoundManager.Instance.Play_Sound( SoundManager.Instance.ShowItem);
			transform.SetParent(target);
			float pomX = target.position.x -transform.position.x;
			float pomY = target.position.y - transform.position.y;
			Vector3 pos;
			float startX   = transform.position.x;
			float startY   = transform.position.y;
			float speedY =  pomY/pomX;
			float dX = pomX;

			float dist = Vector2.Distance(transform.position,target.position);
		  
			while  (dist  >0.2f && dist <20 )
			{
				yield return new WaitForFixedUpdate();
		
				pos = transform.position;
				dX  = (pos.x - startX);
				
				pos.x +=pomX*Time.deltaTime*2;
				pos.y  =   startY  +  dX*  speedY  +      2*   (target.position.x -transform.position.x)/pomX *dX /pomX  ;
				
				transform.position = pos;
				transform.Rotate( new Vector3(0,0, 180*Time.deltaTime ));
				dist = Vector2.Distance(transform.position,target.position);
				transform.localScale = Vector3.one * Mathf.Lerp(transform.localScale.x, endScaleFactor,Time.fixedDeltaTime*3);
			}
			
			animator.Play("closeCover",-1,0);
			float pom = 0;
			Vector3 sPos = transform.position;
			Vector3 ePos = target.position - new Vector3(0,1,0);

			while(pom<1)
			{
				pom+=Time.fixedDeltaTime* 4;
				transform.position = Vector3.Lerp(sPos, ePos, pom);
				yield return new WaitForFixedUpdate();
			}
			OneItemEnabledNo = -1;
			Camera.main.SendMessage("NextPhase", "FruitBlender", SendMessageOptions.DontRequireReceiver);
			yield return new WaitForSeconds(.2F);
			Camera.main.SendMessage("ShowChoppedFruitBlender", ItemNo );
			transform.GetChild(0).gameObject.SetActive(false);
			Destroy(transform.gameObject,1);
		}
		else if(animationType =="Milk" && animator != null) 
		{
			bIsKoriScene = true;
			float pom = 0;
			Vector3 sPos = transform.position;
			while(pom<1)
			{
				pom+=Time.fixedDeltaTime* 2;
				transform.position = Vector3.Lerp(sPos, target.position,pom);
				yield return new WaitForFixedUpdate();
			}
			
			animator.enabled = true;
			animator.Play("InsertMilk",-1,0);
			yield return new WaitForSeconds(0.1f);
			Camera.main.SendMessage("NextPhase", "InsertMilk", SendMessageOptions.DontRequireReceiver);
			yield return new WaitForSeconds(1.3f);
			//animator.enabled = false;
			StartCoroutine(nameof(MoveBack));

			
		}

		else if(animationType =="Cereal" && animator != null) 
		{
			bIsKoriScene = true;
			float pom = 0;
			Vector3 sPos = transform.position;
			while(pom<1)
			{
				pom+=Time.fixedDeltaTime* 2;
				transform.position = Vector3.Lerp(sPos, target.position,pom);
				yield return new WaitForFixedUpdate();
			}
			
			animator.enabled = true;
			animator.Play("InsertCereal",-1,0);
			yield return new WaitForSeconds(0.1f);
			Camera.main.SendMessage("NextPhase", "InsertCereal", SendMessageOptions.DontRequireReceiver);
			yield return new WaitForSeconds(1.3f);
			//animator.enabled = false;
			StartCoroutine(nameof(MoveBack));
			
			
		}

		else if(animationType =="FruitBowl" && animator != null) 
		{
			bIsKoriScene = true;
			float pom = 0;
			Vector3 sPos = transform.position;
			while(pom<1)
			{
				pom+=Time.fixedDeltaTime* 2;
				transform.position = Vector3.Lerp(sPos, target.position,pom);
				yield return new WaitForFixedUpdate();
			}
			
			animator.enabled = true;
			animator.Play("InsertFruits",-1,0);
			yield return new WaitForSeconds(0.1f);
			Camera.main.SendMessage("NextPhase", "InsertFruits", SendMessageOptions.DontRequireReceiver);
			yield return new WaitForSeconds(1.3f);
			//animator.enabled = false;
			StartCoroutine(nameof(MoveBack));
			
			
		}

		else if(animationType =="TissueClean" && animator != null) 
		{

			bIsKoriScene = true;
			float pom = 0;
			Vector3 sPos = transform.position;
			while(pom<1)
			{
				pom+=Time.fixedDeltaTime* 2;
				transform.position = Vector3.Lerp(sPos, target.position,pom);
				yield return new WaitForFixedUpdate();
			}
			
			Destroy( GameObject.Find ("CleanWithTissue"));
			animator.Play("TissueClean",-1,0);
			yield return new WaitForSeconds(1.3f);
			Camera.main.SendMessage("NextPhase", "CleanBaby", SendMessageOptions.DontRequireReceiver);

			 
		 
		}
	}

	public void OnBeginDrag (PointerEventData eventData)
	{
		if(   bIsKoriScene ) return;
	
		if(bMovingBack) return;

		if(topMovementLimit == null) topMovementLimit = GameObject.Find("TopMovementLimit");

		StopAllCoroutines(); 
		pointerEventData = eventData;
		bAnimationActive = false;
		if(OneItemEnabledNo >-1 && ItemNo != OneItemEnabledNo)
		{
			bDrag = false;
			return;
		}
		
		if(  !bIsKoriScene   && !bDrag  )
		{
			bDrag = true;
			startPosition = transform.position;
			diffPos =transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition)   ;
			diffPos = new Vector3(diffPos.x,diffPos.y,0);
			dragItemParent.position = transform.parent .position;
			transform.SetParent(dragItemParent);

			if(!bTestOnlyOnEndDrag) InvokeRepeating("TestTarget",0f, .1f);
			


			if(animationType =="FruitBlender" && animator != null)
			{
				animator.Play("openCover",-1,0);
				Tutorial.Instance.StopTutorial();
			}
			if(animationType =="BlenderTop")
			{
				Tutorial.Instance.StopTutorial();
			}
			if(animationType =="Milk" || animationType =="Cereal" || animationType =="FruitBowl" || animationType =="TissueClean")
			{
				Tutorial.Instance.StopTutorial();
			}
		 
			 

		}
	}
	
	public void OnDrag (PointerEventData eventData)
	{
		
	}
	
	public void OnEndDrag (PointerEventData eventData)
	{
		 
		if(    bDrag 	)  
		{
			 
			bDrag = false;
			if(bTestOnlyOnEndDrag &&  !bIsKoriScene) TestTarget();
			else
			{
				CancelInvoke(nameof(TestTarget));
				StartCoroutine(nameof(MoveBack) );
			}
		 
		}
	}
	
	private IEnumerator MoveBack(  )
	{
		if(!bMovingBack)
		{
			if(animationType =="FruitBlender" && animator != null) animator.Play("closeCover",-1,0);

			bMovingBack = true;
			
			 
			yield return new WaitForEndOfFrame( );
		 
			float pom = 0;
			Vector3 positionS = transform.position;
			while(pom<1 )
			{ 
				pom+=Time.fixedDeltaTime*2;
				transform.position = Vector3.Lerp(positionS, startPosition,pom);

				yield return new WaitForFixedUpdate( );
			}
			
			transform.SetParent(parentOld);
			transform.position = startPosition;
			
		 
			
			//activeToolNo = 0;
			bMovingBack = false;
			if(bIsKoriScene) 
			{
			}
		}
 
	}
	
	public void StartMoveBack()
	{
		CancelInvoke(nameof(TestTarget));
		StartCoroutine(nameof(MoveBack) );
		
	}
	
	private void OnApplicationFocus( bool hasFocus )
	{
		if(  !appFocus && hasFocus )
		{
			if(  !bIsKoriScene &&  bDrag )
			{
				bDrag = false;
				
				CancelInvoke(nameof(TestTarget));
				StartCoroutine(nameof(MoveBack) );
			}
		}
		appFocus = hasFocus;
		
	}

 

}
