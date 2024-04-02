using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TemplateScripts;
using UnityEngine.Serialization;

public class Minigame5A : MonoBehaviour {
	private int fruitCount = 0; 
	public ProgressBar progressBar;
	
	private static int _completedActionNo = 0;
	
	[FormerlySerializedAs("ButtonNext")] [SerializeField] private GameObject buttonNext;
	[FormerlySerializedAs("ButtonHome")] [SerializeField] private GameObject buttonHome;

	private int phase = 0;

	[SerializeField] private Animator animDish; 
	[SerializeField] private Animator animSpoon;
	[SerializeField] private Animator animBlender; 
	
	[FormerlySerializedAs("BlenderButton")] [SerializeField] private ItemAction blenderButton;
	[FormerlySerializedAs("BlenderTopPart")] [SerializeField] private DragItem blenderTopPart;
	
	[SerializeField] private CanvasGroup cgTissue; 

	[FormerlySerializedAs("DishFood")] [SerializeField] private Image dishFood;
	[FormerlySerializedAs("DishFoodEndPos")] [SerializeField] private Transform dishFoodEndPos;

	[FormerlySerializedAs("FruitChops")] [SerializeField] private Image[] fruitChops;
	[FormerlySerializedAs("SpriteFruitChops")] [SerializeField] private Sprite[] spriteFruitChops;

	[SerializeField] private BabyController babyC;
	[SerializeField] private ParticleSystem psLevelCompleted;

	private void Awake()
	{
		buttonNext.SetActive(false);
	}
	
	private IEnumerator Start () 
	{
		DragItem.OneItemEnabledNo = -1;
		_completedActionNo = 0;
 
		BlockClicks.Instance.SetBlockAll(true);
		BlockClicks.Instance.SetBlockAllDelay(.2f,false);

		yield return new WaitForSeconds(.1f);
		
		if( SoundManager.Instance!=null)  SoundManager.Instance.Play_Sound( SoundManager.Instance.ShowItem);
		blenderButton.enabled = false;

		//podesi bebicu 
		//Debug.Log("Selektovana bebica:  " +  (GameData.selectedMinigameIndex+1) );
		babyC.GetComponent<SetBabyAtlas>().SetBaby(GameData.GetSelectedBaby());
		babyC.BabyIdle();

		yield return new WaitForSeconds(1);
		LevelTransition.Instance.ShowScene();
		animBlender.Play("showBlender");

		Tutorial.Instance.ShowTutorial(0);
	}

	public void NextPhase( string phaseState )
	{
		StartCoroutine(nameof(WaitNextPhase),phaseState);
	}

	private IEnumerator WaitNextPhase( string phaseState )
	{
		//Debug.Log(phaseState +"   @");
		if(phaseState == "FruitBlender"  )
		{
			fruitCount++;
			if(fruitCount == 3)
			{
				phase ++;
				CompletedAction();
				yield return new WaitForSeconds(1);
				blenderButton.enabled = true;
			 
				ShowTut();
			}
			else
			{
				Tutorial.Instance.StopTutorial();
			}
		}
		else if(phaseState == "BlenderButton" && phase==1)
		{
			 
			blenderButton.enabled = false;
			animBlender.Play("BlenderOn");
			Tutorial.Instance.StopTutorial();
			yield return new WaitForSeconds(4);

			phase ++;
			CompletedAction();
			if( SoundManager.Instance!=null)  SoundManager.Instance.Play_Sound( SoundManager.Instance.ShowItem);
			animDish.Play("showDish");
			blenderTopPart.enabled = true;
			animBlender.enabled = false;
			ShowTut();
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

			phase ++;
			CompletedAction();

			yield return new WaitForSeconds(1.5f);
			blenderTopPart.enabled = false;
			fruitChops[0].enabled = false;
			fruitChops[1].enabled = false;
			fruitChops[2].enabled = false;
			animBlender.enabled = true;
			animBlender.Play("hideBlender");
			if( SoundManager.Instance!=null)  SoundManager.Instance.Play_Sound( SoundManager.Instance.ShowItem);
			GameObject.Destroy(animBlender.gameObject,2);

			yield return new WaitForSeconds(0.9f);
			//prikazi kasiku
			animSpoon.Play("showSpoon");
			if( SoundManager.Instance!=null)  SoundManager.Instance.Play_Sound( SoundManager.Instance.ShowItem);
			animSpoon.GetComponent<Spoon>().enabled = true;
			yield return new WaitForSeconds(.8f);
			animSpoon.StopPlayback();
			animSpoon.enabled = false;
			ShowTut();
		}
		else  if(phaseState == "FeedBaby"  )
		{
			babyC.BabyIdle();
			phase ++;
			CompletedAction();
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

			DragItem.OneItemEnabledNo = 4;
			ShowTut();
		}

		else  if(phaseState == "CleanBaby"  )
		{
			Tutorial.Instance.StopTutorial();
			phase ++;
			CompletedAction();
			 
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


	public void ShowChoppedFruitBlender(int fruitNo)
	{
		fruitChops[ fruitCount-1].enabled = true;
		fruitChops[ fruitCount-1].sprite = spriteFruitChops[(fruitNo-1)];
	}

	private void ShowTut()
	{
		if(_completedActionNo == 1 ) Tutorial.Instance.ShowTutorial(1);
		else if( _completedActionNo == 2 ) Tutorial.Instance.ShowTutorial(2);
		else if( _completedActionNo == 3 ) Tutorial.Instance.ShowTutorial(3);
		else if( _completedActionNo == 4 ) Tutorial.Instance.ShowTutorial(4);
 
	}
 
	public void CompletedAction()
	{
		_completedActionNo++;
		progressBar.SetProgress(_completedActionNo/5f , true );
		
		if(_completedActionNo == 5)
		{
			if( SoundManager.Instance!=null)  SoundManager.Instance.Play_Sound( SoundManager.Instance.MinigameCompleted);
			StartCoroutine(nameof(LevelCompleted));
			babyC.BabySmile();
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
		_completedActionNo = 0;
		if(SoundManager.Instance!=null) SoundManager.Instance.Play_ButtonClick();
		StopAllCoroutines();
		GameData.BCompletedMiniGame = false;
		LevelTransition.Instance.HideSceneAndLoadNext("SelectMinigame"); 
		BlockClicks.Instance.SetBlockAll(true);
		//Implementation.Instance.ShowInterstitial();
	}

	public void  ButtonNextClicked()
	{
		_completedActionNo = 0;
		if(SoundManager.Instance!=null) SoundManager.Instance.Play_ButtonClick();
		BlockClicks.Instance.SetBlockAll(true);
		LevelTransition.Instance.HideSceneAndLoadNext("SelectMinigame");
		StopAllCoroutines();
		//Implementation.Instance.ShowInterstitial();
	}
}
