using UnityEngine;
using System.Collections;
using TemplateScripts;
using UnityEngine.Serialization;

public class MosquitoMove : MonoBehaviour {
	
	private float speed = .5f;
	[FormerlySerializedAs("LimitTopLeft")] [SerializeField] private Transform limitTopLeft;
	[FormerlySerializedAs("LimitBottomRight")] [SerializeField] private Transform limitBottomRight;

	public Vector3 targetPos = Vector3.zero;
	public Vector3 prevTargetPos = Vector3.zero;

	private float timePom = 0;

	private bool bMove = false;
	private bool bUnisten = false;

	private AudioSource mosquitoSound;
	
	public static int ActiveSoundsCount;
	
	private bool bSoundActive = false;
	private readonly int maxActiveSounds = 4;

	private void Awake () 
	{
		bMove = false;
	}
	
	private IEnumerator Start()
	{
		 
		mosquitoSound = transform.GetComponent<AudioSource>();
		if(SoundManager.SoundOn ==1 && ActiveSoundsCount<maxActiveSounds)
		{
			ActiveSoundsCount++;
			bSoundActive = true;
			mosquitoSound.Play((ulong) Random.Range(0,50000));
			mosquitoSound.pitch = Random.Range(.95f,1.5f);
		}


		transform.position = new Vector3( 2000,2000,2000);
		limitTopLeft = transform.parent.parent.Find("TL");
		limitBottomRight = transform.parent.parent.Find("BR");
		
		yield return new WaitForSeconds(.2f);
	
		if(Random.Range(0f,2f) >1)
			transform.position =      new Vector3(  limitTopLeft.position.x- Random.Range(2f,3f), Random.Range(limitBottomRight.position.y, limitTopLeft.position.y),0 );
		else
			transform.position =   new Vector3(limitBottomRight.position.x + Random.Range(2f,3f), Random.Range(limitBottomRight.position.y, limitTopLeft.position.y),0);
 
		StartCoroutine( nameof(SetTargetPos));
		bMove = true;
	}

	private IEnumerator SetTargetPos()
	{
		yield return new WaitForEndOfFrame();
		prevTargetPos = transform.position;
		targetPos = new Vector3(Random.Range(limitBottomRight.position.x, limitTopLeft.position.x), Random.Range(limitBottomRight.position.y, limitTopLeft.position.y));
		
		if(SoundManager.SoundOn ==1 &&   !bSoundActive && ActiveSoundsCount<maxActiveSounds)
		{
			ActiveSoundsCount++;
			bSoundActive = true;
			mosquitoSound.Play( (ulong) Random.Range(0,50000) );
			mosquitoSound.pitch = Random.Range(.95f,1.5f);
		}
	}
	
	private void Update () 
	{
		if(bMove)	
		{
			timePom +=Time.deltaTime*speed;
		
			transform.position = Vector3.Lerp(prevTargetPos,targetPos, timePom);
		
			if(timePom>1) 
			{
				speed = Random.Range(.1f,.3f);
				timePom = 0;
				StartCoroutine( nameof(SetTargetPos));
			}
		}
	}

	public void Smack()
	{
		if(!bUnisten)
		{
			bUnisten= true;
			transform.GetChild(0).gameObject.SetActive(false);
			transform.GetChild(1).gameObject.SetActive(true);
			Camera.main.SendMessage("NextPhase", "MosquitoHit", SendMessageOptions.DontRequireReceiver);

			if(	bSoundActive)
			{
				mosquitoSound.Stop();
				ActiveSoundsCount--;
			}

			Destroy(gameObject,1);
		}
	}


}
