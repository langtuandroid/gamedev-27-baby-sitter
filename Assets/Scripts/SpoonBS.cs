using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using TemplateScripts;
using UnityEngine.Serialization;

public class SpoonBS : MonoBehaviour , IBeginDragHandler, IDragHandler, IEndDragHandler
{
	private Vector3 startPositionN ;

	private bool bIsKoriSceneE = false;
	[HideInInspector()]
	[SerializeField] private bool bDrag = false;

	private float x;
	private float y;
	private Vector3 diffPosS = new Vector3(0,0,0);
	private readonly float testDistanceE =  .25f; 
	  
	private int feedCountT = 0;
	
	private ParticleSystem psFinishActionN;
	private PointerEventData pointerEventDataA;
	 
	private Transform parentOldD;
	private Transform dragItemParentT;
	
	[FormerlySerializedAs("TestPoint")] [SerializeField] private Transform testPoint;
	[FormerlySerializedAs("TargetPoint")] [SerializeField] private Transform[] targetPoint;
	
	private SpoonTestPoint spoonTestPointT = SpoonTestPoint.Bowl;

	[FormerlySerializedAs("FoodSpoon")] [SerializeField] private Image foodSpoon;
	[FormerlySerializedAs("FoodBowl")] [SerializeField] private Transform foodBowl;
	[FormerlySerializedAs("FoodBowlStartPos")] [SerializeField] private Transform foodBowlStartPos;
	[FormerlySerializedAs("FoodBowlEndPos")] [SerializeField] private Transform foodBowlEndPos;

	public bool bMixingFood = false;
	[SerializeField] private bool bMixingFoodStarted = false;
	[SerializeField] private float timeMixingFood = 0;

	[FormerlySerializedAs("MixingFood")] [SerializeField] private Image mixingFood ;
	[FormerlySerializedAs("CleanWithTissue")] [SerializeField] private Image cleanWithTissue; 
	public BabyControllerBs babyC;

	[FormerlySerializedAs("TopMovementLimit")] [SerializeField] private GameObject topMovementLimit;
	
	private float mixValL = 1f;	
	private float timeNotMoveE = 0;
	
	private bool bMovingBackK = false;
	
	private bool bFixAnimationMoveE = false;
	private Vector3 endAnimPosS = Vector3.zero;

	private void Awake()
	{
		if(cleanWithTissue!=null) cleanWithTissue.color = new Color(1,1,1,0);
	}

	private IEnumerator Start()
	{

		yield return new WaitForSeconds(0.1f);
		
		dragItemParentT = GameObject.Find("ActiveItemHolder").transform;
		
		startPositionN  = transform.position;
		
		testPoint = transform.Find("TestPoint");
		parentOldD = transform.parent;
		
		bIsKoriSceneE = false;
	}
	
	private void Update()
	{ 
		if( bDrag )
		{
			
			x = Input.mousePosition.x;
			y = Input.mousePosition.y;
			
			Vector3 posM = Camera.main.ScreenToWorldPoint(new Vector3(x ,y,10f) ) ;//+ diffPos;
			if( topMovementLimit!=null && posM.y >topMovementLimit.transform.position.y) posM = new Vector3(posM.x, topMovementLimit.transform.position.y  ,posM.z);
			transform.position =  Vector3.Lerp (transform.position  , posM  , 10 * Time.deltaTime)   ;
		}

		if(bMixingFoodStarted ) 
		{
			if(bMixingFood)
			{
				if(!pointerEventDataA.IsPointerMoving() && timeNotMoveE >.5f )
				{

					bMixingFoodStarted = false;
					if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.StopSound( SoundManagerBS.Instance.MixingFoodD);
					return;
				}

			}

			timeMixingFood +=Time.deltaTime;
			timeNotMoveE+=Time.deltaTime;
			if( timeMixingFood>mixValL )
			{
				if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.PlaySound( SoundManagerBS.Instance.MixingFoodD);
				mixValL +=1f;
				mixingFood.transform.localScale = new Vector3(-mixingFood.transform.localScale.x, mixingFood.transform.localScale.y, mixingFood.transform.localScale.z);
				 
			}

			if(timeMixingFood >5)
			{
				bMixingFoodStarted = false;
				//InvokeRepeating("TestTarget",0f, .1f);
				bDrag = false;
				 StartCoroutine(nameof(MoveBackK));
				Camera.main.SendMessage("NextPhaseE", "MixedFood", SendMessageOptions.DontRequireReceiver);
				if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.StopSound( SoundManagerBS.Instance.MixingFoodD);
			}
			 
		}
	}

	private void TestTargetT()
	{ 
		if(spoonTestPointT == SpoonTestPoint.Bowl)
		{
			if( Vector2.Distance(testPoint.position,targetPoint[0].position) < testDistanceE)
		   	{
			    spoonTestPointT  = SpoonTestPoint.Mouth;
				foodSpoon.enabled = true;

				foodBowl.position = Vector3.Lerp(foodBowlStartPos.position,foodBowlEndPos.position,(feedCountT+1)/5f);
				foodBowl.localScale = Vector3.one * (.8f + .2f - (feedCountT+1)/25f);
				if(feedCountT == 4) foodBowl.gameObject.SetActive(false);
			}
		}
		else if(spoonTestPointT == SpoonTestPoint.Mouth)
		{
			 
			if( Vector2.Distance(testPoint.position,targetPoint[1].position) < testDistanceE)
			{
				foodSpoon.enabled = false;
				feedCountT++;
				spoonTestPointT = SpoonTestPoint.Bowl;
				
				if(cleanWithTissue!=null) cleanWithTissue.color = new Color(1,1,1, .2f*feedCountT);
				babyC.BabyEat();
				if(feedCountT == 5)
				{
					endAnimPosS= transform.position;
					bFixAnimationMoveE = true;
					Camera.main.SendMessage("NextPhaseE", "FeedBaby", SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}
	private void LateUpdate()
	{
		if(bFixAnimationMoveE)
		{
			transform.position = endAnimPosS;
		}
	}
 
		
	public void OnBeginDrag (PointerEventData eventData)
	{
		if(bMovingBackK) return;
		if(topMovementLimit == null) topMovementLimit = GameObject.Find("TopMovementLimit");
		StopAllCoroutines(); 
		pointerEventDataA = eventData;

		if(  !bIsKoriSceneE   && !bDrag  )
		{
			bDrag = true;
			startPositionN = transform.position;
			dragItemParentT.position = transform.parent.position;
			transform.SetParent(dragItemParentT);
			
			if(!bMixingFood)	
			{
				InvokeRepeating("TestTargetT",0f, .1f);
				 babyC.BabyWaitToEat();
			}
			
		}

		TutorialBS.Instance.PauseTutorialL("spoon");
	}
	
	
	public void OnDrag (PointerEventData eventData)
	{
		if(bMixingFood)
		{
			if(eventData.IsPointerMoving() && ( Vector2.Distance(testPoint.position,targetPoint[0].position) < .8f) )
			{
				bMixingFoodStarted = true;
				timeNotMoveE = 0;
			}
		}
	}
	
	public void OnEndDrag (PointerEventData eventData)
	{
		
		if(  !bIsKoriSceneE &&  bDrag 	)  
		{
			bMixingFoodStarted = false;
			if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.StopSound( SoundManagerBS.Instance.MixingFoodD);
			bDrag = false;
			StartCoroutine(nameof(MoveBackK) );
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
				transform.position = Vector3.Lerp(positionS, startPositionN,pom);

				yield return new WaitForFixedUpdate( );
			}
			
			transform.SetParent(parentOldD);
			transform.position = startPositionN;

			bMovingBackK = false;
			if(bIsKoriSceneE) 
			{
			}
		}
		
	}
	
	public void StartMoveBackK()
	{
		CancelInvoke(nameof(TestTargetT));
		StartCoroutine(nameof(MoveBackK) );
	}
	
	
 
	private bool appFocus = true;
	
	private void OnApplicationFocus( bool hasFocus )
	{
		if(  !appFocus && hasFocus )
		{
			if(  !bIsKoriSceneE &&  bDrag )
			{
				bDrag = false;
				
				CancelInvoke(nameof(TestTargetT));
				StartCoroutine(nameof(MoveBackK) );
			}
		}
		appFocus = hasFocus;	
	}
 
		
	
	enum SpoonTestPoint
	{
		Bowl,
		Mouth
	}

}
