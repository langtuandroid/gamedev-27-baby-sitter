using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TemplateScripts;
using UnityEngine.Serialization;

public class Minigame7 : MonoBehaviour {
	
	[SerializeField] private GameObject prefMosquito;

	private int mosquitoCount;
	private int mosquitoHits = 0; 
	
	[SerializeField] private ProgressBar progressBar;
	
	public static int CompletedActionNo = 0;
	
	[FormerlySerializedAs("ButtonNext")] [SerializeField] private GameObject buttonNext;
	[FormerlySerializedAs("ButtonHome")] [SerializeField] private GameObject buttonHome;

	private int phase = 0;
	[SerializeField] private MosquitoStick mt;
	 
	[FormerlySerializedAs("MosquitosHolder")] [SerializeField] private Transform mosquitosHolder;

	[FormerlySerializedAs("Curtains1")] [SerializeField] private Transform curtains1;
	[FormerlySerializedAs("Curtains2")] [SerializeField] private Transform curtains2;

	[FormerlySerializedAs("LampHandle")] [SerializeField] private ItemAction lampHandle;
	[FormerlySerializedAs("Krema")] [SerializeField] private ItemAction krema;
	
	[SerializeField] private Animator animKrema;

	[SerializeField] private BabyController babyC;

	[SerializeField] private Transform spotsHolderHead;
	[SerializeField] private Transform spotsHolderBody;
	
	private Image[] cremeSpots;
	
	[FormerlySerializedAs("RubCream")] [SerializeField] private ItemAction rubCream;
	[SerializeField] private GameObject psSleeping;
	[SerializeField] private ParticleSystem psLevelCompleted;

	private void Awake()
	{
		MosquitoMove.ActiveSoundsCount = 0;
		buttonNext.SetActive(false);
		InvokeRepeating(nameof(CreateMosquito),5,4);
	}
	
	private IEnumerator Start () {
		CompletedActionNo = 0;
		
		BlockClicks.Instance.SetBlockAll(true);
		BlockClicks.Instance.SetBlockAllDelay(.2f,false);
		

		yield return new WaitForSeconds(.1f);
		
		babyC.GetComponent<SetBabyAtlas>().SetBaby(GameData.GetSelectedBaby());
		babyC.BabyCryingIdle();

		mosquitoCount = mosquitosHolder.childCount;
		yield return new WaitForSeconds(1);
		LevelTransition.Instance.ShowScene();

		yield return new WaitForSeconds(1);

		mt.bEnabled = true;

		Tutorial.Instance.ShowTutorial(0);
	}


	private void CreateMosquito()
	{
		GameObject go;
		if(mosquitoCount <12)
		{
			go = GameObject.Instantiate(prefMosquito);
			go.transform.parent = mosquitosHolder;
			go.transform.localScale = Vector3.one;
			mosquitoCount++;
		}

		if(mosquitoCount <12)
		{
			go = GameObject.Instantiate(prefMosquito);
			go.transform.parent = mosquitosHolder;
			go.transform.localScale = Vector3.one;
			mosquitoCount++;
		}

		if(mosquitoCount >=12) 
		{
			CancelInvoke(nameof(CreateMosquito)); 
		}
	}

	private void ShowTut()
	{
		if(CompletedActionNo == 1 ) Tutorial.Instance.ShowTutorial(1);
		else if( CompletedActionNo == 2 ) Tutorial.Instance.ShowTutorial(2);
		else if( CompletedActionNo == 3 ) Tutorial.Instance.ShowTutorial(3);
		else if( CompletedActionNo == 4 ) Tutorial.Instance.ShowTutorial(4);
		else if( CompletedActionNo == 5 ) Tutorial.Instance.ShowTutorial(5);
	}


	public void NextPhase( string phaseState )
	{
		StartCoroutine(nameof(WaitNextPhase),phaseState);
	}
	
	private IEnumerator WaitNextPhase( string phaseState )
	{
//		Debug.Log(phaseState +"   @");
		if(phaseState == "MosquitoHit"  )
		{
			mosquitoHits++;
			 
			if(mosquitoHits == 12)
			{
				phase ++;
				CompletedAction();

				yield return new WaitForSeconds(1);
				mt.Hide();
				yield return new WaitForSeconds(1);
				GameObject.Destroy(mt.gameObject );
				krema.bEnabled = true;
				animKrema.Play("show");
				ShowTut();
			}
				
		}
		else if(phaseState == "Krema"  )
		{
			StartCoroutine(nameof(ShowCreamSpots));
			yield return new WaitForSeconds(1);
			animKrema.Play("hide");
			krema.bEnabled = false;
			yield return new WaitForSeconds(1);
			GameObject.Destroy (krema.gameObject);

			CompletedAction();
			ShowTut();
		}

		else if(phaseState == "RubCream")
		{
			rubCream.bEnabled = false;
			rubCream.gameObject.SetActive(false);
			
			StartCoroutine(nameof(HideCreamSpots));
			CompletedAction();
			babyC.BabySmile();
			ShowTut();
		}

		else if(phaseState == "Curtains"  )
		{
			babyC.BabySleepy();
			phase ++;
			yield return new WaitForSeconds(1);
			CompletedAction();
			GameObject.Destroy (curtains1.parent.GetComponent<ItemAction>());

			yield return new WaitForSeconds(1);
			curtains1.parent.gameObject. AddComponent<GraphicIgnoreRaycast>()  ;
			lampHandle.bEnabled = true;
			ShowTut();
		}
		else if(phaseState == "LampHandle"  )
		{
			babyC.BabySleeping();
			psSleeping.SetActive(true);
			lampHandle.bEnabled = false;
			lampHandle.enabled = false;
			phase ++;
			CompletedAction();
			yield return new WaitForSeconds(1);
			 

		}
			
		
		yield return new WaitForEndOfFrame();
	}

	private IEnumerator ShowCreamSpots()
	{
		yield return new WaitForFixedUpdate();
		if(cremeSpots == null || cremeSpots.Length == 0)
		{
			
			cremeSpots = new Image[spotsHolderHead.childCount + spotsHolderBody.childCount];
			for(int i = 0; i<spotsHolderHead.childCount; i++)
			{
				cremeSpots[i] = spotsHolderHead.GetChild(i).GetChild(0).GetComponent<Image>();
			}
			
			for(int i = 0; i<spotsHolderBody.childCount; i++)
			{
				
				cremeSpots[ i + spotsHolderHead.childCount ] = spotsHolderBody.GetChild(i).GetChild(0).GetComponent<Image>();
			}
			
		}
		
		float pom = 0;
		while(pom <1)
		{
			pom +=Time.fixedDeltaTime * 2;
			Color c  = new Color(1,1,1,pom);
			for(int i = 0; i<cremeSpots.Length; i++)
			{
				cremeSpots[i].color = c;
			}
			yield return new WaitForFixedUpdate();
		}
		for(int i = 0; i<cremeSpots.Length; i++)
		{
			cremeSpots[i].transform.parent.GetComponent<Image>().enabled  = false;
		}
		rubCream.gameObject.SetActive(true);
		rubCream.bEnabled = true;
	}
	
	private IEnumerator HideCreamSpots()
	{
		
		float pom = 1;
		while(pom >0)
		{
			pom -=Time.fixedDeltaTime * 2;
			Color c  = new Color(1,1,1,pom);
			for(int i = 0; i<cremeSpots.Length; i++)
			{
				cremeSpots[i].color = c;
			}
			yield return new WaitForFixedUpdate();
		}
		
		for(int i = 0; i<cremeSpots.Length; i++)
		{
			cremeSpots[i].transform.parent.gameObject.SetActive(false);
		}

		Destroy (curtains1.GetComponent<GraphicIgnoreRaycast>() );
		Destroy (curtains2.GetComponent<GraphicIgnoreRaycast>() );
		
	}


	public void CompletedAction()
	{
		CompletedActionNo++;
		progressBar.SetProgress(CompletedActionNo/5f , true );
		
		if(CompletedActionNo == 5)
		{
			if( SoundManager.Instance!=null)  SoundManager.Instance.Play_Sound( SoundManager.Instance.MinigameCompleted);
			StartCoroutine(nameof(LevelCompleted));
		}
		else 
		{
			if( SoundManager.Instance!=null)  SoundManager.Instance.Play_Sound( SoundManager.Instance.ActionCompleted);
		}
	}
	
	private IEnumerator LevelCompleted()
	{
		//Debug.Log("Completed");
		GameData.BCompletedMiniGame = true;
		psLevelCompleted.gameObject.SetActive(true);
		yield return new WaitForEndOfFrame();
		psLevelCompleted.Stop();
		psLevelCompleted.Play();
		
		yield return new WaitForSeconds(1);
		buttonNext.SetActive(true);
		buttonHome.SetActive(false);
	}
	
	public void ButtonHomeClicked( )
	{
		CompletedActionNo = 0;
		psSleeping.GetComponent<ParticleSystem>().enableEmission = false;
		if(SoundManager.Instance!=null) SoundManager.Instance.Play_ButtonClick();
		StopAllCoroutines();
		GameData.BCompletedMiniGame = false;
		LevelTransition.Instance.HideSceneAndLoadNext("SelectMinigame"); 
		BlockClicks.Instance.SetBlockAll(true);
	}

	public void  ButtonNextClicked()
	{
		CompletedActionNo = 0;
		psSleeping.GetComponent<ParticleSystem>().enableEmission = false;
		BlockClicks.Instance.SetBlockAll(true);
		if(SoundManager.Instance!=null) SoundManager.Instance.Play_ButtonClick();
		LevelTransition.Instance.HideSceneAndLoadNext("SelectMinigame");
		StopAllCoroutines();
	}
}