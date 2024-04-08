using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TemplateScripts;
using UnityEngine.Serialization;

public class Minigame7BS : MonoBehaviour {
	
	[SerializeField] private GameObject prefMosquito;

	private int mosquitoCountT;
	private int mosquitoHitsS = 0; 
	
	[SerializeField] private ProgressBar progressBar;
	
	public static int CompletedActionNoN = 0;
	
	[FormerlySerializedAs("ButtonNext")] [SerializeField] private GameObject buttonNext;
	[FormerlySerializedAs("ButtonHome")] [SerializeField] private GameObject buttonHome;

	private int phaseS = 0;
	[SerializeField] private MosquitoStick mt;
	 
	[FormerlySerializedAs("MosquitosHolder")] [SerializeField] private Transform mosquitosHolder;

	[FormerlySerializedAs("Curtains1")] [SerializeField] private Transform curtains1;
	[FormerlySerializedAs("Curtains2")] [SerializeField] private Transform curtains2;

	[FormerlySerializedAs("LampHandle")] [SerializeField] private ItemActionBS lampHandle;
	[FormerlySerializedAs("Krema")] [SerializeField] private ItemActionBS krema;
	
	[SerializeField] private Animator animKrema;

	[SerializeField] private BabyControllerBs babyC;

	[SerializeField] private Transform spotsHolderHead;
	[SerializeField] private Transform spotsHolderBody;
	
	private Image[] cremeSpotsS;
	
	[FormerlySerializedAs("RubCream")] [SerializeField] private ItemActionBS rubCream;
	[SerializeField] private GameObject psSleeping;
	[SerializeField] private ParticleSystem psLevelCompleted;

	private void Awake()
	{
		MosquitoMove.ActiveSoundsCountT = 0;
		buttonNext.SetActive(false);
		InvokeRepeating(nameof(CreateMosquitoS),5,4);
	}
	
	private IEnumerator Start () {
		CompletedActionNoN = 0;
		
		BlockClicksBs.Instance.SetBlockAllL(true);
		BlockClicksBs.Instance.SetBlockAllDelayY(.2f,false);
		

		yield return new WaitForSeconds(.1f);
		
		babyC.GetComponent<SetBabyAtlasBS>().SetBabyY(GameDataBS.GetSelectedBabyY());
		babyC.BBabyCryingIdle();

		mosquitoCountT = mosquitosHolder.childCount;
		yield return new WaitForSeconds(1);
		LevelTransitionBS.Instance.ShowSceneE();

		yield return new WaitForSeconds(1);

		mt.bEnabled = true;

		TutorialBS.Instance.ShowTutorial(0);
	}


	private void CreateMosquitoS()
	{
		GameObject go;
		if(mosquitoCountT <12)
		{
			go = GameObject.Instantiate(prefMosquito);
			go.transform.parent = mosquitosHolder;
			go.transform.localScale = Vector3.one;
			mosquitoCountT++;
		}

		if(mosquitoCountT <12)
		{
			go = GameObject.Instantiate(prefMosquito);
			go.transform.parent = mosquitosHolder;
			go.transform.localScale = Vector3.one;
			mosquitoCountT++;
		}

		if(mosquitoCountT >=12) 
		{
			CancelInvoke(nameof(CreateMosquitoS)); 
		}
	}

	private void ShowTutorial()
	{
		if(CompletedActionNoN == 1 ) TutorialBS.Instance.ShowTutorial(1);
		else if( CompletedActionNoN == 2 ) TutorialBS.Instance.ShowTutorial(2);
		else if( CompletedActionNoN == 3 ) TutorialBS.Instance.ShowTutorial(3);
		else if( CompletedActionNoN == 4 ) TutorialBS.Instance.ShowTutorial(4);
		else if( CompletedActionNoN == 5 ) TutorialBS.Instance.ShowTutorial(5);
	}


	public void NextPhaseE( string phaseState )
	{
		StartCoroutine(nameof(WaitNextPhaseE),phaseState);
	}
	
	private IEnumerator WaitNextPhaseE( string phaseState )
	{
//		Debug.Log(phaseState +"   @");
		if(phaseState == "MosquitoHit"  )
		{
			mosquitoHitsS++;
			 
			if(mosquitoHitsS == 12)
			{
				phaseS ++;
				CompletedActionN();

				yield return new WaitForSeconds(1);
				mt.HideE();
				yield return new WaitForSeconds(1);
				GameObject.Destroy(mt.gameObject );
				krema.bEnabled = true;
				animKrema.Play("show");
				ShowTutorial();
			}
				
		}
		else if(phaseState == "Krema"  )
		{
			StartCoroutine(nameof(ShowCreamSpotsS));
			yield return new WaitForSeconds(1);
			animKrema.Play("hide");
			krema.bEnabled = false;
			yield return new WaitForSeconds(1);
			GameObject.Destroy (krema.gameObject);

			CompletedActionN();
			ShowTutorial();
		}

		else if(phaseState == "RubCream")
		{
			rubCream.bEnabled = false;
			rubCream.gameObject.SetActive(false);
			
			StartCoroutine(nameof(HideCreamSpotsS));
			CompletedActionN();
			babyC.BBabySmile();
			ShowTutorial();
		}

		else if(phaseState == "Curtains"  )
		{
			babyC.BBabySleepy();
			phaseS ++;
			yield return new WaitForSeconds(1);
			CompletedActionN();
			GameObject.Destroy (curtains1.parent.GetComponent<ItemActionBS>());

			yield return new WaitForSeconds(1);
			curtains1.parent.gameObject. AddComponent<GraphicIgnoreRaycastBS>()  ;
			lampHandle.bEnabled = true;
			ShowTutorial();
		}
		else if(phaseState == "LampHandle"  )
		{
			babyC.BBabySleeping();
			psSleeping.SetActive(true);
			lampHandle.bEnabled = false;
			lampHandle.enabled = false;
			phaseS ++;
			CompletedActionN();
			yield return new WaitForSeconds(1);
			 

		}
			
		
		yield return new WaitForEndOfFrame();
	}

	private IEnumerator ShowCreamSpotsS()
	{
		yield return new WaitForFixedUpdate();
		if(cremeSpotsS == null || cremeSpotsS.Length == 0)
		{
			
			cremeSpotsS = new Image[spotsHolderHead.childCount + spotsHolderBody.childCount];
			for(int i = 0; i<spotsHolderHead.childCount; i++)
			{
				cremeSpotsS[i] = spotsHolderHead.GetChild(i).GetChild(0).GetComponent<Image>();
			}
			
			for(int i = 0; i<spotsHolderBody.childCount; i++)
			{
				
				cremeSpotsS[ i + spotsHolderHead.childCount ] = spotsHolderBody.GetChild(i).GetChild(0).GetComponent<Image>();
			}
			
		}
		
		float pom = 0;
		while(pom <1)
		{
			pom +=Time.fixedDeltaTime * 2;
			Color c  = new Color(1,1,1,pom);
			for(int i = 0; i<cremeSpotsS.Length; i++)
			{
				cremeSpotsS[i].color = c;
			}
			yield return new WaitForFixedUpdate();
		}
		for(int i = 0; i<cremeSpotsS.Length; i++)
		{
			cremeSpotsS[i].transform.parent.GetComponent<Image>().enabled  = false;
		}
		rubCream.gameObject.SetActive(true);
		rubCream.bEnabled = true;
	}
	
	private IEnumerator HideCreamSpotsS()
	{
		
		float pom = 1;
		while(pom >0)
		{
			pom -=Time.fixedDeltaTime * 2;
			Color c  = new Color(1,1,1,pom);
			for(int i = 0; i<cremeSpotsS.Length; i++)
			{
				cremeSpotsS[i].color = c;
			}
			yield return new WaitForFixedUpdate();
		}
		
		for(int i = 0; i<cremeSpotsS.Length; i++)
		{
			cremeSpotsS[i].transform.parent.gameObject.SetActive(false);
		}

		Destroy (curtains1.GetComponent<GraphicIgnoreRaycastBS>() );
		Destroy (curtains2.GetComponent<GraphicIgnoreRaycastBS>() );
		
	}


	public void CompletedActionN()
	{
		CompletedActionNoN++;
		progressBar.SetProgressBar(CompletedActionNoN/5f , true );
		
		if(CompletedActionNoN == 5)
		{
			if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.PlaySound( SoundManagerBS.Instance.MinigameCompleted);
			StartCoroutine(nameof(LevelCompletedD));
		}
		else 
		{
			if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.PlaySound( SoundManagerBS.Instance.ActionCompleted);
		}
	}
	
	private IEnumerator LevelCompletedD()
	{
		//Debug.Log("Completed");
		GameDataBS.BCompletedMiniGameE = true;
		psLevelCompleted.gameObject.SetActive(true);
		yield return new WaitForEndOfFrame();
		psLevelCompleted.Stop();
		psLevelCompleted.Play();
		
		yield return new WaitForSeconds(1);
		buttonNext.SetActive(true);
		buttonHome.SetActive(false);
	}
	
	public void ButtonHomeClickedD( )
	{
		CompletedActionNoN = 0;
		psSleeping.GetComponent<ParticleSystem>().enableEmission = false;
		if(SoundManagerBS.Instance!=null) SoundManagerBS.Instance.Play_ButtonClickK();
		StopAllCoroutines();
		GameDataBS.BCompletedMiniGameE = false;
		LevelTransitionBS.Instance.HideSceneAndLoadNextT("SelectMinigame"); 
		BlockClicksBs.Instance.SetBlockAllL(true);
	}

	public void ButtonNextClickedD()
	{
		CompletedActionNoN = 0;
		psSleeping.GetComponent<ParticleSystem>().enableEmission = false;
		BlockClicksBs.Instance.SetBlockAllL(true);
		if(SoundManagerBS.Instance!=null) SoundManagerBS.Instance.Play_ButtonClickK();
		LevelTransitionBS.Instance.HideSceneAndLoadNextT("SelectMinigame");
		StopAllCoroutines();
	}
}