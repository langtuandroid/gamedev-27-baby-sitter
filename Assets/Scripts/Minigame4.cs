using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TemplateScripts;
using UnityEngine.Serialization;

public class Minigame4 : MonoBehaviour {

	private int selectedMenu = 1;
	
	[SerializeField] private TopMenu topMenu;
	[SerializeField] private Animator animBasket;
	[SerializeField] private Animator animRightMenu;
	[SerializeField] private ProgressBar progressBar;
	
	public static int CompletedActionNo = 0;
	
	[FormerlySerializedAs("ButtonNext")] [SerializeField] private GameObject buttonNext;
	[FormerlySerializedAs("ButtonHome")] [SerializeField] private GameObject buttonHome;

	private readonly bool bCream = false;
	
	[SerializeField] private Image imgCream;
	[SerializeField] private Sprite spriteCream;
	[SerializeField] private BabyController babyC;

	[FormerlySerializedAs("SpotsHolderHead")] [SerializeField] private Transform spotsHolderHead;
	[FormerlySerializedAs("SpotsHolderBody")] [SerializeField] private Transform spotsHolderBody;
	[FormerlySerializedAs("CremeSpots")] [SerializeField] private Image[] cremeSpots;
	[FormerlySerializedAs("RubCream")] [SerializeField] private ItemAction rubCream;

	public ParticleSystem psLevelCompleted;

	private void Awake()
	{
		buttonNext.SetActive(false);
	}
	
	private IEnumerator Start () {
		
		//topMenu.SetMenuItems(1);
		CompletedActionNo = 0;
		
		
		BlockClicks.Instance.SetBlockAll(true);
		BlockClicks.Instance.SetBlockAllDelay(.2f,false);

		animBasket.Play("ShowRightMenu");

		//animRightMenu.Play("ShowRightMenu");
		yield return new WaitForSeconds(.1f);
 
		//podesi bebicu 
		//Debug.Log("Selektovana bebica:  " +  (GameData.selectedMinigameIndex+1) );
		babyC.GetComponent<SetBabyAtlas>().SetBaby(GameData.GetSelectedBaby());
		babyC.BabyCryingIdle();

		yield return new WaitForSeconds(1);
		LevelTransition.Instance.ShowScene();

		Tutorial.Instance.ShowTutorial(0);
	}
	
	public void DirtyClothesInBasket(int itemNo)
	{
		 
		CompletedActionNo++;
		progressBar.SetProgress(CompletedActionNo/6f , true );
		if(CompletedActionNo == 1)
		{
			selectedMenu = 1;
			topMenu.SetMenuItems(1);
			topMenu.ShowTopMenu();

			animBasket.Play("HideRightMenu");
		}
		ShowTut();
	}

	private void ShowTut()
	{
		if(CompletedActionNo == 1 ) Tutorial.Instance.ShowTutorial(1);
		else if( CompletedActionNo == 2 ) Tutorial.Instance.ShowTutorial(2);
		else if( CompletedActionNo == 3 ) Tutorial.Instance.ShowTutorial(3);
		else if( CompletedActionNo == 4 ) Tutorial.Instance.ShowTutorial(4);
		else if( CompletedActionNo == 5 ) Tutorial.Instance.ShowTutorial(5);
	}

	public void CompletedAction()
	{
	 
		CompletedActionNo++;
		progressBar.SetProgress(CompletedActionNo/6f , true );

		if(CompletedActionNo == 2)
		{
			 
			animRightMenu.Play("ShowRightMenu");
		}

		if(CompletedActionNo == 4)
		{
			 
			StartCoroutine(nameof(ShowCreamSpots));
		}


		if(CompletedActionNo == 6)
		{

			if( SoundManager.Instance!=null)  SoundManager.Instance.Play_Sound( SoundManager.Instance.MinigameCompleted);
			StartCoroutine(nameof(LevelCompleted));
		}
		else 
		{
			ShowTut();
			if( SoundManager.Instance!=null)  SoundManager.Instance.Play_Sound( SoundManager.Instance.ActionCompleted);
		}
	}

	public void NextPhase( string phaseState )
	{
		StartCoroutine(nameof(WaitNextPhase),phaseState);
	}
	
	private IEnumerator WaitNextPhase( string phaseState )
	{
		if(phaseState == "Cream" && !bCream)
		{
			imgCream.sprite = spriteCream;
			CompletedAction();
		}

		if(phaseState == "TissueClean" )
		{

			imgCream.GetComponent<ItemAction>().enabled = true;
			imgCream.GetComponent<ItemAction>().bEnabled = true;
			CompletedAction();

		}

		if(phaseState == "RubCream")
		{
			rubCream.bEnabled = false;
			rubCream.gameObject.SetActive(false);

			StartCoroutine(nameof(HideCreamSpots));
			CompletedAction();
			//babyC.BabySmile();

			babyC.BabyWaitToBrushTeeth();
		}

//		else if(phaseState == "GivePacifier" && !bPacifier)
//		{
//			bPacifier = true;
//			//Debug.Log("GivePacifier");
//			phase ++;
//			CompletedAction();
//		}
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
		yield return new WaitForSeconds(2);
		 babyC.BabySmile();
	}
	
	public void ButtonHomeClicked( )
	{
		CompletedActionNo = 0;
		if(SoundManager.Instance!=null) SoundManager.Instance.Play_ButtonClick();
		StopAllCoroutines();
		GameData.BCompletedMiniGame = false;
		LevelTransition.Instance.HideSceneAndLoadNext("SelectMinigame"); 
		
		BlockClicks.Instance.SetBlockAll(true);
		//Implementation.Instance.ShowInterstitial();
	}

	public void  ButtonNextClicked()
	{
		CompletedActionNo = 0;
		BlockClicks.Instance.SetBlockAll(true);
		if(SoundManager.Instance!=null) SoundManager.Instance.Play_ButtonClick();
		LevelTransition.Instance.HideSceneAndLoadNext("SelectMinigame");
		StopAllCoroutines();
		//Implementation.Instance.ShowInterstitial();
	}

}
