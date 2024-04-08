using UnityEngine;
using System.Collections;
using TemplateScripts;
using UnityEngine.Serialization;

public class MosquitoMove : MonoBehaviour {
	
	private float speedD = .5f;
	[FormerlySerializedAs("LimitTopLeft")] [SerializeField] private Transform limitTopLeft;
	[FormerlySerializedAs("LimitBottomRight")] [SerializeField] private Transform limitBottomRight;

	public Vector3 targetPosS = Vector3.zero;
	public Vector3 prevTargetPosS = Vector3.zero;

	private float timePomM = 0;

	private bool bMoveE = false;
	private bool bUnistenN = false;

	private AudioSource mosquitoSoundD;
	
	public static int ActiveSoundsCountT;
	
	private bool bSoundActiveE = false;
	private readonly int maxActiveSoundsS = 4;

	private void Awake () 
	{
		bMoveE = false;
	}
	
	private IEnumerator Start()
	{
		mosquitoSoundD = transform.GetComponent<AudioSource>();
		if(SoundManagerBS.SoundOnN ==1 && ActiveSoundsCountT<maxActiveSoundsS)
		{
			ActiveSoundsCountT++;
			bSoundActiveE = true;
			mosquitoSoundD.Play((ulong) Random.Range(0,50000));
			mosquitoSoundD.pitch = Random.Range(.95f,1.5f);
		}


		transform.position = new Vector3( 2000,2000,2000);
		limitTopLeft = transform.parent.parent.Find("TL");
		limitBottomRight = transform.parent.parent.Find("BR");
		
		yield return new WaitForSeconds(.2f);
	
		if(Random.Range(0f,2f) >1)
			transform.position =      new Vector3(  limitTopLeft.position.x- Random.Range(2f,3f), Random.Range(limitBottomRight.position.y, limitTopLeft.position.y),0 );
		else
			transform.position =   new Vector3(limitBottomRight.position.x + Random.Range(2f,3f), Random.Range(limitBottomRight.position.y, limitTopLeft.position.y),0);
 
		StartCoroutine( nameof(SetTargetPosS));
		bMoveE = true;
	}

	private IEnumerator SetTargetPosS()
	{
		yield return new WaitForEndOfFrame();
		prevTargetPosS = transform.position;
		targetPosS = new Vector3(Random.Range(limitBottomRight.position.x, limitTopLeft.position.x), Random.Range(limitBottomRight.position.y, limitTopLeft.position.y));
		
		if(SoundManagerBS.SoundOnN ==1 &&   !bSoundActiveE && ActiveSoundsCountT<maxActiveSoundsS)
		{
			ActiveSoundsCountT++;
			bSoundActiveE = true;
			mosquitoSoundD.Play( (ulong) Random.Range(0,50000) );
			mosquitoSoundD.pitch = Random.Range(.95f,1.5f);
		}
	}
	
	private void Update () 
	{
		if(bMoveE)	
		{
			timePomM +=Time.deltaTime*speedD;
		
			transform.position = Vector3.Lerp(prevTargetPosS,targetPosS, timePomM);
		
			if(timePomM>1) 
			{
				speedD = Random.Range(.1f,.3f);
				timePomM = 0;
				StartCoroutine( nameof(SetTargetPosS));
			}
		}
	}

	public void SmackK()
	{
		if(!bUnistenN)
		{
			bUnistenN= true;
			transform.GetChild(0).gameObject.SetActive(false);
			transform.GetChild(1).gameObject.SetActive(true);
			Camera.main.SendMessage("NextPhase", "MosquitoHit", SendMessageOptions.DontRequireReceiver);

			if(	bSoundActiveE)
			{
				mosquitoSoundD.Stop();
				ActiveSoundsCountT--;
			}

			Destroy(gameObject,1);
		}
	}


}
