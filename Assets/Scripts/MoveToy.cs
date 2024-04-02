using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class MoveToy : MonoBehaviour , IBeginDragHandler, IDragHandler, IEndDragHandler
{
	private Vector3 startPosition ;
	private bool bIsKoriScene = false;
	private bool bDrag = false;
	private float x;
	private float y;
	private Vector3 diffPos = new Vector3(0,0,0);
	private readonly float testDistance =  1f;

	private bool bMovingBack = false;
	
	public int ShelfIndex = 0;

	private Transform parentOld;
	private Transform dragItemParent;
	
	[FormerlySerializedAs("TestPoint")] [SerializeField] private Transform testPoint;
	public Transform[] TargetPoint;
	
	private int targetPointIndex = -1;
	
	private bool appFocus = true;
	 
	private Image shadow;
	[FormerlySerializedAs("TopMovementLimit")] [SerializeField] private GameObject topMovementLimit;
	private IEnumerator Start () 
	{
		testPoint = transform;
		shadow = transform.GetComponent<Image>();
		yield return new WaitForSeconds(0.1f);
		
		dragItemParent = GameObject.Find("ActiveItemHolder").transform;
		
		startPosition  = transform.position;
		
		parentOld = transform.parent;
		
		bIsKoriScene = false;
	}
	
	private void Update()
	{ 
		if( !bIsKoriScene &&  bDrag )
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
 
		int closestPoint= -1;
		float distance = 1000;
		float distance2 = 0;
		
		for(int i= 0;i<TargetPoint.Length; i++)
		{
			if(TargetPoint[i].childCount == 0)
			{
				distance2 = Vector2.Distance(testPoint.position,TargetPoint[i].position);
				if(distance2< testDistance &&  distance2 < distance )  
				{
					closestPoint = i;
					distance = distance2;
				}
			}
		}
		targetPointIndex = closestPoint;
		
		if(targetPointIndex >-1)
		{
			StartCoroutine(nameof(SnapToTarget),TargetPoint[targetPointIndex]);
		}
		else 
		{
			StartCoroutine(nameof(MoveBack) );
		}
	}

	public void OnBeginDrag (PointerEventData eventData)
	{
		 
		if(bMovingBack) return;
		 
		 
		if(  !bIsKoriScene   && !bDrag  )
		{
			if(topMovementLimit == null) topMovementLimit = GameObject.Find("TopMovementLimit");
		 
			bDrag = true;
			startPosition = transform.position;
			diffPos =transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition)   ;
			diffPos = new Vector3(diffPos.x,diffPos.y,0);
			dragItemParent.position = transform.parent .position;
			transform.SetParent(dragItemParent);
			 
			Tutorial.Instance.StopTutorial();
		}
	}
	
	
	public void OnDrag (PointerEventData eventData)
	{
		
	}
	
	public void OnEndDrag (PointerEventData eventData)
	{
		
		if(  !bIsKoriScene &&  bDrag 	)  
		{
			
			bDrag = false;
		 	TestTarget();
 
		}
	}
	
	private IEnumerator SnapToTarget( Transform target)
	{
		 
		bDrag = false;
		bIsKoriScene = true;
		float pom = 0;
		Vector3 sPos = transform.position;
		while(pom<1)
		{
			pom+=Time.fixedDeltaTime* 4;
			transform.position = Vector3.Lerp(sPos, target.position,pom);
			yield return new WaitForFixedUpdate();
		}

		transform.SetParent(target);
		transform.localPosition = Vector3.zero;
 
		Camera.main.SendMessage("NextPhase",  SendMessageOptions.DontRequireReceiver);
		yield return new WaitForFixedUpdate();
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
			 
				
				yield return new WaitForFixedUpdate( );
			}
			
			transform.SetParent(parentOld);
			transform.position = startPosition;
 
			bMovingBack = false;
			 
		}
		
	}
	
	void OnApplicationFocus( bool hasFocus )
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
