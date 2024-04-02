using UnityEngine;
using UnityEngine.UI;
using TemplateScripts;

public class BabyController : MonoBehaviour {

	private Animator anim;
	private Animator animEyes;

	[SerializeField] private GameObject[] redSpots;
	[SerializeField] private Image diaper;
	[SerializeField] private GameObject poop;
	[SerializeField] private GameObject dirtCltohes;
	[SerializeField] private Image clothes;
	[SerializeField] private GameObject cleanWithTissue;
	
	[SerializeField] private SetBabyAtlas atlas;
	
	public void SelectMinigame_SetBaby( int baby,  int minigame)
	{
		anim = transform.GetComponent<Animator>();
		atlas =  transform.GetComponent<SetBabyAtlas>();


		atlas.SetBaby(baby);
		atlas.SetMiniGame(minigame);


		switch(minigame)
		{
		case 0:
		{
			clothes.gameObject.SetActive(true);
			dirtCltohes.SetActive(true);
			BabyCryingIdle();
			break;
		}
		case 1:
		{
			BabyIdle();
			break;
		}
		case 2:
		{
			clothes.gameObject.SetActive(true);
			BabySleepy();
			break;
		}
		case 3:
		{
			redSpots[0].SetActive(true);
			redSpots[1].SetActive(true);
			cleanWithTissue.SetActive(true);
			poop.SetActive(true);
			BabyCryingIdle();
			break;
		}
		case 4:
		{
			BabyCryingIdle();
			break;
		}
		case 5:
		{
			BabyIdle();
			break;
		}
		case 6:
		{
			BabySleepy();
			break;
		}
		case 7:
		{
			BabyCryingIdle();
			break;
		}

		default:
			BabyIdle();
			break;
		}
	}


	private void Start () {
		anim = transform.GetComponent<Animator>();

		if( SoundManager.Instance!=null) 
		{
			SoundManager.Instance.listStopSoundOnExit.Add( SoundManager.Instance.BabyChew);
			SoundManager.Instance.listStopSoundOnExit.Add( SoundManager.Instance.BabyCry);

		}
	}

	public void  AnimHappyCompleted()
	{
		anim.SetInteger("Happy", Random.Range(1,4));
		anim.speed = Random.Range(0.8f,1.2f);
	}

	public void  AnimCryingCompleted()
	{

		anim.SetInteger("Crying", Random.Range(1,4));
		anim.speed = Random.Range(0.8f,1.2f);
	}
	
	public void BabyIdle()
	{
		CancelInvoke();
		anim.SetBool("bCrying",false);
		anim.SetInteger("Crying", 0);
		anim.SetBool("bEat",false);
		 
		anim.ResetTrigger("tSmile");

		switch(Random.Range(1,4))
		{
		case 1: anim.Play ("BabyHappyIdle",-1,0); anim.SetInteger("Happy",1);break;
		case 2: anim.Play ("BabyHappyIdle2",-1,0); anim.SetInteger("Happy",2);break;
		case 3: anim.Play ("BabyHappyIdle3",-1,0); anim.SetInteger("Happy",3);break;
		}
		 
		anim.speed = Random.Range(0.9f,1.2f);
	}

	public void BabySmile()
	{
		CancelInvoke();
		anim.SetBool("bCrying",false);
		anim.SetInteger("Crying", 0);
		anim.SetBool("bEat",false);
		anim.SetInteger("Happy",2);
		 anim.SetTrigger("tSmile");
		//anim.Play("BabyHappySmile",-1,0); 

	}

	public void BabyWaitToEat()
	{
		CancelInvoke();
		anim.Play("BabyWaitEatAnimation",-1,0);
		anim.SetBool("bEat",true);
		anim.SetInteger("Happy",0);

		 
	}

	public void BabyEat()
	{
		CancelInvoke();
		anim.Play("BabyEatAnimation",-1,0);
		anim.SetBool("bEat",true);
		if( SoundManager.Instance!=null) SoundManager.Instance.StopAndPlay_Sound( SoundManager.Instance.BabyChew,.1f); 
	}

	public void BabyCryingIdle()
	{
 
		anim.SetInteger("Happy",0);
		 
		anim.SetBool("bCrying",true);
		switch(Random.Range(1,4))
		{
		case 1: anim.Play ("BabyCryingIdle",-1,0);break;
		case 2: anim.Play ("BabyCryingIdle2",-1,0);break;
		case 3: anim.Play ("BabyCryingIdle3",-1,0);break;
		}
		anim.speed = Random.Range(0.9f,1.2f);
		CancelInvoke();
		Invoke(nameof(BabyCryOpenMouth), Random.Range(1.5f,4f));
	}

	public void BabyCryOpenMouth()
	{
		anim.SetInteger("Happy",0);
		anim.SetBool("bCrying",true);
		anim.SetTrigger("tCryOpenMouth");
		CancelInvoke();
		Invoke("BabyCryOpenMouth", Random.Range(4.5f,12f));

		if( SoundManager.Instance!=null) SoundManager.Instance.StopAndPlay_Sound( SoundManager.Instance.BabyCry,.1f); 
	}

	public void BabySleepy()
	{
		anim.SetInteger("Happy",0);
		anim.SetInteger("Crying", 0);
		anim.SetBool("bCrying",false);
		anim.SetBool("bEat",false);
		anim.ResetTrigger("tCryOpenMouth");
		anim.Play("Sleepy",-1,0);
		CancelInvoke();
	 
	}

	public void BabySleepy2()
	{
		anim.SetInteger("Happy",0);
		anim.SetInteger("Crying", 0);
		anim.SetBool("bCrying",false);
		anim.SetBool("bEat",false);
		anim.ResetTrigger("tCryOpenMouth");
		anim.Play("Sleepy2",-1,0);
		CancelInvoke();
	}

	public void BabySleeping()
	{
		anim.SetInteger("Happy",0);
		anim.SetInteger("Crying", 0);
		anim.SetBool("bCrying",false);
		anim.SetBool("bEat",false);
		anim.ResetTrigger("tCryOpenMouth");
		anim.Play("Sleeping",-1,0);
		CancelInvoke();
	}

	public void BabyBath()
	{
		anim.SetInteger("Happy",0);
		anim.SetInteger("Crying", 0);
		anim.SetBool("bCrying",false);
		anim.SetBool("bEat",false);
		anim.ResetTrigger("tCryOpenMouth");
		anim.Play("BabyBath",-1,0);
		CancelInvoke();
	}



	public void BabyWaitToBrushTeeth()
	{
		CancelInvoke();
		anim.Play("BrushTeethWait",-1,0);
		anim.SetBool("bBrushingTeeth",false);
		anim.SetInteger("Happy",0);
		 
		
	}
	
	public void BabyBrushTeeth(bool bBrushingTeeth)
	{
		 
		anim.SetBool("bBrushingTeeth",bBrushingTeeth);
		
	}




}
