using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TemplateScripts;
//using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class MosquitoStick : MonoBehaviour, IPointerClickHandler
{
	public bool bEnabled = false;
	private bool bAnimation = false;
	
	[FormerlySerializedAs("TestPoint")] [SerializeField] private Transform testPoint;
	
	private readonly float testDistance = .5f;
	private Animator anim;
	private float timePom = 0;
	private Vector3 startPos;
	private Vector3 endPos;
	
	[FormerlySerializedAs("TopMovementLimit")] [SerializeField] private GameObject topMovementLimit;

	private void Start () {

		startPos = new Vector3(5,-2,0);
		endPos = new Vector3(0.5f,-2,0);

		anim = transform.GetChild(0).GetComponent<Animator>();
	}
	
	public void Update()
	{
		if(bEnabled && timePom <1)
		{
			timePom +=Time.deltaTime*4;
			anim.transform.position = Vector3.Lerp(startPos,endPos,timePom);
		}
	}

	public void OnPointerClick (PointerEventData eventData)
	{
		if( bEnabled  && !bAnimation ) 
		{
			if(topMovementLimit == null) topMovementLimit = GameObject.Find("TopMovementLimit");

			timePom = 0;
			startPos = anim.transform.position;
			endPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x ,Input.mousePosition.y,10f) );
			if( topMovementLimit!=null && endPos.y >topMovementLimit.transform.position.y) endPos = new Vector3(endPos.x, topMovementLimit.transform.position.y  ,endPos.z);

			anim.Play("Smack",-1,0);
			bAnimation = true;
			Tutorial.Instance.StopTutorial();
		}
	}

	public void AnimSmackEnd()
	{
		bAnimation = false;
		 
		Collider2D[] hitColliders = Physics2D.OverlapCircleAll(testPoint.position, testDistance  , 1 << LayerMask.NameToLayer("Tool1Interact")); 
		for ( int i = 0; i<hitColliders.Length; i ++  )
		{
			hitColliders[i].SendMessage("Smack");
		}

		if(  SoundManager.Instance!=null)  	SoundManager.Instance.Play_Sound( SoundManager.Instance.MoquitoSmack);
	}

	public void Hide()
	{
		bEnabled = false;
		bAnimation = true;
		anim.Play("completed",-1,0);
	}
}
