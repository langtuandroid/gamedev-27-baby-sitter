using UnityEngine;
using TemplateScripts;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class MosquitoStick : MonoBehaviour, IPointerClickHandler
{
	public bool bEnabled = false;
	private bool bAnimationN = false;
	
	[FormerlySerializedAs("TestPoint")] [SerializeField] private Transform testPoint;
	
	private readonly float testDistanceE = .5f;
	private Animator animM;
	private float timePomM = 0;
	private Vector3 startPosS;
	private Vector3 endPosS;
	
	[FormerlySerializedAs("TopMovementLimit")] [SerializeField] private GameObject topMovementLimit;

	private void Start () {

		startPosS = new Vector3(5,-2,0);
		endPosS = new Vector3(0.5f,-2,0);

		animM = transform.GetChild(0).GetComponent<Animator>();
	}
	
	public void Update()
	{
		if(bEnabled && timePomM <1)
		{
			timePomM +=Time.deltaTime*4;
			animM.transform.position = Vector3.Lerp(startPosS,endPosS,timePomM);
		}
	}

	public void OnPointerClick (PointerEventData eventData)
	{
		if( bEnabled  && !bAnimationN ) 
		{
			if(topMovementLimit == null) topMovementLimit = GameObject.Find("TopMovementLimit");

			timePomM = 0;
			startPosS = animM.transform.position;
			endPosS = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x ,Input.mousePosition.y,10f) );
			if( topMovementLimit!=null && endPosS.y >topMovementLimit.transform.position.y) endPosS = new Vector3(endPosS.x, topMovementLimit.transform.position.y  ,endPosS.z);

			animM.Play("Smack",-1,0);
			bAnimationN = true;
			TutorialBS.Instance.StopTutor();
		}
	}

	public void AnimSmack_End()
	{
		bAnimationN = false;
		 
		Collider2D[] hitColliders = Physics2D.OverlapCircleAll(testPoint.position, testDistanceE  , 1 << LayerMask.NameToLayer("Tool1Interact")); 
		for ( int i = 0; i<hitColliders.Length; i ++  )
		{
			hitColliders[i].SendMessage("Smack");
		}

		if(  SoundManagerBS.Instance!=null)  	SoundManagerBS.Instance.PlaySound( SoundManagerBS.Instance.MoquitoSmackK);
	}

	public void HideE()
	{
		bEnabled = false;
		bAnimationN = true;
		animM.Play("completed",-1,0);
	}
}
