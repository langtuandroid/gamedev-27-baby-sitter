using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TemplateScripts;

public class Minigame5BBS : MonoBehaviour {
	
	private int fruitCountT; 
	[SerializeField] private ProgressBar progressBar;

	private static int _completedActionNoN = 0;
	
	[SerializeField] private GameObject buttonNext;
	[SerializeField] private GameObject buttonHome;

	private int phaseE = 0;
	
	[SerializeField] private Animator animDish; 
	[SerializeField] private Animator animSpoon;
	[SerializeField] private Transform milk; 
	[SerializeField] private Transform cereal; 
	[SerializeField] private Transform fruitBowl; 
 
	[SerializeField] private CanvasGroup cgTissue; 
	[SerializeField] private Image [] dishFood;
	[SerializeField] private Transform dishFoodEndPos;
	[SerializeField] private BabyControllerBs babyC;
	[SerializeField] private ParticleSystem psLevelCompleted; 
	
	private void Awake()
	{
		buttonNext.SetActive(false);
	}
	
	private IEnumerator Start () 
	{
		DragItemBS.OneItemEnabledNo =1;
		_completedActionNoN = 0;
		
		BlockClicksBs.Instance.SetBlockAllL(true);
		BlockClicksBs.Instance.SetBlockAllDelayY(.2f,false);

		yield return new WaitForSeconds(.1f);
		
		babyC.GetComponent<SetBabyAtlasBS>().SetBabyY(GameDataBS.GetSelectedBabyY());
		babyC.BabyIdle();

		yield return new WaitForSeconds(1);
		LevelTransitionBS.Instance.ShowScene();
		TutorialBS.Instance.ShowTutorial(0);

	}

	private void ShowTutorial()
	{
		if(_completedActionNoN == 1 ) TutorialBS.Instance.ShowTutorial(1);
		else if( _completedActionNoN == 2 ) TutorialBS.Instance.ShowTutorial(2);
		else if( _completedActionNoN == 3 ) TutorialBS.Instance.ShowTutorial(3);
		else if( _completedActionNoN == 4 ) TutorialBS.Instance.ShowTutorial(4);
		else if( _completedActionNoN == 5 ) TutorialBS.Instance.ShowTutorial(5);
	}
	
	public void NextPhaseE( string phaseState )
	{
		StartCoroutine(nameof(WaitNextPhaseE),phaseState);
	}

	private IEnumerator WaitNextPhaseE( string phaseState )
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
			
			phaseE ++;
			CompletedActionN();
			//hide milk
			//show cereal
			yield return new WaitForSeconds(2);
			if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.PlaySound( SoundManagerBS.Instance.ShowItemM);
			 pom = 0;
			 sPos = milk.position;
			while(pom<1)
			{
				pom+=Time.fixedDeltaTime*2;
				milk.position = Vector3.Lerp(sPos, cereal.position, pom);
				yield return new WaitForFixedUpdate();
			}
			
			milk.gameObject.SetActive(false);
			if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.StopAndPlay_Sound( SoundManagerBS.Instance.ShowItemM,.4f);
			pom = 0;
			while(pom<1)
			{
				pom+=Time.fixedDeltaTime;
				cereal.position = Vector3.Lerp(milk.position , sPos, pom);
				yield return new WaitForFixedUpdate();
			}
			DragItemBS.OneItemEnabledNo = 2;
			ShowTutorial();

		}
		else if(phaseState == "InsertCereal"  )
		{
			 
			phaseE ++;
			CompletedActionN();
			 
			yield return new WaitForSeconds(2);
			float pom = 0;
			Vector3 sPos = cereal.position;
			if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.PlaySound( SoundManagerBS.Instance.ShowItemM);
			while(pom<1)
			{
				pom+=Time.fixedDeltaTime*2;
				cereal.position = Vector3.Lerp(sPos, fruitBowl.position, pom);
				yield return new WaitForFixedUpdate();
			}

			cereal.gameObject.SetActive(false);
			if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.StopAndPlay_Sound( SoundManagerBS.Instance.ShowItemM,.4f); 
			pom = 0;
			while(pom<1)
			{
				pom+=Time.fixedDeltaTime;
				fruitBowl.position = Vector3.Lerp(cereal.position , sPos, pom);
				yield return new WaitForFixedUpdate();
			}
			DragItemBS.OneItemEnabledNo = 3;
			ShowTutorial();
		}
		else if(phaseState == "InsertFruits" )
		{
		 
			phaseE ++;
			CompletedActionN();
			 
			yield return new WaitForSeconds(2);
			float pom = 0;
			Vector3 sPos = fruitBowl.position;
			if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.PlaySound( SoundManagerBS.Instance.ShowItemM);
			while(pom<1)
			{
				pom+=Time.fixedDeltaTime*2;
				fruitBowl.position = Vector3.Lerp(sPos, cereal.position, pom);
				yield return new WaitForFixedUpdate();
			}
			
			fruitBowl.gameObject.SetActive(false);

			if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.StopAndPlay_Sound( SoundManagerBS.Instance.ShowItemM,.4f);
	
			animSpoon.Play("showSpoon");
			animSpoon.GetComponent<SpoonBS>().enabled = true;
			yield return new WaitForSeconds(.8f);
			animSpoon.StopPlayback();
			animSpoon.enabled = false;
			animSpoon.GetComponent< SpoonBS>().bMixingFood = true;
			ShowTutorial();
		}

		else if(phaseState == "MixedFood" )
		{
			StartCoroutine(nameof(WaitChangeSpriteE),4);
			animSpoon.GetComponent< SpoonBS>().bMixingFood = false;
			phaseE ++;
			CompletedActionN();
			yield return new WaitForSeconds(1.2f);
			ShowTutorial();
		}

		 
 
		else  if(phaseState == "FeedBaby"  )
		{

			babyC.BabyIdle();
			phaseE ++;
			CompletedActionN();
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
			DragItemBS.OneItemEnabledNo = 4;
			ShowTutorial();
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
	

	public void ChangeSpriteCereal(int _index)
	{
		StartCoroutine(nameof(WaitChangeSpriteE),_index);
	}

	private IEnumerator WaitChangeSpriteE(int _index)
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

	public void ChangeSpriteFruitsBowlL()
	{
		StartCoroutine(nameof(WaitChangeSpriteE),3);
	}
	
	public void CompletedActionN()
	{
		_completedActionNoN++;
		progressBar.SetProgressBar(_completedActionNoN/6f , true );
		
		if(_completedActionNoN == 6)
		{
			if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.PlaySound( SoundManagerBS.Instance.MinigameCompleted);
			StartCoroutine(nameof(LevelCompletedD));
			babyC.BabySmile();
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
		LevelTransitionBS.Instance.HideSceneAndLoadNext("SelectMinigame"); 
		BlockClicksBs.Instance.SetBlockAllL(true);
		//Implementation.Instance.ShowInterstitial();
	}

	public void ButtonNextClickedD()
	{
		_completedActionNoN = 0;
		BlockClicksBs.Instance.SetBlockAllL(true);
		if(SoundManagerBS.Instance!=null) SoundManagerBS.Instance.Play_ButtonClickK();
		LevelTransitionBS.Instance.HideSceneAndLoadNext("SelectMinigame");
		StopAllCoroutines();
		//Implementation.Instance.ShowInterstitial();
	}
}
