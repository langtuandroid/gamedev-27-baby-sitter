using UnityEngine;
using System.Collections;
using TemplateScripts;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class RightMenuItemBS : MonoBehaviour ,  IBeginDragHandler, IDragHandler, IEndDragHandler
{ 
	private bool bDragG = false;
	
	[SerializeField] private bool bEnableDrag = true;
	[SerializeField] private bool bSnapToTarget = false;

	private bool bSnappingG = false;
	
	private float x;
	private float y;
	
	private Vector3 dragOffsetT = new Vector3(0,0,0);
	
	private Transform startParentT;
	private Vector3 startPositionN;
	private Transform activeItemM;
	
	[FormerlySerializedAs("ActiveItemHolder")] [SerializeField] private Transform activeItemHolder;
	private readonly int mouseFollowSpeedD = 10; 
	
	[FormerlySerializedAs("TargetPoint")] [SerializeField] private Transform targetPoint;
	[SerializeField] private GameObject prefabDragItem;
	[SerializeField] private int itemPhaseNo = 0;

	private bool bSelectableE = true;

	[SerializeField] private string animationName ="";
	
	[SerializeField] private Animator anim;

	[FormerlySerializedAs("SnapDistance")] [SerializeField] private float snapDistance = 2f;
	[SerializeField] private CanvasGroup cg;

	private float progressS = 0;
	[SerializeField] private ParticleSystem psToothbrush;
	[SerializeField] private ParticleSystem psToothbrush2;

	[FormerlySerializedAs("Toothbrush1")] [SerializeField] private Image toothbrush1;
	[FormerlySerializedAs("Toothbrush2")] [SerializeField] private Image toothbrush2;
	[SerializeField] private BabyControllerBs babyC;

	[FormerlySerializedAs("BlackTeeth1")] [SerializeField] private Image blackTeeth1;
	[FormerlySerializedAs("BlackTeeth2")] [SerializeField] private Image blackTeeth2;
	[SerializeField] private ParticleSystem psSparklesTeeth;

	[FormerlySerializedAs("TopMovementLimit")] [SerializeField] private GameObject topMovementLimit;

	private void Start () {
		startPositionN = prefabDragItem.transform.localPosition;
		startParentT = prefabDragItem.transform.parent;
	}
	
	private void Update () 
	{
		if( bDragG  && bEnableDrag)  
		{
			x = Input.mousePosition.x;
			y = Input.mousePosition.y;
			
			Vector3 posM = Camera.main.ScreenToWorldPoint(new Vector3(x ,y,10.0f)    ) - dragOffsetT; 
			if( topMovementLimit!=null && posM.y >topMovementLimit.transform.position.y) posM = new Vector3(posM.x, topMovementLimit.transform.position.y  ,posM.z);
			activeItemM.position =  Vector3.Lerp (activeItemM.position, posM  , mouseFollowSpeedD * Time.deltaTime)  ;
		}
	}
	
	public void OnBeginDrag (PointerEventData eventData)
	{
		if(  !bEnableDrag || !bSelectableE) return;
		if( Application.loadedLevelName == "Minigame 4"  && Minigame4BS.CompletedActionNoN != itemPhaseNo) return;

		if(topMovementLimit == null) topMovementLimit = GameObject.Find("TopMovementLimit");
		if(!bSnappingG )
		{
			bDragG = true;
			StopAllCoroutines();
			activeItemM =  prefabDragItem.transform;
			prefabDragItem.SetActive(true);
			activeItemM.SetParent(activeItemHolder);
			 

			activeItemM.localScale = Vector3.one;
			cg.alpha =1;
			
			
			dragOffsetT =  Camera.main.ScreenToWorldPoint(Input.mousePosition) - activeItemM.position; 
			dragOffsetT = new Vector3(dragOffsetT.x,dragOffsetT.y,0);
			
			InvokeRepeating("TestDistanceE",0, .2f);
			if(  SoundManagerBS.Instance!=null)  	SoundManagerBS.Instance.StopAndPlay_Sound( SoundManagerBS.Instance.ButtonClick2);

			if(Application.loadedLevelName == "Minigame 4")
			{
				if(Minigame4BS.CompletedActionNoN != 3  )   TutorialBS.Instance.PauseTutorialL("RightMenu ");
			}
		}
	}
	
	
	public void OnDrag(PointerEventData eventData)
	{
		
	}
	
	public void OnEndDrag(PointerEventData eventData)
	{
		if(  !bEnableDrag) return;
		if(  bDragG   ) 
		{
			bDragG = false;
			CancelInvoke(nameof(TestDistanceE));
			if(animationName == "TissueClean" )StartCoroutine(nameof(HideTissueAnimM));
			else if(animationName == "Toothbrush")
			{
				psToothbrush.enableEmission = false;
				psToothbrush2.enableEmission = false;
				if(  SoundManagerBS.Instance!=null)  	SoundManagerBS.Instance.StopSound( SoundManagerBS.Instance.TeethH);
				StartCoroutine(nameof(HideToothbrushAnimM));
				babyC.BabyBrushTeeth(false);
			}
		}
	}
	
	private void TestDistanceE()
	{
		if(Vector2.Distance(activeItemM.position, targetPoint .position)<snapDistance)
		{
			if( bSnapToTarget )
			{

				StartCoroutine(nameof(SnapToTargetT));
				bDragG = false;
			}
			else
			{
				progressS+=.1f;

				if(progressS < 3)
				{
					if(psToothbrush!=null)
					{
						if(psToothbrush.gameObject.activeSelf == false) psToothbrush.gameObject.SetActive(true);
						psToothbrush.enableEmission = true;
						psToothbrush2.enableEmission = true;
						if(  SoundManagerBS.Instance!=null)  	SoundManagerBS.Instance.StopAndPlay_Sound( SoundManagerBS.Instance.TeethH);
						toothbrush2.enabled =true;
						toothbrush1.enabled =false;
						babyC.BabyBrushTeeth(true);
						if(progressS<=2)
						{
							blackTeeth1.color = new Color(1,1,1,1-progressS/2f);
							blackTeeth2.color = new Color(1,1,1,1-progressS/2f);
						}
						else
						{
							blackTeeth1.enabled = false;
							blackTeeth2.enabled = false;
						}
					}
				}
				else
				{
					bDragG = false;
					CancelInvoke(nameof(TestDistanceE));
					if(psToothbrush!=null)
					{
						psToothbrush.enableEmission = false;
						psToothbrush2.enableEmission = false;
						if(  SoundManagerBS.Instance!=null)  	SoundManagerBS.Instance.StopSound( SoundManagerBS.Instance.TeethH);
						toothbrush1.enabled =true;
						toothbrush2.enabled =false;
					}
					StartCoroutine(nameof(HideToothbrushAnimM));
					Camera.main.SendMessage("CompletedActionN");

					blackTeeth1.color = new Color(1,1,1,0);
					blackTeeth2.color = new Color(1,1,1,0);
					psSparklesTeeth.gameObject.SetActive(true);
					psSparklesTeeth.Play();
					if(  SoundManagerBS.Instance!=null)  	SoundManagerBS.Instance.StopAndPlay_Sound( SoundManagerBS.Instance.ButtonClick2);
				}
			}
		}	
		else
		{
			if(!bSnapToTarget && psToothbrush!=null)
			{
				psToothbrush.enableEmission = false;
				psToothbrush2.enableEmission = false;
				if(  SoundManagerBS.Instance!=null)  	SoundManagerBS.Instance.StopSound( SoundManagerBS.Instance.TeethH);
				toothbrush1.enabled =true;
				toothbrush2.enabled =false;
				babyC.BabyBrushTeeth(false);
			}
		}
	}
	
	private IEnumerator SnapToTargetT()
	{
		if(!bSnappingG  ) 
		{
			bSnappingG = true;
			bDragG = false;
		 
			
			float timeMove = 0;
			
			while  (timeMove  <.5f )
			{
				yield return new WaitForFixedUpdate();
				activeItemM.position = Vector3.Lerp( activeItemM.position , targetPoint.position , timeMove *2)  ;
				
				timeMove += Time.fixedDeltaTime;
				
			}

			if(animationName!="" && anim !=null)
			{
				anim.Play(animationName,-1,0);
				if( animationName == "TissueClean" &&  SoundManagerBS.Instance!=null)  	SoundManagerBS.Instance.PlaySound( SoundManagerBS.Instance.NoseCleanN);
			}
			
			//if(SoundManager.Instance!=null) SoundManager.Instance.StopAndPlay_Sound( SoundManager.Instance.Needle);
			
		}
		yield return new WaitForFixedUpdate();
	}
	
	
	public void HideTissueE()
	{
		StartCoroutine(nameof(HideTissueAnimM));
	}
	
	private IEnumerator HideTissueAnimM()
	{
		GameObject.Destroy( GameObject.Find ("CleanWithTissue"));
		bDragG = false;
		yield return new WaitForFixedUpdate();
		activeItemM.SetParent(startParentT);
		while(cg.alpha>0.05f)
		{
			cg.alpha -=Time.fixedDeltaTime*2;
			activeItemM.position += Vector3.down*Time.fixedDeltaTime*4;
			yield return new WaitForFixedUpdate();
 
		}

		if(bSnappingG )
		{
			bSelectableE = false; 
			Camera.main.SendMessage("NextPhaseE", "TissueClean" );
		}
		activeItemM.localPosition = startPositionN;
		activeItemM.gameObject.SetActive(false);
	}
	
	private IEnumerator HideToothbrushAnimM()
	{
		bDragG = false;
		yield return new WaitForFixedUpdate();
		activeItemM.SetParent(startParentT);
		Vector3 pos = activeItemM.localPosition;
		float pom =0;

//		if(bSnaping )
//		{
//			Camera.main.SendMessage("CompletedAction");
//			if(  SoundManager.Instance!=null)  	SoundManager.Instance.StopAndPlay_Sound( SoundManager.Instance.ButtonClick2);
//		}
		while(pom<1f)
		{

			if(!bSnappingG )
			{

				pom +=Time.fixedDeltaTime*4;
				activeItemM.localPosition = 	Vector3.Lerp(pos,startPositionN,pom);
			}
			else
			{
				pom +=Time.fixedDeltaTime*2;
				cg.alpha  = 1-pom;
			}
			yield return new WaitForFixedUpdate();
		}
		
	 
		if(bSnappingG )
		{
			bSelectableE = false; 
		}
		else
		{
			activeItemM.localPosition = startPositionN;
			activeItemM.gameObject.SetActive(false);
		}
	}

 
	
	
}
