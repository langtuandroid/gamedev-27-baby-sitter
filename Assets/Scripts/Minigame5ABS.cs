using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TemplateScripts;
using UnityEngine.Serialization;

public class Minigame5ABS : MonoBehaviour {
	private int fruitCountT = 0; 
	public ProgressBar progressBar;
	
	private static int _completedActionNoN = 0;
	
	[FormerlySerializedAs("ButtonNext")] [SerializeField] private GameObject buttonNext;
	[FormerlySerializedAs("ButtonHome")] [SerializeField] private GameObject buttonHome;

	private int phaseE = 0;

	[SerializeField] private Animator animDish; 
	[SerializeField] private Animator animSpoon;
	[SerializeField] private Animator animBlender; 
	
	[FormerlySerializedAs("BlenderButton")] [SerializeField] private ItemActionBS blenderButton;
	[FormerlySerializedAs("BlenderTopPart")] [SerializeField] private DragItemBS blenderTopPart;
	
	[SerializeField] private CanvasGroup cgTissue; 

	[FormerlySerializedAs("DishFood")] [SerializeField] private Image dishFood;
	[FormerlySerializedAs("DishFoodEndPos")] [SerializeField] private Transform dishFoodEndPos;

	[FormerlySerializedAs("FruitChops")] [SerializeField] private Image[] fruitChops;
	[FormerlySerializedAs("SpriteFruitChops")] [SerializeField] private Sprite[] spriteFruitChops;

	[SerializeField] private BabyControllerBs babyC;
	[SerializeField] private ParticleSystem psLevelCompleted;

	private void Awake()
	{
		buttonNext.SetActive(false);
	}
	
	private IEnumerator Start() 
	{
		DragItemBS.OneItemEnabledNo = -1;
		_completedActionNoN = 0;
 
		BlockClicksBs.Instance.SetBlockAllL(true);
		BlockClicksBs.Instance.SetBlockAllDelayY(.2f,false);

		yield return new WaitForSeconds(.1f);
		
		if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.PlaySound( SoundManagerBS.Instance.ShowItemM);
		blenderButton.enabled = false;
		
		babyC.GetComponent<SetBabyAtlasBS>().SetBabyY(GameDataBS.GetSelectedBabyY());
		babyC.BBabyIdle();

		yield return new WaitForSeconds(1);
		LevelTransitionBS.Instance.ShowSceneE();
		animBlender.Play("showBlender");

		TutorialBS.Instance.ShowTutorial(0);
	}

	public void NextPhaseS( string phaseState )
	{
		StartCoroutine(nameof(WaitNextPhaseS),phaseState);
	}

	private IEnumerator WaitNextPhaseS( string phaseState )
	{
		if(phaseState == "FruitBlender"  )
		{
			fruitCountT++;
			if(fruitCountT == 3)
			{
				phaseE ++;
				CompletedActionN();
				yield return new WaitForSeconds(1);
				blenderButton.enabled = true;
			 
				ShowTutT();
			}
			else
			{
				TutorialBS.Instance.StopTutor();
			}
		}
		else if(phaseState == "BlenderButton" && phaseE==1)
		{
			 
			blenderButton.enabled = false;
			animBlender.Play("BlenderOn");
			TutorialBS.Instance.StopTutor();
			yield return new WaitForSeconds(4);

			phaseE ++;
			CompletedActionN();
			if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.PlaySound( SoundManagerBS.Instance.ShowItemM);
			animDish.Play("showDish");
			blenderTopPart.enabled = true;
			animBlender.enabled = false;
			ShowTutT();
		}

		else if(phaseState == "PourMash"  )
		{
			 
			dishFood.gameObject.SetActive( true);

			float pom = 0;
			Vector3 sPos = dishFood.transform.position;
			 
			
			while(pom<1)
			{
				pom+=Time.fixedDeltaTime;
				dishFood.transform.position = Vector3.Lerp(sPos, dishFoodEndPos.position, pom);
				dishFood.transform.localScale =  Vector3.one * (.8f+ .2f*pom ) ;
				yield return new WaitForFixedUpdate();
			}

			phaseE ++;
			CompletedActionN();

			yield return new WaitForSeconds(1.5f);
			blenderTopPart.enabled = false;
			fruitChops[0].enabled = false;
			fruitChops[1].enabled = false;
			fruitChops[2].enabled = false;
			animBlender.enabled = true;
			animBlender.Play("hideBlender");
			if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.PlaySound( SoundManagerBS.Instance.ShowItemM);
			GameObject.Destroy(animBlender.gameObject,2);

			yield return new WaitForSeconds(0.9f);
			//prikazi kasiku
			animSpoon.Play("showSpoon");
			if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.PlaySound( SoundManagerBS.Instance.ShowItemM);
			animSpoon.GetComponent<SpoonBS>().enabled = true;
			yield return new WaitForSeconds(.8f);
			animSpoon.StopPlayback();
			animSpoon.enabled = false;
			ShowTutT();
		}
		else  if(phaseState == "FeedBaby"  )
		{
			babyC.BBabyIdle();
			phaseE ++;
			CompletedActionN();
			animSpoon.enabled = true;

			animSpoon.Play("hideSpoon");
			Destroy(animSpoon.gameObject,1);
			 
			cgTissue.alpha = 0;
			cgTissue.gameObject.SetActive(true);
			while(cgTissue.alpha<.99f)
			{
				cgTissue.alpha+=Time.fixedDeltaTime*2;
				yield return new WaitForFixedUpdate();
			}
			cgTissue.alpha= 1;

			DragItemBS.OneItemEnabledNo = 4;
			ShowTutT();
		}

		else  if(phaseState == "CleanBaby"  )
		{
			TutorialBS.Instance.StopTutor();
			phaseE ++;
			CompletedActionN();
			 
			//hide tissue
			while(cgTissue.alpha>0.05f)
			{
				cgTissue.alpha-=Time.fixedDeltaTime*2;
				yield return new WaitForFixedUpdate();
			}
			cgTissue.gameObject.SetActive(false);
		}

 
		yield return new WaitForEndOfFrame();
	}


	public void ShowChoppedFruitBlenderR(int fruitNo)
	{
		fruitChops[ fruitCountT-1].enabled = true;
		fruitChops[ fruitCountT-1].sprite = spriteFruitChops[(fruitNo-1)];
	}

	private void ShowTutT()
	{
		if(_completedActionNoN == 1 ) TutorialBS.Instance.ShowTutorial(1);
		else if( _completedActionNoN == 2 ) TutorialBS.Instance.ShowTutorial(2);
		else if( _completedActionNoN == 3 ) TutorialBS.Instance.ShowTutorial(3);
		else if( _completedActionNoN == 4 ) TutorialBS.Instance.ShowTutorial(4);
 
	}
 
	public void CompletedActionN()
	{
		_completedActionNoN++;
		progressBar.SetProgressBar(_completedActionNoN/5f , true );
		
		if(_completedActionNoN == 5)
		{
			if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.PlaySound( SoundManagerBS.Instance.MinigameCompleted);
			StartCoroutine(nameof(LevelCompletedD));
			babyC.BBabySmile();
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
		_completedActionNoN = 0;
		if(SoundManagerBS.Instance!=null) SoundManagerBS.Instance.Play_ButtonClickK();
		StopAllCoroutines();
		GameDataBS.BCompletedMiniGameE = false;
		LevelTransitionBS.Instance.HideSceneAndLoadNextT("SelectMinigame"); 
		BlockClicksBs.Instance.SetBlockAllL(true);
		//Implementation.Instance.ShowInterstitial();
	}

	public void ButtonNextClickedD()
	{
		_completedActionNoN = 0;
		if(SoundManagerBS.Instance!=null) SoundManagerBS.Instance.Play_ButtonClickK();
		BlockClicksBs.Instance.SetBlockAllL(true);
		LevelTransitionBS.Instance.HideSceneAndLoadNextT("SelectMinigame");
		StopAllCoroutines();
		//Implementation.Instance.ShowInterstitial();
	}
}
