using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TemplateScripts;

public class Minigame5B : MonoBehaviour {
	
	private int fruitCount; 
	[SerializeField] private ProgressBar progressBar;

	private static int _completedActionNo = 0;
	
	[SerializeField] private GameObject buttonNext;
	[SerializeField] private GameObject buttonHome;

	private int phase = 0;
	
	[SerializeField] private Animator animDish; 
	[SerializeField] private Animator animSpoon;
	[SerializeField] private Transform milk; 
	[SerializeField] private Transform cereal; 
	[SerializeField] private Transform fruitBowl; 
 
	[SerializeField] private CanvasGroup cgTissue; 
	[SerializeField] private Image [] dishFood;
	[SerializeField] private Transform dishFoodEndPos;
	[SerializeField] private BabyController babyC;
	[SerializeField] private ParticleSystem psLevelCompleted; 
	
	private void Awake()
	{
		buttonNext.SetActive(false);
	}
	
	private IEnumerator Start () 
	{
		DragItem.OneItemEnabledNo =1;
		_completedActionNo = 0;
		
		BlockClicks.Instance.SetBlockAll(true);
		BlockClicks.Instance.SetBlockAllDelay(.2f,false);

		yield return new WaitForSeconds(.1f);
		
		babyC.GetComponent<SetBabyAtlas>().SetBaby(GameData.GetSelectedBaby());
		babyC.BabyIdle();

		yield return new WaitForSeconds(1);
		LevelTransition.Instance.ShowScene();
		Tutorial.Instance.ShowTutorial(0);

	}

	private void ShowTut()
	{
		if(_completedActionNo == 1 ) Tutorial.Instance.ShowTutorial(1);
		else if( _completedActionNo == 2 ) Tutorial.Instance.ShowTutorial(2);
		else if( _completedActionNo == 3 ) Tutorial.Instance.ShowTutorial(3);
		else if( _completedActionNo == 4 ) Tutorial.Instance.ShowTutorial(4);
		else if( _completedActionNo == 5 ) Tutorial.Instance.ShowTutorial(5);
	}
	
	public void NextPhase( string phaseState )
	{
		StartCoroutine(nameof(WaitNextPhase),phaseState);
	}

	private IEnumerator WaitNextPhase( string phaseState )
	{
		//Debug.Log(phaseState +"   @");
		if(phaseState == "InsertMilk"  )
		{
			dishFood[0].transform.gameObject.SetActive( true);
			
			float pom = 0;
			Vector3 sPos = dishFood[0].transform.position;
			
			
			while(pom<1)
			{
				pom+=Time.fixedDeltaTime;
				dishFood[0].transform.position = Vector3.Lerp(sPos, dishFoodEndPos.position, pom);
				dishFood[0].transform.localScale =  Vector3.one * (.8f+ .2f*pom ) ;
				yield return new WaitForFixedUpdate();
			}
			
			phase ++;
			CompletedAction();
			//hide milk
			//show cereal
			yield return new WaitForSeconds(2);
			if( SoundManager.Instance!=null)  SoundManager.Instance.Play_Sound( SoundManager.Instance.ShowItem);
			 pom = 0;
			 sPos = milk.position;
			while(pom<1)
			{
				pom+=Time.fixedDeltaTime*2;
				milk.position = Vector3.Lerp(sPos, cereal.position, pom);
				yield return new WaitForFixedUpdate();
			}
			
			milk.gameObject.SetActive(false);
			if( SoundManager.Instance!=null)  SoundManager.Instance.StopAndPlay_Sound( SoundManager.Instance.ShowItem,.4f);
			pom = 0;
			while(pom<1)
			{
				pom+=Time.fixedDeltaTime;
				cereal.position = Vector3.Lerp(milk.position , sPos, pom);
				yield return new WaitForFixedUpdate();
			}
			DragItem.OneItemEnabledNo = 2;
			ShowTut();

		}
		else if(phaseState == "InsertCereal"  )
		{
			 
			phase ++;
			CompletedAction();
			 
			yield return new WaitForSeconds(2);
			float pom = 0;
			Vector3 sPos = cereal.position;
			if( SoundManager.Instance!=null)  SoundManager.Instance.Play_Sound( SoundManager.Instance.ShowItem);
			while(pom<1)
			{
				pom+=Time.fixedDeltaTime*2;
				cereal.position = Vector3.Lerp(sPos, fruitBowl.position, pom);
				yield return new WaitForFixedUpdate();
			}

			cereal.gameObject.SetActive(false);
			if( SoundManager.Instance!=null)  SoundManager.Instance.StopAndPlay_Sound( SoundManager.Instance.ShowItem,.4f); 
			pom = 0;
			while(pom<1)
			{
				pom+=Time.fixedDeltaTime;
				fruitBowl.position = Vector3.Lerp(cereal.position , sPos, pom);
				yield return new WaitForFixedUpdate();
			}
			DragItem.OneItemEnabledNo = 3;
			ShowTut();
		}
		else if(phaseState == "InsertFruits" )
		{
		 
			phase ++;
			CompletedAction();
			 
			yield return new WaitForSeconds(2);
			float pom = 0;
			Vector3 sPos = fruitBowl.position;
			if( SoundManager.Instance!=null)  SoundManager.Instance.Play_Sound( SoundManager.Instance.ShowItem);
			while(pom<1)
			{
				pom+=Time.fixedDeltaTime*2;
				fruitBowl.position = Vector3.Lerp(sPos, cereal.position, pom);
				yield return new WaitForFixedUpdate();
			}
			
			fruitBowl.gameObject.SetActive(false);

			if( SoundManager.Instance!=null)  SoundManager.Instance.StopAndPlay_Sound( SoundManager.Instance.ShowItem,.4f);
	
			animSpoon.Play("showSpoon");
			animSpoon.GetComponent<Spoon>().enabled = true;
			yield return new WaitForSeconds(.8f);
			animSpoon.StopPlayback();
			animSpoon.enabled = false;
			animSpoon.GetComponent< Spoon>().bMixingFood = true;
			ShowTut();
		}

		else if(phaseState == "MixedFood" )
		{
			StartCoroutine("WaitChangeSprite",4);
			animSpoon.GetComponent< Spoon>().bMixingFood = false;
			phase ++;
			CompletedAction();
			yield return new WaitForSeconds(1.2f);
			ShowTut();
		}

		 
 
		else  if(phaseState == "FeedBaby"  )
		{

			babyC.BabyIdle();
			phase ++;
			CompletedAction();
			animSpoon.enabled = true;
			
			animSpoon.Play("hideSpoon");
			GameObject.Destroy(animSpoon.gameObject,1);

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
	

	public void ChangeSpriteCereal(int _index)
	{
		StartCoroutine(nameof(WaitChangeSprite),_index);
	}

	private IEnumerator WaitChangeSprite(int _index)
	{
		float pom = 0;
		if(_index == 1)
		{
			dishFood[1].gameObject.SetActive(true);
			while(pom<1)
			{
				pom+= Time.fixedDeltaTime*4;
				dishFood[1].gameObject.SetActive(true);
				dishFood[1].color = new Color(1,1,1,pom);
				yield return new WaitForFixedUpdate();
			}
			dishFood[1].color  = Color.white;
			dishFood[0].gameObject.SetActive(false);
		}
		else if(_index == 2)
		{
			dishFood[2].gameObject.SetActive(true);
			while(pom<1)
			{
				pom+= Time.fixedDeltaTime*4;
				dishFood[2].gameObject.SetActive(true);
				dishFood[2].color = new Color(1,1,1,pom);
				yield return new WaitForFixedUpdate();
			}
			dishFood[2].color  = Color.white;
			dishFood[1].gameObject.SetActive(false);
		}

		else if(_index == 3)
		{
			dishFood[3].gameObject.SetActive(true);
			while(pom<1)
			{
				pom+= Time.fixedDeltaTime*4;
				dishFood[3].gameObject.SetActive(true);
				dishFood[3].color = new Color(1,1,1,pom);
				yield return new WaitForFixedUpdate();
			}
			dishFood[3].color  = Color.white;
			dishFood[2].gameObject.SetActive(false);	
		}
		else if(_index == 4)
		{
			dishFood[3].gameObject.SetActive(true);
			while(pom<1)
			{
				pom+= Time.fixedDeltaTime*4;
				dishFood[4].gameObject.SetActive(true);
				dishFood[4].color = new Color(1,1,1,pom);
				yield return new WaitForFixedUpdate();
			}
			dishFood[4].color  = Color.white;
			dishFood[3].gameObject.SetActive(false);
		} 
	}

	public void ChangeSpriteFruitsBowl()
	{
		StartCoroutine(nameof(WaitChangeSprite),3);
	}
	
	public void CompletedAction()
	{
		_completedActionNo++;
		progressBar.SetProgress(_completedActionNo/6f , true );
		
		if(_completedActionNo == 6)
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
		BlockClicks.Instance.SetBlockAll(true);
		if(SoundManager.Instance!=null) SoundManager.Instance.Play_ButtonClick();
		LevelTransition.Instance.HideSceneAndLoadNext("SelectMinigame");
		StopAllCoroutines();
		//Implementation.Instance.ShowInterstitial();
	}
}
