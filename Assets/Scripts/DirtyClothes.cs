using UnityEngine;
using System.Collections;
using TemplateScripts;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class DirtyClothes : MonoBehaviour,  IBeginDragHandler, IDragHandler, IEndDragHandler
{
	private bool bFinished;
	private bool bSnapToPosition;
	private bool bDrag;
	public bool bEnableDrag = true;
	private bool bMovingBack;
	private bool bSnapToTarget;

	private float x;
	private float y;
	private Vector3 dragOffset = new Vector3(0,0,0);
	
	private Vector3 startPosition;
	private Transform startParent;
	[SerializeField] private Transform activeItemHolder;
	private readonly int mouseFollowSpeed = 10; 

	[SerializeField] private Transform TargetPoint;
	[SerializeField] private Transform TestPoint;
	
	private readonly float snapDistance = 1f;

	[FormerlySerializedAs("DirtyItemNo")] [SerializeField] private int dirtyItemNo;
	[FormerlySerializedAs("Flower")] [SerializeField] private Image flower;

	[FormerlySerializedAs("TestTopMovementLimit")] [SerializeField] private Transform testTopMovementLimit;
	[FormerlySerializedAs("TopMovementLimit")] [SerializeField] private Transform topMovementLimit;
	
	private bool appFocus = true;

	private void Start () {
		startPosition = transform.position;
		startParent = transform.parent;

	}
	
	private void Update () 
	{
		if( Application.loadedLevelName == "Minigame 1" &&   MiniGame1.CompletedActionNo != dirtyItemNo) return;
		if( bDrag  && bEnableDrag)  
		{
			x = Input.mousePosition.x;
			y = Input.mousePosition.y;
			
			Vector3 posM = Camera.main.ScreenToWorldPoint(new Vector3(x ,y,10.0f) );

			if(testTopMovementLimit!=null && topMovementLimit!=null)
			{
				float diffTL = testTopMovementLimit.position.y-transform.position.y;
				if(posM.y+ diffTL>topMovementLimit.position.y) posM = new Vector3(posM.x, topMovementLimit.position.y - diffTL ,posM.z);
			}


			transform.position =  Vector3.Lerp (transform.position  , posM  , 10 * Time.deltaTime)   ;


			transform.position =  Vector3.Lerp (transform.position, posM  , mouseFollowSpeed * Time.deltaTime)  ;
		}
	}
	
	public void OnBeginDrag (PointerEventData eventData)
	{

		if( bFinished || !bEnableDrag) return;
		 
		bSnapToPosition = false;
		bDrag = true;
		 
		
		dragOffset =  Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position; 
		dragOffset = new Vector3(dragOffset.x,dragOffset.y,0);
 
		transform.SetParent(activeItemHolder);
		InvokeRepeating(nameof(TestDistance),0, .1f);
		if(flower != null) 
		{
			StartCoroutine(nameof(ShowFlower));
		}
		if(  SoundManager.Instance!=null)  	SoundManager.Instance.StopAndPlay_Sound( SoundManager.Instance.ButtonClick2);


		Tutorial.Instance.StopTutorial();
	}

	private IEnumerator ShowFlower()
	{
		StopCoroutine(nameof(HideFlower));
		yield return new WaitForEndOfFrame();
		if(flower!=null)
		{
			float a = flower.color.a;
			while(flower.color.a <1)
			{
				a+=Time.fixedDeltaTime*4;
				flower.color = new Color(1,1,1,a);
				yield return new WaitForFixedUpdate();
			}
			flower.color = Color.white;
		}
	}


	private IEnumerator HideFlower()
	{
		StopCoroutine(nameof(ShowFlower));
		yield return new WaitForEndOfFrame();
		if(flower!=null)
		{
			float a = flower.color.a;
			while(flower.color.a >0)
			{
				a-=Time.fixedDeltaTime*4;
				flower.color = new Color(1,1,1,a);
				yield return new WaitForFixedUpdate();
			}
			flower.color = new Color(1,1,1,0);
		}
	}


 
	public void  OnDrag(PointerEventData eventData)
	{
	 
		
	}
	
	public void  OnEndDrag(PointerEventData eventData)
	{
		if( Application.loadedLevelName == "Minigame 1" &&    MiniGame1.CompletedActionNo != dirtyItemNo) return;
		if(  bDrag &&   !bSnapToTarget ) 
		{
			StartCoroutine(nameof(MoveBack) );
			CancelInvoke(nameof(TestDistance));
		}
	}

 
 
	private void TestDistance()
	{
		 
			if(Vector2.Distance(TestPoint.position,TargetPoint.position)<snapDistance)
			{
					StartCoroutine(nameof(SnapToParent));
					bDrag = false;
			}
		 
	}
	
	private IEnumerator SnapToParent()
	{
		if(!bMovingBack && !bFinished ) 
		{
			bSnapToTarget = true;
			bFinished = false;
			bDrag = false;
			bEnableDrag = false;
			 
			CancelInvoke(nameof(TestDistance));
		 
			float timeMove = 0;

			TargetPoint.parent.GetComponent<Animator>().Play("Basket",-1,0);
			if(  SoundManager.Instance!=null)  	SoundManager.Instance.StopAndPlay_Sound( SoundManager.Instance.ButtonClick2);
			while  (timeMove  <1 )
			{
				yield return new WaitForFixedUpdate();
				transform.position = Vector3.Lerp( transform.position , TargetPoint.position , timeMove *2)  ;
 
				transform.localScale  = Vector3.Lerp(transform.localScale, Vector3.zero,timeMove);
				timeMove += Time.fixedDeltaTime;

			}

			Camera.main.SendMessage("DirtyClothesInBasket",dirtyItemNo);
			GameObject.Destroy(gameObject,.5f);
		}
		yield return new WaitForFixedUpdate();
	}
	
	private IEnumerator MoveBack(  )
	{
		
		bDrag = false;
		bEnableDrag = false;
		yield return new WaitForFixedUpdate( );
		if(!bMovingBack && !bSnapToTarget)
		{
		 
			bMovingBack = true;
			yield return new WaitForSeconds(.1f );
			float dist = Vector3.Distance( transform.position, startPosition);			 
			Vector3 currentPosition = transform.position;
			
			if(dist>0.05f)
			{
				//Debug.Log("MoveBack");
				float timeCoef =  10f/dist;
				
				yield return new WaitForEndOfFrame( );
				float pom = 0;
				while(pom<1 )
				{ 
					pom+=Time.fixedDeltaTime*timeCoef;
					transform.position = Vector3.Lerp(currentPosition, startPosition,pom);
					yield return new WaitForFixedUpdate( );
				}
			}
			
		 
		 
			bMovingBack = false;
			
			transform.SetParent(startParent);
			transform.position = startPosition;
			
			if((Application.loadedLevelName == "Minigame 1" &&  dirtyItemNo == 1) || Application.loadedLevelName == "Minigame 4"   ) StartCoroutine(nameof(HideFlower));
		}
 
		bEnableDrag = true;
		//Tutorial.bPause = false;	 
	}
	
	
	private void OnApplicationFocus( bool hasFocus )
	{
		if(  !appFocus && hasFocus )
		{
			if(  bDrag )
			{
				bDrag = false;
				
				CancelInvoke(nameof(TestDistance));
				
				StartCoroutine(nameof(MoveBack) );
			}
		}
		appFocus = hasFocus;
		
	}
 

}
