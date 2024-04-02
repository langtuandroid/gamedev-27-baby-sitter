using UnityEngine;
using System.Collections;
using TemplateScripts;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Minigame6 : MonoBehaviour {

	[SerializeField] private ProgressBar progressBar;

	private static int _completedActionNo = 0;
	
	[FormerlySerializedAs("ButtonNext")] [SerializeField] private GameObject buttonNext;
	[FormerlySerializedAs("ButtonHome")] [SerializeField] private GameObject buttonHome;
 
	[SerializeField] private Image[] toysShelf;
	[SerializeField] private Image[] toys;
	[SerializeField] private Sprite[] toysSprites;
	
	[FormerlySerializedAs("StartPositions")] [SerializeField] private Transform[] startPositions;
	[FormerlySerializedAs("Shelf1Positions")] [SerializeField] private Transform[] shelf1Positions;
	[FormerlySerializedAs("Shelf2Positions")] [SerializeField] private Transform[] shelf2Positions;
	[FormerlySerializedAs("Shelf3Positions")] [SerializeField] private Transform[] shelf3Positions;

	private int toyImgA, toyImgB, toyImgC;
	[SerializeField] private ParticleSystem psLevelCompleted;

	private void Awake()
	{
		buttonNext.SetActive(false);
	}
	
	private IEnumerator Start () {


		Transform pom;
		int a,b;
		for(int i = 0; i<100;i++)
		{
		 	a = Random.Range(0,startPositions.Length);
			pom = startPositions[a];
			b = Random.Range(0,startPositions.Length);
			startPositions[a] = startPositions[b];
			startPositions[b] = pom;
		}

		toyImgA = Random.Range(0,toysSprites.Length);

		toyImgB = Random.Range(0,toysSprites.Length);
		while(toyImgA == toyImgB)
		{
			toyImgB = Random.Range(0,toysSprites.Length);
		}

		toyImgC = Random.Range(0,toysSprites.Length);
		while(toyImgC == toyImgB || toyImgC == toyImgA)
		{
			toyImgC = Random.Range(0,toysSprites.Length);
		}



		for(int i = 0; i<toys.Length;i++)
		{
			toys[i].transform.parent.SetParent(startPositions[i]);
			toys[i].transform.parent.localPosition = Vector3.zero;
			MoveToy mt = toys[i].transform.parent.GetComponent<MoveToy>();
			if(i<3)
			{
				toys[i].sprite = toysSprites[toyImgA];
				mt.ShelfIndex = 1;
				mt.TargetPoint = shelf1Positions;
			}
			else if(i<6)
			{
				toys[i].sprite = toysSprites[toyImgB];
				mt.ShelfIndex = 2;
				mt.TargetPoint = shelf2Positions;
			}
			else
			{
				toys[i].sprite = toysSprites[toyImgC];
				mt.ShelfIndex = 3;
				mt.TargetPoint = shelf3Positions;
			}
		}

		toysShelf[0].sprite =  toysSprites[toyImgA];
		toysShelf[1].sprite =  toysSprites[toyImgB];
		toysShelf[2].sprite =  toysSprites[toyImgC];

		toysShelf[0].transform.parent.SetParent(shelf1Positions[Random.Range(0,shelf1Positions.Length)]);
		toysShelf[1].transform.parent.SetParent(shelf2Positions[Random.Range(0,shelf2Positions.Length)]);
		toysShelf[2].transform.parent.SetParent(shelf3Positions[Random.Range(0,shelf3Positions.Length)]);

		toysShelf[0].transform.parent.localPosition = Vector3.zero;
		toysShelf[1].transform.parent.localPosition = Vector3.zero;
		toysShelf[2].transform.parent.localPosition = Vector3.zero;

		//topMenu.SetMenuItems(1);
		_completedActionNo = 0;
		
		
		BlockClicks.Instance.SetBlockAll(true);
		BlockClicks.Instance.SetBlockAllDelay(.2f,false);
		
		yield return new WaitForSeconds(1);
		LevelTransition.Instance.ShowScene();
		
		
		//podesi bebicu 
		//Debug.Log("Selektovana bebica:  " +  (GameData.selectedMinigameIndex+1) );

		Tutorial.Instance.tutStartPos[0].position = toys[5].transform.position + new Vector3(0,0.2f,0);

		int deadloop = 0;
		int tutPosIndex=   Random.Range(0,shelf2Positions.Length);
		while(shelf2Positions[tutPosIndex]  .childCount>0 && deadloop<1000)
		{
			tutPosIndex=   Random.Range(0,shelf2Positions.Length) ;
		}
		                            
		Tutorial.Instance.tutEndPos[0].position =shelf2Positions[tutPosIndex] .position+ new Vector3(0,0.2f,0);
		Tutorial.Instance.ShowTutorial(0);
	}

	public void CompletedAction()
	{
 
		_completedActionNo++;
		progressBar.SetProgress(_completedActionNo/9f , true );
		
	 
		if(_completedActionNo == 9)
		{
			if( SoundManager.Instance!=null)  SoundManager.Instance.Play_Sound( SoundManager.Instance.MinigameCompleted);
			StartCoroutine(nameof(LevelCompleted));
		}
		else 
		{
			if( SoundManager.Instance!=null)  SoundManager.Instance.Play_Sound( SoundManager.Instance.ActionCompleted);
		}
		 
	}
	
	public void NextPhase(   )
	{
		CompletedAction();
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

	public void ButtonNextClicked()
	{
		_completedActionNo = 0;
		BlockClicks.Instance.SetBlockAll(true);
		if(SoundManager.Instance!=null) SoundManager.Instance.Play_ButtonClick();
		LevelTransition.Instance.HideSceneAndLoadNext("SelectMinigame");
		StopAllCoroutines();
		//Implementation.Instance.ShowInterstitial();
	}
}
