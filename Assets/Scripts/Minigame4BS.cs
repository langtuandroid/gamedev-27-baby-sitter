using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TemplateScripts;
using UnityEngine.Serialization;

public class Minigame4BS : MonoBehaviour {

	private int selectedMenuU = 1;
	
	[FormerlySerializedAs("topMenu")] [SerializeField] private TopMenuBS topMenuBs;
	[SerializeField] private Animator animBasket;
	[SerializeField] private Animator animRightMenu;
	[SerializeField] private ProgressBar progressBar;
	
	public static int CompletedActionNoN = 0;
	
	[FormerlySerializedAs("ButtonNext")] [SerializeField] private GameObject buttonNext;
	[FormerlySerializedAs("ButtonHome")] [SerializeField] private GameObject buttonHome;

	private readonly bool bCreamM = false;
	
	[SerializeField] private Image imgCream;
	[SerializeField] private Sprite spriteCream;
	[SerializeField] private BabyControllerBs babyC;

	[FormerlySerializedAs("SpotsHolderHead")] [SerializeField] private Transform spotsHolderHead;
	[FormerlySerializedAs("SpotsHolderBody")] [SerializeField] private Transform spotsHolderBody;
	[FormerlySerializedAs("CremeSpots")] [SerializeField] private Image[] cremeSpots;
	[FormerlySerializedAs("RubCream")] [SerializeField] private ItemActionBS rubCream;

	[FormerlySerializedAs("psLevelCompleted")] public ParticleSystem psLevelCompletedD;

	private void Awake()
	{
		buttonNext.SetActive(false);
	}
	
	private IEnumerator Start () {
		CompletedActionNoN = 0;
		
		
		BlockClicksBs.Instance.SetBlockAllL(true);
		BlockClicksBs.Instance.SetBlockAllDelayY(.2f,false);

		animBasket.Play("ShowRightMenu");
		
		yield return new WaitForSeconds(.1f);

		babyC.GetComponent<SetBabyAtlasBS>().SetBabyY(GameDataBS.GetSelectedBabyY());
		babyC.BBabyCryingIdle();

		yield return new WaitForSeconds(1);
		LevelTransitionBS.Instance.ShowSceneE();

		TutorialBS.Instance.ShowTutorial(0);
	}
	
	public void DirtyClothesInBasketT(int itemNo)
	{
		CompletedActionNoN++;
		progressBar.SetProgressBar(CompletedActionNoN/6f , true );
		if(CompletedActionNoN == 1)
		{
			selectedMenuU = 1;
			topMenuBs.SetMenuItemsS(1);
			topMenuBs.ShowTopMenuU();

			animBasket.Play("HideRightMenu");
		}
		ShowTutorial();
	}

	private void ShowTutorial()
	{
		if(CompletedActionNoN == 1 ) TutorialBS.Instance.ShowTutorial(1);
		else if( CompletedActionNoN == 2 ) TutorialBS.Instance.ShowTutorial(2);
		else if( CompletedActionNoN == 3 ) TutorialBS.Instance.ShowTutorial(3);
		else if( CompletedActionNoN == 4 ) TutorialBS.Instance.ShowTutorial(4);
		else if( CompletedActionNoN == 5 ) TutorialBS.Instance.ShowTutorial(5);
	}

	public void CompletedActionN()
	{
	 
		CompletedActionNoN++;
		progressBar.SetProgressBar(CompletedActionNoN/6f , true );

		if(CompletedActionNoN == 2)
		{
			 
			animRightMenu.Play("ShowRightMenu");
		}

		if(CompletedActionNoN == 4)
		{
			 
			StartCoroutine(nameof(ShowCreamSpotsS));
		}


		if(CompletedActionNoN == 6)
		{

			if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.PlaySound( SoundManagerBS.Instance.MinigameCompleted);
			StartCoroutine(nameof(LevelCompletedD));
		}
		else 
		{
			ShowTutorial();
			if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.PlaySound( SoundManagerBS.Instance.ActionCompleted);
		}
	}

	public void NextPhaseE( string phaseState )
	{
		StartCoroutine(nameof(WaitNextPhaseE),phaseState);
	}
	
	private IEnumerator WaitNextPhaseE( string phaseState )
	{
		if(phaseState == "Cream" && !bCreamM)
		{
			imgCream.sprite = spriteCream;
			CompletedActionN();
		}

		if(phaseState == "TissueClean" )
		{

			imgCream.GetComponent<ItemActionBS>().enabled = true;
			imgCream.GetComponent<ItemActionBS>().bEnabled = true;
			CompletedActionN();

		}

		if(phaseState == "RubCream")
		{
			rubCream.bEnabled = false;
			rubCream.gameObject.SetActive(false);

			StartCoroutine(nameof(HideCreamSpotsS));
			CompletedActionN();

			babyC.BBabyWaitToBrushTeeth();
		}
		
		yield return new WaitForEndOfFrame();
	}

	private IEnumerator ShowCreamSpotsS()
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

	private IEnumerator HideCreamSpotsS()
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
		
	}
	
	private IEnumerator LevelCompletedD()
	{
		GameDataBS.BCompletedMiniGameE = true;
		psLevelCompletedD.gameObject.SetActive(true);
		yield return new WaitForEndOfFrame();
		psLevelCompletedD.Stop();
		psLevelCompletedD.Play();
		
		yield return new WaitForSeconds(1);
		buttonNext.SetActive(true);
		buttonHome.SetActive(false);
		yield return new WaitForSeconds(2);
		 babyC.BBabySmile();
	}
	
	public void ButtonHomeClickedD( )
	{
		CompletedActionNoN = 0;
		if(SoundManagerBS.Instance!=null) SoundManagerBS.Instance.Play_ButtonClickK();
		StopAllCoroutines();
		GameDataBS.BCompletedMiniGameE = false;
		LevelTransitionBS.Instance.HideSceneAndLoadNextT("SelectMinigame"); 
		
		BlockClicksBs.Instance.SetBlockAllL(true);
	}

	public void ButtonNextClickedD()
	{
		CompletedActionNoN = 0;
		BlockClicksBs.Instance.SetBlockAllL(true);
		if(SoundManagerBS.Instance!=null) SoundManagerBS.Instance.Play_ButtonClickK();
		LevelTransitionBS.Instance.HideSceneAndLoadNextT("SelectMinigame");
		StopAllCoroutines();
	}

}
