using UnityEngine;
using UnityEngine.UI;
using TemplateScripts;
using UnityEngine.Serialization;

public class BabyControllerBs : MonoBehaviour {

	private Animator animM;
	private Animator animEyesE;

	[FormerlySerializedAs("redSpots")] [SerializeField] private GameObject[] redSpotsS;
	[SerializeField] private Image diaper;
	[FormerlySerializedAs("poop")] [SerializeField] private GameObject poopP;
	[FormerlySerializedAs("dirtCltohes")] [SerializeField] private GameObject dirtClothes;
	[FormerlySerializedAs("clothes")] [SerializeField] private Image clothesE;
	[FormerlySerializedAs("cleanWithTissue")] [SerializeField] private GameObject cleanWithTissueE;
	
	[FormerlySerializedAs("atlas")] [SerializeField] private SetBabyAtlasBS atlass;
	
	public void SelectMinigameSetBaby(int baby, int minigame)
	{
		animM = transform.GetComponent<Animator>();
		atlass =  transform.GetComponent<SetBabyAtlasBS>();
		
		atlass.SetBabyY(baby);
		atlass.SetMiniGameE(minigame);
		
		switch(minigame)
		{
		case 0:
		{
			clothesE.gameObject.SetActive(true);
			dirtClothes.SetActive(true);
			BBabyCryingIdle();
			break;
		}
		case 1:
		{
			BBabyIdle();
			break;
		}
		case 2:
		{
			clothesE.gameObject.SetActive(true);
			BBabySleepy();
			break;
		}
		case 3:
		{
			redSpotsS[0].SetActive(true);
			redSpotsS[1].SetActive(true);
			cleanWithTissueE.SetActive(true);
			poopP.SetActive(true);
			BBabyCryingIdle();
			break;
		}
		case 4:
		{
			BBabyCryingIdle();
			break;
		}
		case 5:
		{
			BBabyIdle();
			break;
		}
		case 6:
		{
			BBabySleepy();
			break;
		}
		case 7:
		{
			BBabyCryingIdle();
			break;
		}

		default:
			BBabyIdle();
			break;
		}
	}


	private void Start () {
		animM = transform.GetComponent<Animator>();

		if( SoundManagerBS.Instance!=null) 
		{
			SoundManagerBS.Instance.listStopSoundOnExit.Add( SoundManagerBS.Instance.BabyChewW);
			SoundManagerBS.Instance.listStopSoundOnExit.Add( SoundManagerBS.Instance.BabyCryY);

		}
	}

	public void AnimHappy_Completed()
	{
		animM.SetInteger("Happy", Random.Range(1,4));
		animM.speed = Random.Range(0.8f,1.2f);
	}

	public void AnimCrying_Completed()
	{

		animM.SetInteger("Crying", Random.Range(1,4));
		animM.speed = Random.Range(0.8f,1.2f);
	}
	
	public void BBabyIdle()
	{
		CancelInvoke();
		animM.SetBool("bCrying",false);
		animM.SetInteger("Crying", 0);
		animM.SetBool("bEat",false);
		 
		animM.ResetTrigger("tSmile");

		switch(Random.Range(1,4))
		{
		case 1: animM.Play ("BabyHappyIdle",-1,0); animM.SetInteger("Happy",1);break;
		case 2: animM.Play ("BabyHappyIdle2",-1,0); animM.SetInteger("Happy",2);break;
		case 3: animM.Play ("BabyHappyIdle3",-1,0); animM.SetInteger("Happy",3);break;
		}
		 
		animM.speed = Random.Range(0.9f,1.2f);
	}

	public void BBabySmile()
	{
		CancelInvoke();
		animM.SetBool("bCrying",false);
		animM.SetInteger("Crying", 0);
		animM.SetBool("bEat",false);
		animM.SetInteger("Happy",2);
		 animM.SetTrigger("tSmile");
		//anim.Play("BabyHappySmile",-1,0); 

	}

	public void BBabyWaitToEat()
	{
		CancelInvoke();
		animM.Play("BabyWaitEatAnimation",-1,0);
		animM.SetBool("bEat",true);
		animM.SetInteger("Happy",0);
	}

	public void BBabyEat()
	{
		CancelInvoke();
		animM.Play("BabyEatAnimation",-1,0);
		animM.SetBool("bEat",true);
		if( SoundManagerBS.Instance!=null) SoundManagerBS.Instance.StopAndPlay_Sound( SoundManagerBS.Instance.BabyChewW,.1f); 
	}

	public void BBabyCryingIdle()
	{
		animM.SetInteger("Happy",0);
		 
		animM.SetBool("bCrying",true);
		switch(Random.Range(1,4))
		{
		case 1: animM.Play ("BabyCryingIdle",-1,0);break;
		case 2: animM.Play ("BabyCryingIdle2",-1,0);break;
		case 3: animM.Play ("BabyCryingIdle3",-1,0);break;
		}
		animM.speed = Random.Range(0.9f,1.2f);
		CancelInvoke();
		Invoke(nameof(BBabyCryOpenMouth), Random.Range(1.5f,4f));
	}

	public void BBabyCryOpenMouth()
	{
		animM.SetInteger("Happy",0);
		animM.SetBool("bCrying",true);
		animM.SetTrigger("tCryOpenMouth");
		CancelInvoke();
		Invoke(nameof(BBabyCryOpenMouth), Random.Range(4.5f,12f));

		if( SoundManagerBS.Instance!=null) SoundManagerBS.Instance.StopAndPlay_Sound( SoundManagerBS.Instance.BabyCryY,.1f); 
	}

	public void BBabySleepy()
	{
		animM.SetInteger("Happy",0);
		animM.SetInteger("Crying", 0);
		animM.SetBool("bCrying",false);
		animM.SetBool("bEat",false);
		animM.ResetTrigger("tCryOpenMouth");
		animM.Play("Sleepy",-1,0);
		CancelInvoke();
	 
	}

	public void BBabySleepy2()
	{
		animM.SetInteger("Happy",0);
		animM.SetInteger("Crying", 0);
		animM.SetBool("bCrying",false);
		animM.SetBool("bEat",false);
		animM.ResetTrigger("tCryOpenMouth");
		animM.Play("Sleepy2",-1,0);
		CancelInvoke();
	}

	public void BBabySleeping()
	{
		animM.SetInteger("Happy",0);
		animM.SetInteger("Crying", 0);
		animM.SetBool("bCrying",false);
		animM.SetBool("bEat",false);
		animM.ResetTrigger("tCryOpenMouth");
		animM.Play("Sleeping",-1,0);
		CancelInvoke();
	}

	public void BBabyBath()
	{
		animM.SetInteger("Happy",0);
		animM.SetInteger("Crying", 0);
		animM.SetBool("bCrying",false);
		animM.SetBool("bEat",false);
		animM.ResetTrigger("tCryOpenMouth");
		animM.Play("BabyBath",-1,0);
		CancelInvoke();
	}
	
	public void BBabyWaitToBrushTeeth()
	{
		CancelInvoke();
		animM.Play("BrushTeethWait",-1,0);
		animM.SetBool("bBrushingTeeth",false);
		animM.SetInteger("Happy",0);
		 
		
	}
	
	public void BBabyBrushTeeth(bool bBrushingTeeth)
	{
		animM.SetBool("bBrushingTeeth",bBrushingTeeth);
	}




}
