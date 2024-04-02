using UnityEngine;
using System.Collections;
using TemplateScripts;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class RightMenuItem : MonoBehaviour ,  IBeginDragHandler, IDragHandler, IEndDragHandler
{ 
	private bool bDrag = false;
	
	[SerializeField] private bool bEnableDrag = true;
	[SerializeField] private bool bSnapToTarget = false;

	private bool bSnapping = false;
	
	private float x;
	private float y;
	
	private Vector3 dragOffset = new Vector3(0,0,0);
	
	private Transform startParent;
	private Vector3 startPosition;
	private Transform activeItem;
	
	[FormerlySerializedAs("ActiveItemHolder")] [SerializeField] private Transform activeItemHolder;
	private readonly int mouseFollowSpeed = 10; 
	
	[FormerlySerializedAs("TargetPoint")] [SerializeField] private Transform targetPoint;
	[SerializeField] private GameObject prefabDragItem;
	[SerializeField] private int itemPhaseNo = 0;

	private bool bSelectable = true;

	[SerializeField] private string animationName ="";
	
	[SerializeField] private Animator anim;

	[FormerlySerializedAs("SnapDistance")] [SerializeField] private float snapDistance = 2f;
	[SerializeField] private CanvasGroup cg;

	private float progress = 0;
	[SerializeField] private ParticleSystem psToothbrush;
	[SerializeField] private ParticleSystem psToothbrush2;

	[FormerlySerializedAs("Toothbrush1")] [SerializeField] private Image toothbrush1;
	[FormerlySerializedAs("Toothbrush2")] [SerializeField] private Image toothbrush2;
	[SerializeField] private BabyController babyC;

	[FormerlySerializedAs("BlackTeeth1")] [SerializeField] private Image blackTeeth1;
	[FormerlySerializedAs("BlackTeeth2")] [SerializeField] private Image blackTeeth2;
	[SerializeField] private ParticleSystem psSparklesTeeth;

	[FormerlySerializedAs("TopMovementLimit")] [SerializeField] private GameObject topMovementLimit;

	private void Start () {
		startPosition = prefabDragItem.transform.localPosition;
		startParent = prefabDragItem.transform.parent;
	//	cg = prefabDragItem.GetComponent<CanvasGroup>();
	 
	}
	
	private void Update () 
	{
		if( bDrag  && bEnableDrag)  
		{
			x = Input.mousePosition.x;
			y = Input.mousePosition.y;
			
			Vector3 posM = Camera.main.ScreenToWorldPoint(new Vector3(x ,y,10.0f)    ) - dragOffset; 
			if( topMovementLimit!=null && posM.y >topMovementLimit.transform.position.y) posM = new Vector3(posM.x, topMovementLimit.transform.position.y  ,posM.z);
			activeItem.position =  Vector3.Lerp (activeItem.position, posM  , mouseFollowSpeed * Time.deltaTime)  ;
		}
	}
	
	public void OnBeginDrag (PointerEventData eventData)
	{
		if(  !bEnableDrag || !bSelectable) return;
		if( Application.loadedLevelName == "Minigame 4"  && Minigame4.CompletedActionNo != itemPhaseNo) return;

		if(topMovementLimit == null) topMovementLimit = GameObject.Find("TopMovementLimit");
		if(!bSnapping )
		{
			bDrag = true;
			StopAllCoroutines();
			activeItem =  prefabDragItem.transform;
			prefabDragItem.SetActive(true);
			activeItem.SetParent(activeItemHolder);
			 

			activeItem.localScale = Vector3.one;
			cg.alpha =1;
			
			
			dragOffset =  Camera.main.ScreenToWorldPoint(Input.mousePosition) - activeItem.position; 
			dragOffset = new Vector3(dragOffset.x,dragOffset.y,0);
			
			InvokeRepeating("TestDistance",0, .2f);
			if(  SoundManager.Instance!=null)  	SoundManager.Instance.StopAndPlay_Sound( SoundManager.Instance.ButtonClick2);

			if(Application.loadedLevelName == "Minigame 4")
			{
				if(Minigame4.CompletedActionNo != 3  )   Tutorial.Instance.PauseTutorial("RightMenu ");
			}
		}
	}
	
	
	public void  OnDrag(PointerEventData eventData)
	{
		
	}
	
	public void  OnEndDrag(PointerEventData eventData)
	{
		if(  !bEnableDrag) return;
		if(  bDrag   ) 
		{
			bDrag = false;
			CancelInvoke(nameof(TestDistance));
			if(animationName == "TissueClean" )StartCoroutine(nameof(HideTissueAnim));
			else if(animationName == "Toothbrush")
			{
				psToothbrush.enableEmission = false;
				psToothbrush2.enableEmission = false;
				if(  SoundManager.Instance!=null)  	SoundManager.Instance.Stop_Sound( SoundManager.Instance.Teeth);
				StartCoroutine(nameof(HideToothbrushAnim));
				babyC.BabyBrushTeeth(false);
			}
		}
	}
	
	private void TestDistance()
	{
		if(Vector2.Distance(activeItem.position, targetPoint .position)<snapDistance)
		{
			if( bSnapToTarget )
			{

				StartCoroutine(nameof(SnapToTarget));
				bDrag = false;
			}
			else
			{
				progress+=.1f;
				//Debug.Log("PROG "+progres);


				if(progress < 3)
				{
					if(psToothbrush!=null)
					{
						if(psToothbrush.gameObject.activeSelf == false) psToothbrush.gameObject.SetActive(true);
						psToothbrush.enableEmission = true;
						psToothbrush2.enableEmission = true;
						if(  SoundManager.Instance!=null)  	SoundManager.Instance.StopAndPlay_Sound( SoundManager.Instance.Teeth);
						toothbrush2.enabled =true;
						toothbrush1.enabled =false;
						babyC.BabyBrushTeeth(true);
						if(progress<=2)
						{
							blackTeeth1.color = new Color(1,1,1,1-progress/2f);
							blackTeeth2.color = new Color(1,1,1,1-progress/2f);
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
					bDrag = false;
					CancelInvoke(nameof(TestDistance));
					if(psToothbrush!=null)
					{
						psToothbrush.enableEmission = false;
						psToothbrush2.enableEmission = false;
						if(  SoundManager.Instance!=null)  	SoundManager.Instance.Stop_Sound( SoundManager.Instance.Teeth);
						toothbrush1.enabled =true;
						toothbrush2.enabled =false;
					}
					StartCoroutine(nameof(HideToothbrushAnim));
					Camera.main.SendMessage("CompletedAction");

					blackTeeth1.color = new Color(1,1,1,0);
					blackTeeth2.color = new Color(1,1,1,0);
					psSparklesTeeth.gameObject.SetActive(true);
					psSparklesTeeth.Play();
					if(  SoundManager.Instance!=null)  	SoundManager.Instance.StopAndPlay_Sound( SoundManager.Instance.ButtonClick2);
				}
			}
		}	
		else
		{
			if(!bSnapToTarget && psToothbrush!=null)
			{
				psToothbrush.enableEmission = false;
				psToothbrush2.enableEmission = false;
				if(  SoundManager.Instance!=null)  	SoundManager.Instance.Stop_Sound( SoundManager.Instance.Teeth);
				toothbrush1.enabled =true;
				toothbrush2.enabled =false;
				babyC.BabyBrushTeeth(false);
			}
		}
	}
	
	private IEnumerator SnapToTarget()
	{
		if(!bSnapping  ) 
		{
			bSnapping = true;
			bDrag = false;
		 
			
			float timeMove = 0;
			
			while  (timeMove  <.5f )
			{
				yield return new WaitForFixedUpdate();
				activeItem.position = Vector3.Lerp( activeItem.position , targetPoint.position , timeMove *2)  ;
				
				timeMove += Time.fixedDeltaTime;
				
			}

			if(animationName!="" && anim !=null)
			{
				anim.Play(animationName,-1,0);
				if( animationName == "TissueClean" &&  SoundManager.Instance!=null)  	SoundManager.Instance.Play_Sound( SoundManager.Instance.NoseClean);
			}
			
			//if(SoundManager.Instance!=null) SoundManager.Instance.StopAndPlay_Sound( SoundManager.Instance.Needle);
			
		}
		yield return new WaitForFixedUpdate();
	}
	
	
	public void HideTissue()
	{
		StartCoroutine(nameof(HideTissueAnim));
	}
	
	private IEnumerator HideTissueAnim()
	{
		GameObject.Destroy( GameObject.Find ("CleanWithTissue"));
		bDrag = false;
		yield return new WaitForFixedUpdate();
		activeItem.SetParent(startParent);
		while(cg.alpha>0.05f)
		{
			cg.alpha -=Time.fixedDeltaTime*2;
			activeItem.position += Vector3.down*Time.fixedDeltaTime*4;
			yield return new WaitForFixedUpdate();
 
		}

		//Debug.Log(bSnaping);
		if(bSnapping )
		{
			bSelectable = false; 
			Camera.main.SendMessage("NextPhase", "TissueClean" );
		}
		activeItem.localPosition = startPosition;
		activeItem.gameObject.SetActive(false);
	}
	
	private IEnumerator HideToothbrushAnim()
	{
		bDrag = false;
		yield return new WaitForFixedUpdate();
		activeItem.SetParent(startParent);
		Vector3 pos = activeItem.localPosition;
		float pom =0;

//		if(bSnaping )
//		{
//			Camera.main.SendMessage("CompletedAction");
//			if(  SoundManager.Instance!=null)  	SoundManager.Instance.StopAndPlay_Sound( SoundManager.Instance.ButtonClick2);
//		}
		while(pom<1f)
		{

			if(!bSnapping )
			{

				pom +=Time.fixedDeltaTime*4;
				activeItem.localPosition = 	Vector3.Lerp(pos,startPosition,pom);
			}
			else
			{
				pom +=Time.fixedDeltaTime*2;
				cg.alpha  = 1-pom;
			}
			yield return new WaitForFixedUpdate();
		}
		
	 
		if(bSnapping )
		{
			bSelectable = false; 
		}
		else
		{
			activeItem.localPosition = startPosition;
			activeItem.gameObject.SetActive(false);
		}
	}

 
	
	
}
