using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class MoveToyBS : MonoBehaviour , IBeginDragHandler, IDragHandler, IEndDragHandler
{
	private Vector3 startPositionN ;
	private bool bIsKoriSceneE = false;
	private bool bDragG = false;
	private float x;
	private float y;
	private Vector3 diffPosS = new Vector3(0,0,0);
	private readonly float testDistanceE =  1f;

	private bool bMovingBackK = false;
	
	public int ShelfIndex = 0;

	private Transform parentOldD;
	private Transform dragItemParentT;
	
	[FormerlySerializedAs("TestPoint")] [SerializeField] private Transform testPoint;
	public Transform[] TargetPoint;
	
	private int targetPointIndexX = -1;
	private bool appFocusS = true;
	 
	private Image shadowW;
	
	[FormerlySerializedAs("TopMovementLimit")] [SerializeField] private GameObject topMovementLimit;
	private IEnumerator Start () 
	{
		testPoint = transform;
		shadowW = transform.GetComponent<Image>();
		yield return new WaitForSeconds(0.1f);
		
		dragItemParentT = GameObject.Find("ActiveItemHolder").transform;
		
		startPositionN  = transform.position;
		
		parentOldD = transform.parent;
		
		bIsKoriSceneE = false;
	}
	
	private void Update()
	{ 
		if( !bIsKoriSceneE &&  bDragG )
		{
			
			x = Input.mousePosition.x;
			y = Input.mousePosition.y;
			
			Vector3 posM = Camera.main.ScreenToWorldPoint(new Vector3(x ,y,10f) ) + diffPosS;
			if( topMovementLimit!=null && posM.y >topMovementLimit.transform.position.y) posM = new Vector3(posM.x, topMovementLimit.transform.position.y  ,posM.z);
			transform.position =  Vector3.Lerp (transform.position  , posM  , 10 * Time.deltaTime)   ;
		}
	}

	private void TestTargetT()
	{ 
 
		int closestPoint= -1;
		float distance = 1000;
		float distance2 = 0;
		
		for(int i= 0;i<TargetPoint.Length; i++)
		{
			if(TargetPoint[i].childCount == 0)
			{
				distance2 = Vector2.Distance(testPoint.position,TargetPoint[i].position);
				if(distance2< testDistanceE &&  distance2 < distance )  
				{
					closestPoint = i;
					distance = distance2;
				}
			}
		}
		targetPointIndexX = closestPoint;
		
		if(targetPointIndexX >-1)
		{
			StartCoroutine(nameof(SnapToTargetT),TargetPoint[targetPointIndexX]);
		}
		else 
		{
			StartCoroutine(nameof(MoveBack) );
		}
	}

	public void OnBeginDrag (PointerEventData eventData)
	{
		 
		if(bMovingBackK) return;
		 
		 
		if(  !bIsKoriSceneE   && !bDragG  )
		{
			if(topMovementLimit == null) topMovementLimit = GameObject.Find("TopMovementLimit");
		 
			bDragG = true;
			startPositionN = transform.position;
			diffPosS =transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition)   ;
			diffPosS = new Vector3(diffPosS.x,diffPosS.y,0);
			dragItemParentT.position = transform.parent .position;
			transform.SetParent(dragItemParentT);
			 
			TutorialBS.Instance.StopTutor();
		}
	}

	public void OnDrag (PointerEventData eventData)
	{
		
	}
	
	public void OnEndDrag (PointerEventData eventData)
	{
		
		if(  !bIsKoriSceneE &&  bDragG 	)  
		{
			
			bDragG = false;
		 	TestTargetT();
 
		}
	}
	
	private IEnumerator SnapToTargetT( Transform target)
	{
		 
		bDragG = false;
		bIsKoriSceneE = true;
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
		if(!bMovingBackK)
		{
 
			bMovingBackK = true;
			
			
			yield return new WaitForEndOfFrame( );
			
			float pom = 0;
			Vector3 positionS = transform.position;
			while(pom<1 )
			{ 
				pom+=Time.fixedDeltaTime*2;
				transform.position = Vector3.Lerp(positionS, startPositionN,pom);
			 
				
				yield return new WaitForFixedUpdate( );
			}
			
			transform.SetParent(parentOldD);
			transform.position = startPositionN;
 
			bMovingBackK = false;
			 
		}
		
	}
	
	private void OnApplicationFocus( bool hasFocus )
	{
		if(  !appFocusS && hasFocus )
		{
			if(  !bIsKoriSceneE &&  bDragG )
			{
				bDragG = false;
				
				CancelInvoke(nameof(TestTargetT));
				StartCoroutine(nameof(MoveBack) );
			}
		}
		appFocusS = hasFocus;
		
	}

	

}
