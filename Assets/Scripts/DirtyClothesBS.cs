using UnityEngine;
using System.Collections;
using TemplateScripts;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class DirtyClothesBS : MonoBehaviour,  IBeginDragHandler, IDragHandler, IEndDragHandler
{
	private bool bFinishedD;
	private bool bSnapToPositionN;
	private bool bDragG;
	[FormerlySerializedAs("bEnableDrag")] public bool bEnableDragG = true;
	private bool bMovingBackK;
	private bool bSnapToTargetT;

	private float x;
	private float y;
	private Vector3 dragOffsetT = new Vector3(0,0,0);
	
	private Vector3 startPositionN;
	private Transform startParentT;
	[SerializeField] private Transform activeItemHolder;
	private readonly int mouseFollowSpeedD = 10; 

	[FormerlySerializedAs("TargetPoint")] [SerializeField] private Transform targetPoint;
	[FormerlySerializedAs("TestPoint")] [SerializeField] private Transform testPoint;
	
	private readonly float snapDistanceE = 1f;

	[FormerlySerializedAs("DirtyItemNo")] [SerializeField] private int dirtyItemNo;
	[FormerlySerializedAs("Flower")] [SerializeField] private Image flower;

	[FormerlySerializedAs("TestTopMovementLimit")] [SerializeField] private Transform testTopMovementLimit;
	[FormerlySerializedAs("TopMovementLimit")] [SerializeField] private Transform topMovementLimit;
	
	private bool appFocusS = true;

	private void Start () {
		startPositionN = transform.position;
		startParentT = transform.parent;

	}
	
	private void Update () 
	{
		if( Application.loadedLevelName == "Minigame 1" &&   MiniGame1BS.CompletedActionNoN != dirtyItemNo) return;
		if( bDragG  && bEnableDragG)  
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


			transform.position =  Vector3.Lerp (transform.position, posM  , mouseFollowSpeedD * Time.deltaTime)  ;
		}
	}
	
	public void OnBeginDrag (PointerEventData eventData)
	{

		if( bFinishedD || !bEnableDragG) return;
		 
		bSnapToPositionN = false;
		bDragG = true;
		 
		
		dragOffsetT =  Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position; 
		dragOffsetT = new Vector3(dragOffsetT.x,dragOffsetT.y,0);
 
		transform.SetParent(activeItemHolder);
		InvokeRepeating(nameof(TestDistanceE),0, .1f);
		if(flower != null) 
		{
			StartCoroutine(nameof(ShowFlowerR));
		}
		if(  SoundManagerBS.Instance!=null)  	SoundManagerBS.Instance.StopAndPlay_Sound( SoundManagerBS.Instance.ButtonClick2);


		TutorialBS.Instance.StopTutor();
	}

	private IEnumerator ShowFlowerR()
	{
		StopCoroutine(nameof(HideFlowerR));
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


	private IEnumerator HideFlowerR()
	{
		StopCoroutine(nameof(ShowFlowerR));
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
		if( Application.loadedLevelName == "Minigame 1" &&    MiniGame1BS.CompletedActionNoN != dirtyItemNo) return;
		if(  bDragG &&   !bSnapToTargetT ) 
		{
			StartCoroutine(nameof(MoveBackK) );
			CancelInvoke(nameof(TestDistanceE));
		}
	}
	
	private void TestDistanceE()
	{
		if(Vector2.Distance(testPoint.position,targetPoint.position)<snapDistanceE)
		{
			StartCoroutine(nameof(SnapToParentT));
			bDragG = false;
		}
		 
	}
	
	private IEnumerator SnapToParentT()
	{
		if(!bMovingBackK && !bFinishedD ) 
		{
			bSnapToTargetT = true;
			bFinishedD = false;
			bDragG = false;
			bEnableDragG = false;
			 
			CancelInvoke(nameof(TestDistanceE));
		 
			float timeMove = 0;

			targetPoint.parent.GetComponent<Animator>().Play("Basket",-1,0);
			if(  SoundManagerBS.Instance!=null)  	SoundManagerBS.Instance.StopAndPlay_Sound( SoundManagerBS.Instance.ButtonClick2);
			while  (timeMove  <1 )
			{
				yield return new WaitForFixedUpdate();
				transform.position = Vector3.Lerp( transform.position , targetPoint.position , timeMove *2)  ;
 
				transform.localScale  = Vector3.Lerp(transform.localScale, Vector3.zero,timeMove);
				timeMove += Time.fixedDeltaTime;

			}

			Camera.main.SendMessage("DirtyClothesInBasket",dirtyItemNo);
			GameObject.Destroy(gameObject,.5f);
		}
		yield return new WaitForFixedUpdate();
	}
	
	private IEnumerator MoveBackK(  )
	{
		bDragG = false;
		bEnableDragG = false;
		yield return new WaitForFixedUpdate( );
		if(!bMovingBackK && !bSnapToTargetT)
		{
		 
			bMovingBackK = true;
			yield return new WaitForSeconds(.1f );
			float dist = Vector3.Distance( transform.position, startPositionN);			 
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
					transform.position = Vector3.Lerp(currentPosition, startPositionN,pom);
					yield return new WaitForFixedUpdate( );
				}
			}
			
			bMovingBackK = false;
			
			transform.SetParent(startParentT);
			transform.position = startPositionN;
			
			if((Application.loadedLevelName == "Minigame 1" &&  dirtyItemNo == 1) || Application.loadedLevelName == "Minigame 4"   ) StartCoroutine(nameof(HideFlowerR));
		}
 
		bEnableDragG = true;
		//Tutorial.bPause = false;	 
	}
	
	
	private void OnApplicationFocus( bool hasFocus )
	{
		if(  !appFocusS && hasFocus )
		{
			if(  bDragG )
			{
				bDragG = false;
				
				CancelInvoke(nameof(TestDistanceE));
				
				StartCoroutine(nameof(MoveBackK) );
			}
		}
		appFocusS = hasFocus;
		
	}
 

}
