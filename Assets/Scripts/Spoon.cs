using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using TemplateScripts;
using UnityEngine.Serialization;

public class Spoon : MonoBehaviour , IBeginDragHandler, IDragHandler, IEndDragHandler
{
	private Vector3 startPosition ;

	private bool bIsKoriScene = false;
	[HideInInspector()]
	[SerializeField] private bool bDrag = false;

	private float x;
	private float y;
	private Vector3 diffPos = new Vector3(0,0,0);
	private readonly float testDistance =  .25f; 
	  
	private int feedCount = 0;
	
	private ParticleSystem psFinishAction;
	private PointerEventData pointerEventData;
	 
	private Transform parentOld;
	private Transform dragItemParent;
	
	[FormerlySerializedAs("TestPoint")] [SerializeField] private Transform testPoint;
	[FormerlySerializedAs("TargetPoint")] [SerializeField] private Transform[] targetPoint;
	
	private SpoonTestPoint spoonTestPoint = SpoonTestPoint.Bowl;

	[FormerlySerializedAs("FoodSpoon")] [SerializeField] private Image foodSpoon;
	[FormerlySerializedAs("FoodBowl")] [SerializeField] private Transform foodBowl;
	[FormerlySerializedAs("FoodBowlStartPos")] [SerializeField] private Transform foodBowlStartPos;
	[FormerlySerializedAs("FoodBowlEndPos")] [SerializeField] private Transform foodBowlEndPos;

	public bool bMixingFood = false;
	[SerializeField] private bool bMixingFoodStarted = false;
	[SerializeField] private float timeMixingFood = 0;

	[FormerlySerializedAs("MixingFood")] [SerializeField] private Image mixingFood ;
	[FormerlySerializedAs("CleanWithTissue")] [SerializeField] private Image cleanWithTissue; 
	public BabyController babyC;

	[FormerlySerializedAs("TopMovementLimit")] [SerializeField] private GameObject topMovementLimit;
	
	private float mixVal = 1f;	
	private float timeNotMove = 0;
	
	private bool bMovingBack = false;
	
	private bool bFixAnimationMove = false;
	private Vector3 endAnimPos = Vector3.zero;

	private void Awake()
	{
		if(cleanWithTissue!=null) cleanWithTissue.color = new Color(1,1,1,0);
	}

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
				if(!pointerEventData.IsPointerMoving() && timeNotMove >.5f )
				{

					bMixingFoodStarted = false;
					if( SoundManager.Instance!=null)  SoundManager.Instance.Stop_Sound( SoundManager.Instance.MixingFood);
					return;
				}

			}

			timeMixingFood +=Time.deltaTime;
			timeNotMove+=Time.deltaTime;
			if( timeMixingFood>mixVal )
			{
				if( SoundManager.Instance!=null)  SoundManager.Instance.Play_Sound( SoundManager.Instance.MixingFood);
				mixVal +=1f;
				mixingFood.transform.localScale = new Vector3(-mixingFood.transform.localScale.x, mixingFood.transform.localScale.y, mixingFood.transform.localScale.z);
				 
			}

			if(timeMixingFood >5)
			{
				bMixingFoodStarted = false;
				//InvokeRepeating("TestTarget",0f, .1f);
				bDrag = false;
				 StartCoroutine("MoveBack");
				Camera.main.SendMessage("NextPhase", "MixedFood", SendMessageOptions.DontRequireReceiver);
				if( SoundManager.Instance!=null)  SoundManager.Instance.Stop_Sound( SoundManager.Instance.MixingFood);
			}
			 
		}
	}

	private void TestTarget()
	{ 
		if(spoonTestPoint == SpoonTestPoint.Bowl)
		{
			if( Vector2.Distance(testPoint.position,targetPoint[0].position) < testDistance)
		   	{
			    spoonTestPoint  = SpoonTestPoint.Mouth;
				foodSpoon.enabled = true;

				foodBowl.position = Vector3.Lerp(foodBowlStartPos.position,foodBowlEndPos.position,(feedCount+1)/5f);
				foodBowl.localScale = Vector3.one * (.8f + .2f - (feedCount+1)/25f);
				if(feedCount == 4) foodBowl.gameObject.SetActive(false);
			}
		}
		else if(spoonTestPoint == SpoonTestPoint.Mouth)
		{
			 
			if( Vector2.Distance(testPoint.position,targetPoint[1].position) < testDistance)
			{
				foodSpoon.enabled = false;
				feedCount++;
				spoonTestPoint = SpoonTestPoint.Bowl;
				
				if(cleanWithTissue!=null) cleanWithTissue.color = new Color(1,1,1, .2f*feedCount);
				babyC.BabyEat();
				if(feedCount == 5)
				{
					endAnimPos= transform.position;
					bFixAnimationMove = true;
					Camera.main.SendMessage("NextPhase", "FeedBaby", SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}
	private void LateUpdate()
	{
		if(bFixAnimationMove)
		{
			transform.position = endAnimPos;
		}
	}
 
		
	public void OnBeginDrag (PointerEventData eventData)
	{
		if(bMovingBack) return;
		if(topMovementLimit == null) topMovementLimit = GameObject.Find("TopMovementLimit");
		StopAllCoroutines(); 
		pointerEventData = eventData;

		if(  !bIsKoriScene   && !bDrag  )
		{
			bDrag = true;
			startPosition = transform.position;
			dragItemParent.position = transform.parent.position;
			transform.SetParent(dragItemParent);
			
			if(!bMixingFood)	
			{
				InvokeRepeating("TestTarget",0f, .1f);
				 babyC.BabyWaitToEat();
			}
			
		}

		Tutorial.Instance.PauseTutorial("spoon");
	}
	
	
	public void OnDrag (PointerEventData eventData)
	{
		if(bMixingFood)
		{
			if(eventData.IsPointerMoving() && ( Vector2.Distance(testPoint.position,targetPoint[0].position) < .8f) )
			{
				bMixingFoodStarted = true;
				timeNotMove = 0;
			}
		}
	}
	
	public void OnEndDrag (PointerEventData eventData)
	{
		
		if(  !bIsKoriScene &&  bDrag 	)  
		{
			bMixingFoodStarted = false;
			if( SoundManager.Instance!=null)  SoundManager.Instance.Stop_Sound( SoundManager.Instance.MixingFood);
			bDrag = false;
			StartCoroutine(nameof(MoveBack) );
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

				yield return new WaitForFixedUpdate( );
			}
			
			transform.SetParent(parentOld);
			transform.position = startPosition;

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
	
	
 
	bool appFoucs = true;
	private void OnApplicationFocus( bool hasFocus )
	{
		if(  !appFoucs && hasFocus )
		{
			if(  !bIsKoriScene &&  bDrag )
			{
				bDrag = false;
				
				CancelInvoke(nameof(TestTarget));
				StartCoroutine(nameof(MoveBack) );
			}
		}
		appFoucs = hasFocus;	
	}
 
		
	
	enum SpoonTestPoint
	{
		Bowl,
		Mouth
	}

}
